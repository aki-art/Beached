using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
    public class DrowningMonitorPatch
    {
        //[HarmonyPatch(typeof(DrowningMonitor), "CellSafeTest")]
        public class DrowningMonitor_CellSafeTest_Patch
        {
            public static bool Prefix(DrowningMonitor __instance, int testCell, ref bool __result)
            {
                if (__instance != null && __instance.HasTag(BTags.aquatic))
                {
                    var cellAbove = Grid.CellAbove(testCell);
                    __result = Grid.IsValidCell(testCell)
                            && Grid.IsValidCell(cellAbove)
                            && Grid.IsSubstantialLiquid(testCell, 0.95f)
                            && !Grid.Element[cellAbove].IsSolid;

                    return false;
                }

                return true;
            }
        }
    }
}
