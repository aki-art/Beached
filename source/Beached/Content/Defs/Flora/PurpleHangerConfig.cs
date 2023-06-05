using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	internal class PurpleHangerConfig : IEntityConfig
	{
		public const string ID = "Beached_PurpleHanger";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.PURPLEHANGER.NAME,
				STRINGS.CREATURES.SPECIES.PURPLEHANGER.DESC,
				100f,
				Assets.GetAnim("beached_purplehanger_kanim"),
				"idle_bottom",
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

			prefab.AddOrGet<Updatable>();

			//prefab.AddOrGet<StackablePlant>().validFoundationTag = ID;

			prefab.AddOrGet<PurpleHanger>().maxInitialLength = 12;
			//prefab.AddOrGet<BambooStalkPiece>().leafVariationCount = 11;

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.randomiseLoopedOffset = true;
			//kbac.animWidth = 0.75f;

			prefab.AddTag(BTags.bamboo);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
