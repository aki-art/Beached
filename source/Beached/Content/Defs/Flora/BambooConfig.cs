using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	internal class BambooConfig : IEntityConfig
	{
		public const string ID = "Beached_Bamboo";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BAMBOO.NAME,
				STRINGS.CREATURES.SPECIES.BAMBOO.DESC,
				100f,
				Assets.GetAnim("beached_bamboo_kanim"),
				"idle",
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
			prefab.AddOrGet<DrowningMonitor>();
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

			/*            var segmented = prefab.AddOrGet<SegmentedBamboo>();
						segmented.maxLength = 32;
						segmented.segmentAnimFile = "beached_bamboo_kanim";

						var kbac = prefab.AddOrGet<KBatchedAnimController>();
						kbac.randomiseLoopedOffset = true;*/
			//kbac.animWidth = 0.75f;
			/*
						var ladder = prefab.AddOrGet<Ladder>();
						ladder.isPole = true;
						ladder.downwardsMovementSpeedMultiplier = 1.5f;
						ladder.upwardsMovementSpeedMultiplier = 0.5f;*/

			prefab.AddTag(BTags.bamboo);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
