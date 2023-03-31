using Beached.Content.Defs.Ores;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
    internal class MusselTongueConfig : IEntityConfig
    {
        public static string ID = "Beached_MusselTongue";

        public GameObject CreatePrefab()
        {
            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                2_800_000f,
                FOOD.FOOD_QUALITY_AWFUL,
                FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                FOOD.DEFAULT_ROT_TEMPERATURE,
                FOOD.SPOIL_TIME.VERYSLOW,
                true);

            var prefab = BEntityTemplates.CreateFood(
                ID,
                "beached_musseltongue_kanim",
                0.8f,
                0.25f,
                foodInfo);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
