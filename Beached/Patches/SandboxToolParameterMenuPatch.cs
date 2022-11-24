using Beached.Content;
using Beached.Content.Defs.Entities;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using static SandboxToolParameterMenu.SelectorValue;

namespace Beached.Patches
{
    public class SandboxToolParameterMenuPatch
    {
        private static readonly HashSet<Tag> tags = new HashSet<Tag>()
        {
            CrystalConfig.ID,
            "GeyserGeneric_" + GeyserConfigs.AMMONIA_VENT,
            "GeyserGeneric_" + GeyserConfigs.MURKY_BRINE_GEYSER,
            "GeyserGeneric_" + GeyserConfigs.BISMUTH_VOLCANO,
        };

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "ConfigureEntitySelector")]
        public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance)
            {
                var sprite = Def.GetUISprite(Assets.GetPrefab(CrabConfig.ID));
                var parent = __instance.entitySelector.filters.First(x => x.Name == global::STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL);

                if (parent == null)
                {
                    return;
                }

                var filter = new SearchFilter("Beached", obj => obj is KPrefabID id && tags.Contains(id.PrefabTag), parent, sprite);

                AddFilters(__instance, filter);
            }


            public static void AddFilters(SandboxToolParameterMenu menu, params SearchFilter[] newFilters)
            {
                var filters = menu.entitySelector.filters;

                if (filters == null)
                {
                    Log.Warning("Filters are null");
                    return;
                }

                Log.Debug("Adding filters here");

                var f = new List<SearchFilter>(filters);
                f.AddRange(newFilters);
                menu.entitySelector.filters = f.ToArray();

                UpdateOptions(menu);
            }

            public static void UpdateOptions(SandboxToolParameterMenu menu)
            {
                var filters = menu.entitySelector.filters;

                if (filters == null)
                {
                    return;
                }

                var options = ListPool<object, SandboxToolParameterMenu>.Allocate();

                foreach (var prefab in Assets.Prefabs)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.condition(prefab))
                        {
                            options.Add(prefab);
                            break;
                        }
                    }
                }

                menu.entitySelector.options = options.ToArray();
                options.Recycle();
            }

            private static SearchFilter FindParent(SandboxToolParameterMenu menu, string parentFilterID)
            {
                return parentFilterID != null ? menu.entitySelector.filters.First(x => x.Name == parentFilterID) : null;
            }
        }
    }
}