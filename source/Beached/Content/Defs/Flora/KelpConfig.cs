using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class KelpConfig : IEntityConfig
	{
		public const string ID = "Beached_Kelp";
		public const string SEED_ID = "Beached_KelpSeed";
		public const string PREVIEW_ID = "Beached_Kelp_preview";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_KELP.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_KELP.DESC,
				100f,
				Assets.GetAnim("beached_kelp_kanim"),
				"idle_0",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER1);

			var seedPrefab = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_KELP.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_KELP.DESC,
				Assets.GetAnim("beached_seed_kelp_kanim"),
				additionalTags: [GameTags.CropSeed]);

			EntityTemplates.CreateAndRegisterPreviewForPlant(seedPrefab, PREVIEW_ID, Assets.GetAnim("beached_kelp_kanim"), "place", 1, 1);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers =
			[
				ObjectLayer.Building
			];

			prefab.AddOrGet<EntombVulnerable>();
			prefab.AddOrGet<SubmersionMonitor>();
			prefab.AddOrGet<Prioritizable>();
			prefab.AddOrGet<Uprootable>();

			/*            var toppleMonitor = prefab.AddOrGet<ToppleMonitor>();
						toppleMonitor.validFoundationTag = BTags.StackablePlant;
						toppleMonitor.objectLayer = ObjectLayer.Building;
			*/
			prefab.AddOrGet<Harvestable>();
			prefab.AddOrGet<HarvestDesignatable>();

			//prefab.AddOrGet<Updatable>();

			//prefab.AddOrGet<StackablePlant>().validFoundationTag = ID;

			//prefab.AddOrGet<Bamboo>().maxInitialLength = 32;
			/*            var stemPiece = prefab.AddOrGet<StemPiece>();
                        stemPiece.leafVariationCount = 4;
                        stemPiece.prefix = "idle";*/

			var segmented = prefab.AddOrGet<SegmentedKelp>();
			segmented.segmentAnimFile = "beached_kelp_kanim";
			segmented.maxLength = 16;
			segmented.segmentDeathFx = SpawnFXHashes.BuildingSpark;

			prefab.AddOrGet<KelpSubmersionMonitor>();

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.randomiseLoopedOffset = true;

			var germCultivator = prefab.AddOrGet<GermCultivator>();
			germCultivator.rate = 0.2f;
			germCultivator.germ = PlanktonGerms.ID;
			germCultivator.maxDiseaseCount = 100_000;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
