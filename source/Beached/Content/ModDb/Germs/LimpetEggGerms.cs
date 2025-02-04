using Beached.Content.DefBuilders;
using Klei.AI;
using TUNING;

namespace Beached.Content.ModDb.Germs
{
	public class LimpetEggGerms : Disease
	{
		public const string ID = "Beached_LimpetEgg";
		public const float RAD_KILL_RATE = 2.5f;

		public float UVHalfLife => 2f; // UV Lamps compat

		private static readonly RangeInfo temperatureRangeInfo = MiscUtil.RangeInfoCelsius(0, 15, 65, 80);
		private static readonly RangeInfo temperatureHalfLivesInfo = new(10f, 1200f, 1200f, 10f);
		private static readonly RangeInfo pressureRangeInfo = new(0f, 0f, 1000f, 1000f);


		public LimpetEggGerms(bool statsOnly) : base(ID, 20, temperatureRangeInfo, temperatureHalfLivesInfo,
			pressureRangeInfo, RangeInfo.Idempotent(), RAD_KILL_RATE, statsOnly)
		{
			overlayColourName = ID;
		}

		public override void PopulateElemGrowthInfo()
		{
			InitializeElemGrowthArray(ref elemGrowthInfo, DEFAULT_GROWTH_INFO);

			new GrowthInfoBuilder(this)
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
				.DiesAndSlowsOnSolid()
				.GrowsIn([SimHashes.Oxygen, Elements.saltyOxygen, SimHashes.ContaminatedOxygen], 100_000)
				.DisinfectedBy(SimHashes.ChlorineGas, SimHashes.BleachStone, SimHashes.Ethanol)
				.DiesIn(Elements.murkyBrine, SimHashes.Brine, Elements.sulfurousWater, Elements.sourBrine);

			InitializeElemExposureArray(ref elemExposureInfo, DEFAULT_EXPOSURE_INFO);
		}
	}
}
