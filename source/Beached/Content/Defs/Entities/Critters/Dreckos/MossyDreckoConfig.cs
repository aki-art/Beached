using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Dreckos
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class MossyDreckoConfig : BaseDreckoConfig, IEntityConfig
	{
		public const string ID = "Beached_MossyDrecko";
		public const string EGG_ID = "Beached_MossyDreckoEgg";
		public static float mossPerCycle = 100f;
		public static float scaleGrowthCycles = 3f;

		protected override string AnimFile => "beached_mossy_drecko_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Navigator(CritterBuilder.NAVIGATION.DRECKO)
				.Egg(BabyMossyDreckoConfig.ID, "beached_egg_mossy_drecko_kanim")
					.EggChance(EGG_ID, 0.92f)
					.EggChance(DreckoConfig.EGG_ID, 0.6f)
					.EggChance(DreckoPlasticConfig.EGG_ID, 0.2f)
					.Incubation(30f)
					.Fertility(90f)
					.SortOrder(DreckoPlasticConfig.EGG_SORT_ORDER + 1)
					.Done()
				.Drops([MeatConfig.ID, MeatConfig.ID]);
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>();

			var plantDiet = new Diet.Info([
				BasicSingleHarvestPlantConfig.ID,
				PrickleFlowerConfig.ID
			],

			DreckoPlasticConfig.POOP_ELEMENT,
			DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN,
			DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT,
			food_type: Diet.Info.FoodType.EatPlantDirectly);

			var diet = new Diet([plantDiet]);

			var calorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			calorieMonitor.diet = diet;
			calorieMonitor.minConsumedCaloriesBeforePooping = DreckoPlasticConfig.MIN_POOP_SIZE_IN_CALORIES;

			var scaleGrowth = prefab.AddOrGetDef<ScaleGrowthMonitor.Def>();
			scaleGrowth.defaultGrowthRate = 1.0f / scaleGrowthCycles / CONSTS.CYCLE_LENGTH;
			scaleGrowth.dropMass = mossPerCycle * scaleGrowthCycles;
			scaleGrowth.itemDroppedOnShear = Elements.moss.CreateTag();
			scaleGrowth.levelCount = 6;
			scaleGrowth.targetAtmosphere = SimHashes.Helium;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			// TODO: temporary for the placeholder kanim

			if (!prefab.TryGetComponent(out SymbolOverrideController controller))
				controller = SymbolOverrideControllerUtil.AddToPrefab(prefab);

			controller.ApplySymbolOverridesByAffix(Assets.GetAnim(AnimFile), "fbr_");

			prefab.AddOrGet<CreatureBrain>().symbolPrefix = "fbr_";

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
