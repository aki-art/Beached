using Beached.Content.Defs.Items.Foods;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Plants
{
    public class MusselSproutConfig : IEntityConfig
    {
        public const string ID = "Beached_MusselSprout";
        public const string BASE_TRAIT_ID = ID + "_BaseTrait";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                STRINGS.CREATURES.SPECIES.MUSSEL_SPROUT.NAME,
                STRINGS.CREATURES.SPECIES.MUSSEL_SPROUT.DESC,
                50f,
                Assets.GetAnim("beached_mussel_sprout_kanim"),
                "idle",
                Grid.SceneLayer.BuildingBack,
                1,
                1,
                DECOR.PENALTY.TIER0);

            prefab.AddOrGet<SimTemperatureTransfer>();
            prefab.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
            {
                ObjectLayer.Building
            };
            prefab.AddOrGet<EntombVulnerable>();
            prefab.AddOrGet<Prioritizable>();
            prefab.AddOrGet<Uprootable>();
            prefab.AddOrGet<UprootedMonitor>();
            prefab.AddOrGet<Harvestable>();
            prefab.AddOrGet<HarvestDesignatable>();
            prefab.AddOrGet<SeedProducer>().Configure(TongueConfig.ID, SeedProducer.ProductionType.DigOnly, 1);
            prefab.AddOrGet<BasicForagePlantPlanted>();
            prefab.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;

            return prefab;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            inst.GetComponent<KBatchedAnimController>().animScale *= 0.66f;
        }
    }
}