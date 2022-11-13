using Beached.Content;
using Beached.Content.ModDb;
using HarmonyLib;
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
    }
}
