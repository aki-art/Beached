using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
    public class GasBreatherFromWorldProviderPatch
    {
        [HarmonyPatch(typeof(GasBreatherFromWorldProvider), "OnSimConsume")]
        public class GasBreatherFromWorldProvider_OnSimConsume_Patch
        {
            // Trigger events for atmospheres
            // Liquids are included for Minnow's waterbreathing ability
            public static void Postfix(Sim.MassConsumedCallback mass_cb_info, OxygenBreather ___oxygenBreather)
            {
                var id = ElementLoader.elements[mass_cb_info.elemIdx].id;
                if (id == Elements.SaltyOxygen || id == SimHashes.SaltWater)
                {
                    ___oxygenBreather.Trigger((int)ModHashes.GreatAirQuality, mass_cb_info);
                }
                else if (id == SimHashes.DirtyWater || id == Elements.MurkyBrine)
                {
                    ___oxygenBreather.Trigger((int)GameHashes.PoorAirQuality, mass_cb_info);
                }
            }
        }
    }
}
