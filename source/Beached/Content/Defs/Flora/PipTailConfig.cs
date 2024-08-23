using Beached.Content.Defs.Foods;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class PipTailConfig : IEntityConfig
	{
		public const string ID = "Beached_PipTail";
		public const string SEED_ID = "Beached_PipTailSeed";
		public const string BASE_TRAIT_ID = "Beached_PipTailOriginal";
		public const string PREVIEW_ID = "Beached_PipTail_preview";
		public const float FERTILIZATION_RATE = 5f / CONSTS.CYCLE_LENGTH;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_PIPTAIL.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_PIPTAIL.DESCRIPTION,
				2f,
				Assets.GetAnim("beached_piptail_kanim"),
				"idle_full",
				Grid.SceneLayer.BuildingFront,
				1,
				2,
				TUNING.DECOR.BONUS.TIER1);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				198.15f,
				248.15f,
				273.15f,
				323.15f,
				[
					SimHashes.Oxygen,
					SimHashes.ContaminatedOxygen,
					Elements.saltyOxygen,
					SimHashes.CarbonDioxide,
					SimHashes.DirtyWater,
					SimHashes.Water
				],
				pressure_warning_low: 0.025f,
				crop_id: PipShootConfig.ID,
				max_radiation: 9800f,
				baseTraitId: BASE_TRAIT_ID,
				baseTraitName: STRINGS.CREATURES.SPECIES.BEACHED_PIPTAIL.NAME);

			prefab.AddOrGet<StandardCropPlant>();
			prefab.AddOrGet<LoopingSounds>();

			EntityTemplates.ExtendPlantToFertilizable(prefab,
			[
				new PlantElementAbsorber.ConsumeInfo()
				{
					tag = Elements.zeolite.CreateTag(),
					massConsumptionRate = FERTILIZATION_RATE
				}
			]);

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_PIPTAIL.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_PIPTAIL.DESC,
				Assets.GetAnim("beached_pipcorn_kanim"),
				additionalTags: [GameTags.CropSeed],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_PIPTAIL.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_piptail_kanim"),
				"place",
				1,
				2);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
