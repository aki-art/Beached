using Beached.Content;
using Beached.Utils;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
    public class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load_Patch
        {
            public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                // Add my new elements
                var list = substanceTablesByDlc[DlcManager.VANILLA_ID].GetList();
                Elements.RegisterSubstances(list);
            }

            public static void Postfix()
            {
                // fix a bug with tags, IMPORTANT FOR NEW ELEMENTS
                ElementUtil.FixTags();

                // recolor water and salt water to be more watery and distinguishable
                var substanceTable = Assets.instance.substanceTable;
                substanceTable.GetSubstance(SimHashes.Water).colour = ModAssets.Colors.water;
                substanceTable.GetSubstance(SimHashes.SaltWater).colour = ModAssets.Colors.saltWater;

                Elements.AfterLoad();
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                foreach (var elem in __result)
                {
                    // change Amber's melting target based on DLC
                    if (elem.elementId == Elements.Amber.ToString() && DlcManager.FeatureRadiationEnabled())
                    {
                        elem.highTempTransitionTarget = SimHashes.Resin.ToString();
                    }
                    else if (elem.elementId == SimHashes.Diamond.ToString())
                    {
                        Mod.Settings.CrossWorld.Elements.originalDiamondCategory = elem.materialCategory;
                    }
                    else if (elem.elementId == SimHashes.Katairite.ToString())
                    {
                        Mod.Settings.CrossWorld.Elements.originalAbyssaliteCategory = elem.materialCategory;
                    }
                    else if (elem.elementId == SimHashes.Lime.ToString())
                    {
                        Mod.Settings.CrossWorld.Elements.originalLimeHighTemp = elem.highTemp;
                        Mod.Settings.CrossWorld.Elements.originalLimeHighTempTarget = elem.highTempTransitionTarget;

                    }
                    // reenable Helium
                    else if (elem.elementId == SimHashes.Helium.ToString())
                    {
                        elem.isDisabled = false;
                    }
                    // reenable Helium
                    else if (elem.elementId == SimHashes.LiquidHelium.ToString())
                    {
                        elem.isDisabled = false;
                    }
                }
            }
        }
    }
}
