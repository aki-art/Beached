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
                        elements.Add(Elements.SaltyOxygen);
                    }

                    safe_elements = elements.ToArray();
                }
            }
        }

        [HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicCreature")]
        public class EntityTemplates_ExtendEntityToBasicCreature_Patch
        {
            public static void Postfix(GameObject template)
            {
                Log.Debug("extend to entity " + template.GetProperName());
                var vulnerability = ModDb.GetAcidVulnerability(template);
                Log.Debug("vulnerability " + vulnerability);
                if (vulnerability > 0)
                {
                    Log.Debug("configured acid on " + template.PrefabID());
                    template.AddOrGet<AcidVulnerable>().acidDamage = vulnerability;
                }
            }
        }
    }
}
