using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class SubmersionMonitorPatch
	{
		[HarmonyPatch(typeof(SubmersionMonitor), "IsCellSafe")]
		public class SubmersionMonitor_IsCellSafe_Patch
		{
			public static void Postfix(SubmersionMonitor __instance, ref bool __result)
			{
				if (!__result && __instance.HasTag(BTags.aquariumPlanted))
					__result = true;
			}
		}
	}
}
