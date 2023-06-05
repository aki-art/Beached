using HarmonyLib;
namespace Beached.Patches
{
	public class SpeechMonitorPatch
	{
		[HarmonyPatch(typeof(SpeechMonitor), nameof(SpeechMonitor.BeginTalking))]
		public class SpeechMonitor_BeginTalking_Patch
		{
			public static void Postfix(SpeechMonitor.Instance smi)
			{
				// TODO: if under effect of Helium Poff
				smi.ev.setPitch(10f);
			}
		}
	}
}
