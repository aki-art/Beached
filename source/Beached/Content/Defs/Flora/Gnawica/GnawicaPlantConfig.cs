using Beached.Content.Scripts.Entities.Plant;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Flora.Gnawica
{
	public class GnawicaPlantConfig : IEntityConfig
	{
		public const string ID = "Beached_GnawicaPlant";
		public const string SEED_ID = "Beached_GnawicaPlantSeed";
		public const string BASE_TRAIT_ID = "Beached_GnawicaPlantOriginal";
		public const string PREVIEW_ID = "Beached_GnawicaPlant_preview";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_GNAWICAPLANT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_GNAWICAPLANT.DESC,
				30f,
				Assets.GetAnim("beached_gnawica_stem_kanim"),
				"idle_loop",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				TUNING.DECOR.NONE);

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				null,
				SeedProducer.ProductionType.Fruit,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_GNAWICASEED.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_GNAWICASEED.DESC,
				Assets.GetAnim("beached_boneworm_seed_kanim"),//beached_gnawica_seed_kanim"),
				additionalTags: [BTags.flatFloorSeed],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_GNAWICAPLANT.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_gnawica_stem_kanim"),
				"place",
				1,
				3);

			var core = prefab.AddComponent<GnawicaCore>();
			core.destroyPartsOnCleanup = false;
			core.stalkPrefabId = GnawicaStemConfig.ID;
			core.nestPrefabId = FuaFuaNestConfig.ID;
			core.mawPrefabId = GnawicaMawConfig.ID;
			core.maxStalkCount = 32;
			core.windyness = 0.1f;
			core.maxWalkers = 3;
			core.minWalkerSteps = 3;
			core.maxWalkerSteps = 6;
			core.allowAdjacentSpawnChance = 0.1f;

			var gnawica = prefab.AddComponent<GnawicaGrowing>();
			gnawica.minLengthForMaw = 12;

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
