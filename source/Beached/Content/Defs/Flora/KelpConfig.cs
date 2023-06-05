using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	internal class KelpConfig : IEntityConfig
	{
		public const string ID = "Beached_Kelp";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.KELP.NAME,
				STRINGS.CREATURES.SPECIES.KELP.DESC,
				100f,
				Assets.GetAnim("beached_kelp_kanim"),
				"idle_0",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER1);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers = new[]
			{
				ObjectLayer.Building
			};

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
			prefab.AddOrGet<SeedProducer>().Configure("BasicForagePlant", SeedProducer.ProductionType.DigOnly, 1);

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

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
