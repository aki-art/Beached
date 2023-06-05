using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class GasLiquidExposureMonitorPatch
	{
#if ELEMENTS
		[HarmonyPatch(typeof(GasLiquidExposureMonitor), nameof(GasLiquidExposureMonitor.InitializeCustomRates))]
		public class GasLiquidExposureMonitor_InitializeCustomRates_Patch
		{
			public static void Postfix(Dictionary<SimHashes, float> ___customExposureRates)
			{
				if (___customExposureRates != null)
					Elements.SetExposureValues(___customExposureRates);
			}
		}
#endif
	}
}
