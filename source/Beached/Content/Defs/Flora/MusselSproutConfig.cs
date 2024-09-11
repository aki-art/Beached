using Beached.Content.Defs.Foods;
using Beached.Content.Scripts;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
    public class MusselSproutConfig : IEntityConfig
	{
		public const string ID = "Beached_MusselSprout";
		public const string BASE_TRAIT_ID = ID + "_BaseTrait";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_MUSSEL_SPROUT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_MUSSEL_SPROUT.DESC,
				50f,
				Assets.GetAnim("beached_mussel_sprout_kanim"),
				"idle",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.PENALTY.TIER0);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];
			prefab.AddOrGet<EntombVulnerable>();
			prefab.AddOrGet<Prioritizable>();
			var uprootable = prefab.AddOrGet<UprootableWithDramaticDeath>();
			uprootable.deathAnimation = "harvest";
			//uprootable.deathSoundFx = ModAssets.Sounds.MUSSEL_SPROUT_HARVEST;
			prefab.AddOrGet<UprootedMonitor>();
			prefab.AddOrGet<Harvestable>();
			prefab.AddOrGet<HarvestDesignatable>();
			prefab.AddOrGet<SeedProducer>().Configure(MusselTongueConfig.ID, SeedProducer.ProductionType.DigOnly, 1);

			var singleHarvestable = prefab.AddOrGet<SingleHarvestable>();
			singleHarvestable.soundFx = ModAssets.Sounds.MUSSEL_SPROUT_HARVEST;
			singleHarvestable.volume = 0.5f;

			prefab.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) => inst.GetComponent<KBatchedAnimController>().animScale *= 0.75f;
	}
}