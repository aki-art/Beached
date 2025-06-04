#if BIONIC
using Beached.Patches;
using Klei.AI;
using System;

namespace Beached.Content.Scripts.Entities
{
	// very similar to GasBreatherFromWorldProvider, but also accepts Water tag
	public class AmphibiousOxygenBreatherProvider : OxygenBreather.IGasProvider
	{
		private OxygenBreather oxygenBreather;
		private Navigator nav;
		private float defaultConsumptionRate; // todo: attribute modifier
		private AttributeModifier underWaterConsumption;

		private const int BREATH_RADIUS = 3;
		private const float LIQUID_MULTIPLIER = 5f;

		public BreathableCellData GetBestBreathableCellAtCurrentLocation()
		{
			return GetBestBreathableCellAroundSpecificCell(
				Grid.PosToCell(oxygenBreather),
				GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS,
				oxygenBreather);
		}

		/// DO NOT CHANGE parameters, a transpiler is using this for substitution <see cref="SafeCellQueryPatch.SafeCellQuery_GetFlags_Patch" />
		public static BreathableCellData GetBestBreathableCellAroundSpecificCell(int theSpecificCell, CellOffset[] breathRange, OxygenBreather breather)
		{
			breathRange ??= GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS;

			var highestBreathableMassFound = 0f;
			var targetCell = theSpecificCell;
			var elementFound = SimHashes.Vacuum;
			var isLiquid = false;

			foreach (var offset in breathRange)
			{
				var cell = Grid.OffsetCell(theSpecificCell, offset);
				var pressure = GetBreathableCellMassAndIfLiquid(cell, out var elementID, out var liquid);

				if (pressure > highestBreathableMassFound && pressure > breather.noOxygenThreshold)
				{
					highestBreathableMassFound = pressure;
					targetCell = cell;
					elementFound = elementID;
					isLiquid = liquid;
				}
			}

			return new BreathableCellData
			{
				cell = targetCell,
				elementId = elementFound,
				mass = highestBreathableMassFound,
				isLiquid = isLiquid,
				isBreathable = (elementFound != SimHashes.Vacuum)
			};
		}

		// used for afe cell query original substitution
		public static float GetBreathableCellMass(int cell, out SimHashes elementID)
		{
			elementID = SimHashes.Vacuum;

			if (Grid.IsValidCell(cell))
			{
				var element = Grid.Element[cell];
				if (element.HasTag(GameTags.Breathable) || element.HasTag(GameTags.AnyWater))
				{
					elementID = element.id;

					return Grid.Mass[cell];
				}
			}

			return 0f;
		}

		public static float GetBreathableCellMassAndIfLiquid(int cell, out SimHashes elementID, out bool isLiquid)
		{
			elementID = SimHashes.Vacuum;
			isLiquid = false;

			if (Grid.IsValidCell(cell))
			{
				var element = Grid.Element[cell];
				if (element.HasTag(GameTags.Breathable) || element.HasTag(GameTags.AnyWater))
				{
					elementID = element.id;
					isLiquid = element.IsLiquid;

					return Grid.Mass[cell];
				}
			}

			return 0f;
		}

		public void OnSetOxygenBreather(OxygenBreather breather)
		{
			oxygenBreather = breather;
			nav = oxygenBreather.GetComponent<Navigator>();
			underWaterConsumption = new AttributeModifier(Db.Get().Attributes.AirConsumptionRate.Id, 99f, "Water Breathing", true);
		}

		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		public bool ShouldEmitCO2() => nav.CurrentNavType != NavType.Tube;

		public bool ShouldStoreCO2() => false;

		public bool IsLowOxygen()
		{
			var data = GetBestBreathableCellAtCurrentLocation();
			var multiplier = data.isLiquid ? LIQUID_MULTIPLIER : 1f;
			return data.isBreathable && data.mass < (oxygenBreather.lowOxygenThreshold * multiplier);
		}

		public bool HasOxygen()
		{
			return oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath)
				|| oxygenBreather.prefabID.HasTag(GameTags.InTransitTube)
				|| GetBestBreathableCellAtCurrentLocation().isBreathable;
		}

		public bool IsBlocked() => oxygenBreather.HasTag(GameTags.HasSuitTank);

		public bool ConsumeGas(OxygenBreather breather, float massToConsume, Action<SimHashes, float, float, byte, int> onConsumptionCompletedCallback)
		{
			if (nav.CurrentNavType == NavType.Tube)
				return true;

			var data = GetBestBreathableCellAtCurrentLocation();

			if (!data.isBreathable)
				return false;

			if (data.isLiquid)
			{
				if (!breather.HasTag(BTags.underWater))
				{
					breather.airConsumptionRate.Add(underWaterConsumption);
					breather.AddTag(BTags.underWater);
				}
			}
			else
			{
				if (breather.HasTag(BTags.underWater))
				{
					breather.airConsumptionRate.Remove(underWaterConsumption);
					breather.RemoveTag(BTags.underWater);
				}
			}

			var handle = Game.Instance.massConsumedCallbackManager.Add(
				GasBreatherFromWorldProvider.OnSimConsumeCallback,
				onConsumptionCompletedCallback,
				nameof(AmphibiousOxygenBreatherProvider));

			SimMessages.ConsumeMass(
				data.cell,
				data.elementId,
				massToConsume,
				BREATH_RADIUS,
				handle.index);

			return true;
		}

		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			return ConsumeGas(oxygen_breather, amount, (_, _, _, _, _) => { });
		}

		public struct BreathableCellData
		{
			public int cell;
			public SimHashes elementId;
			public float mass;
			public bool isBreathable;
			public bool isLiquid;
		}
	}
}
#endif