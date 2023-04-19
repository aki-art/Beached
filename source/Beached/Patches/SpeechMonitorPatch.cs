using HarmonyLib;
namespace Beached.Patches
{
    public class SpeechMonitorPatch
    {
        [HarmonyPatch(typeof(SpeechMonitor), "BeginTalking")]
        public class SpeechMonitor_BeginTalking_Patch
        {
            public static void Postfix(SpeechMonitor.Instance smi)
            {
                smi.ev.setPitch(10f);
            }
        }
    }
}
