using Beached.Content.DefBuilders;
using Klei.AI;
using TUNING;

namespace Beached.Content.ModDb.Germs
{
	public class IceWrathGerms : Disease
	{
		public const string ID = "Beached_IceWrath";
		public const float RAD_KILL_RATE = 1.0f;
		public float UVHalfLife => 1f; // UV Lamps compat


		private static readonly RangeInfo temperatureRangeInfo = MiscUtil.RangeInfoCelsius(-100, -60, -10, 10);
		private static readonly RangeInfo temperatureHalfLivesInfo = new(12000f, 1200f, 600f, 10f);
		private static readonly RangeInfo pressureRangeInfo = new(0f, 0f, 1000f, 1000f);

		public IceWrathGerms(bool statsOnly) : base(
			ID,
			20,
			temperatureRangeInfo,
			temperatureHalfLivesInfo,
			pressureRangeInfo,
			RangeInfo.Idempotent(),
			RAD_KILL_RATE,
			statsOnly)
		{
			overlayColourName = ID;
		}

		public override void PopulateElemGrowthInfo()
		{
			InitializeElemGrowthArray(ref elemGrowthInfo, DEFAULT_GROWTH_INFO);

			var growth = new GrowthInfoBuilder(this)
				.DefaultBehavior(
					DISEASE.UNDERPOPULATION_DEATH_RATE.NONE,
					1.4f,
					6000f,
					0.4f,
					500,
					3000,
					1f / 2000f,
					1)
				.DiesIn(Element.State.Liquid)
				.InstantlyDiesIn(Elements.sulfurousWater)
				.DiesAndSlowsOnSolid()
				.GrowsFastIn(SimHashes.Ice, SimHashes.DirtyIce, SimHashes.BrineIce, SimHashes.BrineIce, SimHashes.MilkIce, Elements.sourBrineIce, Elements.mucusFrozen)
				.DisinfectedBy(SimHashes.ChlorineGas, SimHashes.BleachStone, SimHashes.Ethanol);
		}
	}
}
