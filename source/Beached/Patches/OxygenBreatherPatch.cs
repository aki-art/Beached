using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class OxygenBreatherPatch
	{
		// amphibious breathers (Gills) need to consider liquid pressure too
		[HarmonyPatch(typeof(OxygenBreather), nameof(OxygenBreather.GetOxygenPressure))]
		public class OxygenBreather_GetOxygenPressure_Patch
		{
			public static void Postfix(OxygenBreather __instance, int cell, ref float __result)
			{
				if (!__instance.HasTag(BTags.amphibious))
					return;

				var isInWater = __result == 0
					&& Grid.IsValidCell(cell)
					&& Grid.Element[cell].HasTag(GameTags.AnyWater);

				if (isInWater)
					__result = Grid.Mass[cell];
			}
		}

		// amphibious breathers can breath AnyWater
		[HarmonyPatch(typeof(OxygenBreather), nameof(OxygenBreather.GetBreathableElementAtCell))]
		public class OxygenBreather_GetBreathableElementAtCell_Patch
		{
			public const float NO_WATER_THRESHOLD_MULTIPLIER = 2f;

			public static bool Prefix(OxygenBreather __instance, int cell, CellOffset[] offsets, ref SimHashes __result)
			{
				if (!__instance.HasTag(BTags.amphibious))
					return true;

				offsets ??= __instance.breathableCells;

				var mouthCellAtCell = __instance.GetMouthCellAtCell(cell, offsets);

				if (!Grid.IsValidCell(mouthCellAtCell))
				{
					__result = SimHashes.Vacuum;
					return false;
				}

				var element = Grid.Element[mouthCellAtCell];
				var mass = Grid.Mass[mouthCellAtCell];

				if (element.IsGas && element.HasTag(GameTags.Breathable) && mass > __instance.noOxygenThreshold)
				{
					__result = element.id;
					return false;
				}

				if (element.IsLiquid && element.HasTag(GameTags.AnyWater) && mass > __instance.noOxygenThreshold * NO_WATER_THRESHOLD_MULTIPLIER)
				{
					__result = element.id;
					return false;
				}

				__result = SimHashes.Vacuum;

				return false;
			}
		}
	}
}
