using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
    public class RationaAIPatch
    {
        // Extend duplicant AI
        [HarmonyPatch(typeof(RationalAi), "InitializeStates")]
        public class RationalAi_InitializeStates_Patch
        {
            public static void Postfix(RationalAi __instance)
            {
                __instance.alive
                    .ToggleStateMachine(smi => new FreshAirMonitor.Instance(smi.master));
                    //.ToggleStateMachine(smi => new ScaredMonitor.Instance(smi.master));
            }
        }
    }
}
