using Beached.Content;
using HarmonyLib;
using Klei;
using System.Collections.Generic;

namespace Beached.Patches
{
    public class ElementLoaderPatch
    {

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                // change Amber's melting target based on DLC
                if(DlcManager.FeatureRadiationEnabled())
                {
                    var amber = __result.Find(e => e.elementId == Elements.Amber.ToString() && e.highTempTransitionTarget == SimHashes.Carbon.ToString());

                    if (amber != null)
                    {
                        amber.highTempTransitionTarget = SimHashes.Resin.ToString();
                    }
                    else
                    {
                        Log.Warning("Tried to modify Amber but it was not found.");
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load_Patch
        {
            public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                var list = substanceTablesByDlc[DlcManager.VANILLA_ID].GetList();
                Elements.RegisterSubstances(list);
            }

            public static void Postfix()
            {
                // recolor water and salt water to be more watery and distinguishable
                var substanceTable = Assets.instance.substanceTable;
                substanceTable.GetSubstance(SimHashes.Water).colour = ModAssets.Colors.water;
                substanceTable.GetSubstance(SimHashes.SaltWater).colour = ModAssets.Colors.saltWater;
            }
        }
    }
}
