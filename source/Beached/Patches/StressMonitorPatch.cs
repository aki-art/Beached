using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class StressMonitorPatch
	{
		[HarmonyPatch(typeof(StressMonitor.Instance), "HasHadEnough")]
		public class StressMonitor_Instance_HasHadEnough_Patch
		{
			public static void Postfix(StressMonitor.Instance __instance, ref bool __result)
			{
				if (__instance.master.gameObject.HasTag(BTags.easilyTriggers))
					__result = __instance.allowStressBreak && __instance.stress.value >= CONSTS.DUPLICANTS.EASILY_TRIGGERED_THRESHOLD;
			}
		}
	}
}
