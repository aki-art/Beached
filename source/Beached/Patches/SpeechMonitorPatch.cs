using Beached.Content;
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
				if (smi.HasTag(BTags.heliumPoffed))
					smi.ev.setPitch(10f);
			}
		}
	}
}
