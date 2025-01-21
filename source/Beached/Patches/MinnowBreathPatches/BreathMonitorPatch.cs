#if BIONIC
using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;

namespace Beached.Patches.MinnowBreathPatches
{
	public class BreathMonitorPatch
	{
		// todo: transpile
		[HarmonyPatch(typeof(BreathMonitor), "UpdateRecoverBreathCell", typeof(BreathMonitor.Instance))]
		public class BreathMonitor_UpdateRecoverBreathCell_Patch
		{
			public static bool Prefix(BreathMonitor.Instance smi)
			{
				if (!smi.HasTag(BTags.amphibious))
					return true;

				if (smi.canRecoverBreath)
				{
					smi.query.Reset();
					smi.navigator.RunQuery(smi.query);
					int cell = smi.query.GetResultCell();

					var isBreathable = !AmphibiousOxygenBreatherProvider.GetBestBreathableCellAroundSpecificCell(
						cell,
						GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS,
						smi.breather).isBreathable;

					if (isBreathable)
						cell = PathFinder.InvalidCell;

					smi.sm.recoverBreathCell.Set(cell, smi, false);
				}

				return false;
			}
		}
	}
}
#endif