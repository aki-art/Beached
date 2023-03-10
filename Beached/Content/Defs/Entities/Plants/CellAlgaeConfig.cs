using Beached.Content.Defs.Items.Foods;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Plants
{
    public class CellAlgaeConfig : IEntityConfig
    {
        public const string ID = "Beached_AlgaeCell";
        public const float WATER_RATE = 5f / CONSTS.CYCLE_LENGTH;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                STRINGS.CREATURES.SPECIES.CELLALGAE.NAME,
                STRINGS.CREATURES.SPECIES.CELLALGAE.DESCRIPTION,
                10f,
                Assets.GetAnim("beached_singlecell_kanim"),
                "idle_loop",
                Grid.SceneLayer.BuildingBack,
                1,
                1,
                DECOR.BONUS.TIER0);

            EntityTemplates.ExtendEntityToBasicPlant(
                prefab,
                MiscUtil.CelsiusToKelvin(-10f),
                MiscUtil.CelsiusToKelvin(0f),
                MiscUtil.CelsiusToKelvin(32f),
                MiscUtil.CelsiusToKelvin(40f),
                new[]
                {
                    SimHashes.Water,
                    SimHashes.SaltWater,
                    SimHashes.Brine,
                    Elements.murkyBrine
                },
                false,
                0f,
                0.15f,
                JellyConfig.ID,
                false,
                true,
                false,
                true,
                2400f,
                0f,
                7400f,
                ID + "Original",
                STRINGS.CREATURES.SPECIES.CELLALGAE.NAME);


            var drowningMonitor = prefab.AddOrGet<DrowningMonitor>();
            drowningMonitor.canDrownToDeath = false;
            drowningMonitor.livesUnderWater = true;

            prefab.AddOrGet<StandardCropPlant>();

            EntityTemplates.ExtendPlantToIrrigated(prefab, new[]
            {
                new PlantElementAbsorber.ConsumeInfo(GameTags.Water,  WATER_RATE)
            });

            prefab.AddOrGet<LoopingSounds>();

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
