using Beached.Content.DefBuilders;
using Klei.AI;
using TUNING;

namespace Beached.Content.ModDb.Germs
{
	internal class PlanktonGerms : Disease
	{
		public const string ID = "Beached_Plankton";
		public const float RAD_KILL_RATE = 2.5f;

		public float UVHalfLife => 1f; // UV Lamps compat

		private static readonly RangeInfo temperatureRangeInfo = MiscUtil.RangeInfoCelsius(0, 10, 60, 75);
		private static readonly RangeInfo temperatureHalfLivesInfo = new(10f, 1200f, 1200f, 10f);
		private static readonly RangeInfo pressureRangeInfo = new(0f, 0f, 1000f, 1000f);

		public PlanktonGerms(bool statsOnly) : base(ID, 20, temperatureRangeInfo, temperatureHalfLivesInfo,
			pressureRangeInfo, RangeInfo.Idempotent(), RAD_KILL_RATE, statsOnly)
		{
			overlayColourName = ID;
		}

		public override void PopulateElemGrowthInfo()
		{
			var growth = new GrowthInfoBuilder(this)
				.DefaultBehavior(
					DISEASE.UNDERPOPULATION_DEATH_RATE.FAST,
					1.4f,
					10_000f,
					0.4f,
					500,
					500,
					1f / 200f,
					1)
				.DiesAndSlowsOnSolid()
				.GrowsIn(GameTags.AnyWater)
				.DisinfectedBy(SimHashes.ChlorineGas, SimHashes.BleachStone, SimHashes.Ethanol);
#if ELEMENTS
			growth
				.InstantlyDiesIn(Elements.sulfurousWater);
#endif

			var exposure = new ExposureRuleBuilder(this)
				.DefaultHalfLife(DISEASE.GROWTH_FACTOR.DEATH_3)
				.DisinfectedBy(SimHashes.ChlorineGas, SimHashes.Ethanol);
#if ELEMENTS
			exposure
				.InstantlyDiesIn(Elements.sulfurousWater);
#endif
		}
	}
}
