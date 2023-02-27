using Beached.Content.Defs.Ores;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Items.Foods
{
    internal class TongueConfig : IEntityConfig
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
                FOOD.SPOIL_TIME.DEFAULT,
                false);

            var prefab = BEntityTemplates.CreateFood(
                ID,
                "meallicegrain_kanim",
                0.8f,
                0.6f,
                foodInfo);


            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst)
        {
            inst.GetComponent<KBatchedAnimController>().TintColour = new Color(1f, 0.6f, 0f);
        }
    }
}
