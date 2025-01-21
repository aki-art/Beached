using Beached.Content.Defs.Items;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	internal class SidewaysPlantConfig : IEntityConfig
	{
		public const string ID = "Beached_SidewaysPlant";
		public const string SEED_ID = "Beached_SidewaysPlantSeed";
		public const string PREVIEW_ID = "Beached_SidewaysPlantPreview";
		public const string BASE_TRAIT_ID = "Beached_SidewaysPlantOriginal";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_SIDEWAYSPLANT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_SIDEWAYSPLANT.DESC,
				100f,
				Assets.GetAnim("beached_sidewaysplant_kanim"),
				"idle_full",
				Grid.SceneLayer.BuildingBack,
				3,
				1,
				DECOR.BONUS.TIER2,
				defaultTemperature: CREATURES.TEMPERATURE.HOT_1);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				CREATURES.TEMPERATURE.FREEZING_10,
				CREATURES.TEMPERATURE.FREEZING_9,
				CREATURES.TEMPERATURE.HOT_2,
				CREATURES.TEMPERATURE.HOT_3,
				[Elements.ammonia],
				true,
				0f,
				0.15f,
				PalmLeafConfig.ID, // TODO
				true,
				true,
				true,
				true,
				2400f,
				0f,
				2200f,
				BASE_TRAIT_ID,
				STRINGS.CREATURES.SPECIES.BEACHED_SIDEWAYSPLANT.NAME);

			var fertilizer = new PlantElementAbsorber.ConsumeInfo()
			{
				tag = SimHashes.Katairite.CreateTag(),
				massConsumptionRate = 1f / 600f
			};

			prefab.AddOrGet<StandardCropPlant>();
			prefab.AddOrGet<LoopingSounds>();

			EntityTemplates.ExtendPlantToFertilizable(prefab, [fertilizer]);


			prefab.AddOrGet<DirectlyEdiblePlant_Growth>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_SIDEWAYSPLANT.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_SIDEWAYSPLANT.DESC,
				Assets.GetAnim("beached_dewnut_kanim"),
				numberOfSeeds: 1,
				planterDirection: SingleEntityReceptacle.ReceptacleDirection.Side,
				additionalTags: [GameTags.CropSeed],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_SIDEWAYSPLANT.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_small_cell_kanim"),
				"place",
				1,
				1);

			prefab.GetComponent<UprootedMonitor>().monitorCells = [CellOffset.right];
			// EntityTemplates.MakeHangingOffsets

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
