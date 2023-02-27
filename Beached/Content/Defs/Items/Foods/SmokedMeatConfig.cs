using Beached.Content.Defs.Ores;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Items.Foods
{
    public class SmokedMeatConfig : IEntityConfig
    {
        public const string ID = "Beached_SmokedMeat";

        public GameObject CreatePrefab()
        {
            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                1_400_000f,
                FOOD.FOOD_QUALITY_GOOD,
                FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                FOOD.DEFAULT_ROT_TEMPERATURE,
                FOOD.SPOIL_TIME.VERYSLOW,
                true);

            var prefab = BEntityTemplates.CreateFood(
                ID,
                "beached_smoked_meat_kanim",
                0.8f,
                0.8f,
                foodInfo);
            
            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
