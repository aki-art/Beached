using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	internal class WorkablePatch
	{
		[HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier")]
		public class Workable_GetEfficiencyMultiplier_Patch
		{
			[HarmonyPriority(Priority.LowerThanNormal)]
			public static void Postfix(Workable __instance, ref float __result)
			{
				var operatingSpeed = BAttributes.operatingSpeed.Lookup(__instance);
				if (operatingSpeed != null)
					__result *= operatingSpeed.GetTotalValue();
			}
		}
	}
}
