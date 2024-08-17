using HarmonyLib;

namespace Beached.Patches
{
	internal class ExternalTemperatureMonitorPatch
	{
		private const float DEFAULT_WARM_AIR = 298.15f; // 25C
		private const float DEFAULT_WARM_LIQUID = 308.15f; // 35C

		// do not set a duplicant in warm weather feeling like they are too cold
		[HarmonyPatch(typeof(ExternalTemperatureMonitor.Instance), "IsTooCold")]
		public class ExternalTemperatureMonito_Instance_IsTooCold_Patch
		{
			public static void Postfix(ExternalTemperatureMonitor.Instance __instance, ref bool __result)
			{
				var cell = Grid.PosToCell(__instance);
				__result &= (float)Grid.Temperature[cell] > GetTemperatureTreshold(__instance, cell);
			}
		}

		private static float GetTemperatureTreshold(ExternalTemperatureMonitor.Instance monitor, int cell) => Grid.IsLiquid(cell)
			? DEFAULT_WARM_LIQUID
			: DEFAULT_WARM_AIR;
	}
}
