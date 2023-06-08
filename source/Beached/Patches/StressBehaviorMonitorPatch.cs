using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class StressBehaviorMonitorPatch
	{
		[HarmonyPatch(typeof(StressBehaviourMonitor), nameof(StressBehaviourMonitor.InitializeStates))]
		public class StressBehaviourMonitor_InitializeStates_Patch
		{
			public static void Postfix(StressBehaviourMonitor __instance)
			{
			}
		}
	}
}
