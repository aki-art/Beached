using Beached.Content;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
    public class SteppedInMonitorPatch
    {

        [HarmonyPatch(typeof(SteppedInMonitor), "GetSoaked", typeof(SteppedInMonitor.Instance))]
        public class SteppedInMonitor_GetSoaked_Patch
        {
            public static bool Prefix(SteppedInMonitor.Instance smi)
            {
                int num = Grid.CellAbove(Grid.PosToCell(smi));
                if (Grid.IsValidCell(num) && Grid.Element[num].id == Elements.Mucus)
                {
                    smi.effects.Remove("CarpetFeet");
                    smi.effects.Remove("WetFeet");

                    smi.effects.Add(BEffects.MUCUS_SOAKED, true);

                    return false;
                }

                smi.effects.Remove(BEffects.MUCUS_SOAKED);

                return true;
            }
        }

        // TODO: possibly a transpiler for speed
        [HarmonyPatch(typeof(SteppedInMonitor), "IsOnCarpet")]
        public class SteppedInMonitor_IsOnCarpet_Patch
        {
            public static void Postfix(ref bool __result, SteppedInMonitor.Instance smi)
            {
                if(!__result)
                {
                    var cell = Grid.CellBelow(Grid.PosToCell(smi));
                    __result = Grid.IsValidCell(cell) && Grid.Element[cell].id == Elements.Moss;
                }
            }
        }
    }
}
