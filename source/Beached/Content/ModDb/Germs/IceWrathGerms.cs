using Beached.Content.DefBuilders;
using Beached.Utils.GlobalEvents;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using TUNING;

namespace Beached.Content.ModDb.Germs
{
	/// <see cref="SimDebugViewPatch"/>
	/// <see cref="SimDebugViewPatch"/>
	public class IceWrathGerms : Disease
	{
		public const string ID = "Beached_IceWrath";
		public const float RAD_KILL_RATE = 1.0f;
		public float UVHalfLife => 1f; // UV Lamps compat

		private static readonly RangeInfo temperatureRangeInfo = MiscUtil.RangeInfoCelsius(-273.15f, 10, 20, 40);

		private static readonly RangeInfo temperatureHalfLivesInfo = new(
			DISEASE.GROWTH_FACTOR.NONE,
			DISEASE.GROWTH_FACTOR.NONE,
			DISEASE.GROWTH_FACTOR.DEATH_5,
			DISEASE.GROWTH_FACTOR.DEATH_MAX);

		private static readonly RangeInfo pressureRangeInfo = new(
			0f,
			0f,
			DISEASE.GROWTH_FACTOR.DEATH_4,
			DISEASE.GROWTH_FACTOR.DEATH_MAX);

		public static byte cachedRunTimeIndex = byte.MaxValue;

		[Subscribe(GlobalEvent.WORLD_RELOADED)]
		public static void OnWorldReloaded(bool _)
		{
			cachedRunTimeIndex = Db.Get().Diseases.GetIndex(ID);
		}

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

			var diffusion = 0.5f;
			new GrowthInfoBuilder(this)
				.DefaultBehavior(
					DISEASE.UNDERPOPULATION_DEATH_RATE.NONE,
					1400f,
					20000f,
					0.4f,
					500,
					500,
					1f / 2000f,
					1)
				// still
				.Rule(new TagGrowthRule(GameTags.IceOre)
				{
					populationHalfLife = DISEASE.GROWTH_FACTOR.NONE,
					overPopulationHalfLife = DISEASE.GROWTH_FACTOR.NONE,
					underPopulationDeathRate = 0,
					minCountPerKG = 0,
					maxCountPerKG = float.PositiveInfinity,
					diffusionScale = 0,
					minDiffusionCount = int.MaxValue
				})
				.Rule(new StateGrowthRule(Element.State.Liquid)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
					underPopulationDeathRate = 0,
					minCountPerKG = 0,
					maxCountPerKG = float.PositiveInfinity,
					diffusionScale = diffusion,
					minDiffusionCount = new int?(1000),
					minDiffusionInfestationTickCount = 0
				})

				.Rule(new StateGrowthRule(Element.State.Gas)
				{
					minCountPerKG = new float?(5f),
					underPopulationDeathRate = new float?(2.66666675f),
					populationHalfLife = new float?(10f),
					overPopulationHalfLife = new float?(10f),
					maxCountPerKG = 100_000f,
					minDiffusionCount = 100,
					diffusionScale = diffusion,
					minDiffusionInfestationTickCount = 0
				})
				.StagnatesIn(Element.State.Solid)
				.GrowsFastIn(Elements.nitrogen, SimHashes.Oxygen, SimHashes.ContaminatedOxygen, SimHashes.Hydrogen, SimHashes.Helium)
				.InstantlyDiesIn(Elements.sulfurousWater)
				.DisinfectedBy(
					SimHashes.ChlorineGas,
					SimHashes.BleachStone,
					SimHashes.Ethanol,
					Elements.saltyOxygen,
					Elements.murkyBrine,
					SimHashes.Salt,
					SimHashes.SaltWater,
					SimHashes.Brine,
					SimHashes.BrineIce);

			InitializeElemExposureArray(ref elemExposureInfo, DEFAULT_EXPOSURE_INFO);
		}
	}
}
