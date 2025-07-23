using Beached.Content.Defs.Items;
using Beached.Content.Scripts.Entities.Plant;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class DewPalmConfig : IEntityConfig
	{
		public const string ID = "Beached_DewPalm";
		public const string SEED_ID = "Beached_DewPalmSeed";
		public const string PREVIEW_ID = "Beached_DewPalmPreview";
		public const string BASE_TRAIT_ID = "Beached_DewPalmOriginal";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.DESC,
				100f,
				Assets.GetAnim("beached_dewpalm_kanim"),
				"idle",
				Grid.SceneLayer.BuildingBack,
				1,
				3,
				DECOR.BONUS.TIER2,
				defaultTemperature: CREATURES.TEMPERATURE.HOT_1);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				CREATURES.TEMPERATURE.FREEZING_10,
				CREATURES.TEMPERATURE.FREEZING_9,
				CREATURES.TEMPERATURE.HOT_2,
				CREATURES.TEMPERATURE.HOT_3,
				null,
				true,
				0f,
				0.15f,
				PalmLeafConfig.ID,
				true,
				true,
				true,
				true,
				2400f,
				0f,
				2200f,
				BASE_TRAIT_ID,
				STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.NAME);

			var ediblePlant = prefab.AddOrGet<DirectlyEdiblePlant_TreeBranches>();
			ediblePlant.overrideCropID = PalmLeafConfig.ID;
			ediblePlant.MinimumEdibleMaturity = 1f;

			prefab.UpdateComponentRequirement<Harvestable>(false);

			var leafs = ModTuning.DewPalm.Leafs().Values;

			var branchGrower = prefab.AddOrGetDef<PlantBranchGrower.Def>();
			branchGrower.BRANCH_OFFSETS = [.. leafs];
			branchGrower.BRANCH_PREFAB_NAME = DewPalmLeafConfig.ID;
			branchGrower.harvestOnDrown = true;
			branchGrower.propagateHarvestDesignation = false;
			branchGrower.MAX_BRANCH_COUNT = leafs.Count;

			prefab.AddOrGet<BuddingTrunk>();

			var dirt = new PlantElementAbsorber.ConsumeInfo()
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 7f / 600f
			};

			EntityTemplates.ExtendPlantToFertilizable(prefab, [dirt]);

			prefab.AddOrGet<StandardCropPlant>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_DEWNUT.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_DEWNUT.DESC,
				Assets.GetAnim("beached_dewnut_kanim"),
				numberOfSeeds: 3,
				additionalTags: [GameTags.CropSeed],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_small_cell_kanim"),
				"place",
				1,
				1);

			//Object.DestroyImmediate(prefab.GetComponent<MutantPlant>());

			var latexStorage = prefab.AddComponent<Storage>();
			latexStorage.storageFilters = [Elements.rubber.CreateTag()];
			latexStorage.capacityKg = 20;
			latexStorage.allowItemRemoval = true;

			var metalStorage = prefab.AddComponent<Storage>();
			metalStorage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			metalStorage.capacityKg = 50;
			metalStorage.allowItemRemoval = false;

			var tap = prefab.AddComponent<RubberTappable>();
			tap.trackSymbol = "bucket";
			tap.materialStorage = latexStorage;
			tap.metalStorage = metalStorage;
			tap.materialPerCycle = 20;
			tap.element = Elements.rubber;

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
