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
				.DiesIn(Element.State.Liquid)
				.Rule(new StateGrowthRule(Element.State.Gas)
				{
					minCountPerKG = 200,
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.NONE),
					overPopulationHalfLife = new float?(float.PositiveInfinity)
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
					SimHashes.BrineIce)
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
				});

			InitializeElemExposureArray(ref elemExposureInfo, DEFAULT_EXPOSURE_INFO);
		}
	}
}
