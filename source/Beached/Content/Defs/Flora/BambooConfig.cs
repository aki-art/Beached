using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.SegmentedEntities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class BambooConfig : IEntityConfig
	{
		public const string ID = "Beached_Bamboo";
		public const string SEED_ID = "Beached_BambooSeed";
		public const string PREVIEW_ID = "Beached_Bamboo_preview";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_BAMBOO.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_BAMBOO.DESC,
				100f,
				Assets.GetAnim("beached_bamboo_kanim"),
				"idle",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER1);

			var seedPrefab = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.DigOnly,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_BAMBOO.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_BAMBOO.DESC,
				Assets.GetAnim("beached_seed_kelp_kanim"),
				additionalTags: [GameTags.CropSeed]);

			EntityTemplates.CreateAndRegisterPreviewForPlant(seedPrefab, PREVIEW_ID, Assets.GetAnim("beached_bamboo_kanim"), "place", 1, 1);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers =
			[
				ObjectLayer.Building
			];

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
			//prefab.AddOrGet<Updatable>();

			//prefab.AddOrGet<StackablePlant>().validFoundationTag = ID;

			/*			var segmented = prefab.AddOrGet<SegmentedBamboo>();
						segmented.maxLength = 32;
						segmented.segmentAnimFile = "beached_bamboo_kanim";*/

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.randomiseLoopedOffset = true;

			/*			var segment = prefab.AddOrGet<LongPlantSegment>();
						segment.isRoot = true;
						segment.animFileRoot = "beached_bamboo";

						var longPlant = prefab.AddOrGet<LongPlant>();
						longPlant.growPrefab = BambooSegmentConfig.ID;
						longPlant.connectTag = BTags.bamboo;
						longPlant.maxLength = 128;
						longPlant.growthDirection = Direction.Up;*/

			var root = prefab.AddOrGet<BambooRoot>();
			root.segmentPrefab = BambooSegmentConfig.ID;
			root.maxLength = 128;
			root.growthTimer = CONSTS.CYCLE_LENGTH;

			prefab.AddOrGet<EntitySegment>();

			//kbac.animWidth = 0.75f;

			var ladder = prefab.AddOrGet<Ladder>();
			ladder.isPole = true;
			ladder.downwardsMovementSpeedMultiplier = 1.5f;
			ladder.upwardsMovementSpeedMultiplier = 0.5f;

			prefab.AddTag(BTags.bamboo);

			prefab.AddComponent<DropStuffOnDeath>().drop = Elements.bambooStem.CreateTag();

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
