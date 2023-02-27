using Beached.Content;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Beached.Patches
{
    public class EntityTemplatesPatch
    {
        // if a plant can survive in oxygen, allow it to live in salty oxygen as well
        [HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicPlant")]
        public class EntityTemplates_ExtendEntityToBasicPlant_Patch
        {
            public static void Prefix(ref SimHashes[] safe_elements)
            {
                if (safe_elements != null)
                {
                    var elements = safe_elements.ToHashSet();
                    if (elements.Contains(SimHashes.Oxygen))
                    {
                        elements.Add(Elements.saltyOxygen);
                    }

                    safe_elements = elements.ToArray();
                }
            }
        }


        [HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToFood")]
        public class EntityTemplates_ExtendEntityToFood_Patch
        {
            public static void Postfix(GameObject template)
            {
                if (Smokable.smokables.TryGetValue(template.PrefabID(), out var config))
                {
                    var smokable = template.AddOrGet<Smokable>();
                    smokable.cyclesToSmoke = config.time;
                    smokable.smokedItemTag = config.smokedItem;
                }
            }
        }
    }
}
