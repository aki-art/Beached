using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Foods;
using TUNING;
using UnityEngine;
using static PlantElementAbsorber;

namespace Beached.Content.Defs.Flora
{
	public class AlgaeCellConfig : IEntityConfig
	{
		public const string ID = "Beached_AlgaeCell";
		public const string SEED_ID = "Beached_AlgaeCellSeed";
		public const string PREVIEW_ID = "Beached_AlgaeCellPreview";
		public const float WATER_RATE = 5f / CONSTS.CYCLE_LENGTH;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_CELLALGAE.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_CELLALGAE.DESCRIPTION,
				10f,
				Assets.GetAnim("beached_singlecell_kanim"),
				"idle_full",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER0,
				additionalTags:
				[
					BTags.aquaticPlant,
					BTags.selfIrrigating
				]);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				MiscUtil.CelsiusToKelvin(-10f),
				MiscUtil.CelsiusToKelvin(0f),
				MiscUtil.CelsiusToKelvin(32f),
				MiscUtil.CelsiusToKelvin(40f),
				CoralTemplate.ALL_WATERS,
				false,
				0f,
				0.15f,
				JellyConfig.ID,
				false,
				true,
				true,
				true,
				2400f,
				0f,
				7400f,
				ID + "Original",
				STRINGS.CREATURES.SPECIES.BEACHED_CELLALGAE.NAME);

			prefab.AddOrGet<SubmersionMonitor>();
			prefab.AddOrGet<StandardCropPlant>();
			prefab.AddOrGet<LoopingSounds>();

			//EntityTemplates.ExtendPlantToIrrigated(prefab, new ConsumeInfo(GameTags.Water, WATER_RATE));

			BEntityTemplates.ExtendPlantToSelfIrrigated(prefab, new ConsumeInfo(GameTags.Water, WATER_RATE));

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_CELLALGAE.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_CELLALGAE.DESC,
				Assets.GetAnim("beached_small_cell_kanim"),
				additionalTags: [BTags.aquaticSeed],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_CELLALGAE.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_small_cell_kanim"),
				"place",
				1,
				1);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
