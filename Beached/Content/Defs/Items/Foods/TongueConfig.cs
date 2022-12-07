using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Items.Foods
{
    internal class TongueConfig : IEntityConfig
    {
        public static string ID = "Beached_MusselTongue";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.TONGUE.NAME,
                STRINGS.ITEMS.FOOD.TONGUE.DESC,
                1f,
                false,
                Assets.GetAnim("meallicegrain_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.5f,
                true);

            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                2800f * 1000f,
                FOOD.FOOD_QUALITY_AWFUL,
                FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                FOOD.DEFAULT_ROT_TEMPERATURE,
                FOOD.SPOIL_TIME.DEFAULT,
                false);

            var gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return gameObject;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst)
        {
            inst.GetComponent<KBatchedAnimController>().TintColour = new Color(1f, 0.6f, 0f);
        }
    }
}
