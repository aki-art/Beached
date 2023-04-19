using UnityEngine;
using TUNING;

namespace Beached.Content.Defs.Flora
{
    internal class DewPalmConfig : IEntityConfig
    {
        public const string ID = "Beached_DewPalm";
        public const string SEED_ID = "Beached_DewPalmSeed";
        public const string BASE_TRAIT_ID = "Beached_DewPalmTrait";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                STRINGS.CREATURES.SPECIES.DEWPALM.NAME,
                STRINGS.CREATURES.SPECIES.DEWPALM.DESC,
                100f,
                Assets.GetAnim("beached_dewpalm_kanim"),
                "idle",
                Grid.SceneLayer.BuildingBack,
                2,
                4,
                DECOR.BONUS.TIER2,
                defaultTemperature: CREATURES.TEMPERATURE.HOT_1);

            EntityTemplates.ExtendEntityToBasicPlant(
                prefab,
                CREATURES.TEMPERATURE.FREEZING_10,
                CREATURES.TEMPERATURE.FREEZING_9,
                CREATURES.TEMPERATURE.HOT_2,
                CREATURES.TEMPERATURE.HOT_3,
                null,
                false,
                0f,
                0.15f,
                "PlantMeat",
                true,
                true,
                true,
                false,
                2400f,
                0f,
                2200f,
                BASE_TRAIT_ID,
                STRINGS.CREATURES.SPECIES.DEWPALM.NAME);

            Object.DestroyImmediate(prefab.GetComponent<MutantPlant>());

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
