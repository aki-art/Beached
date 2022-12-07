using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Items.Foods
{
    public class AstrobarConfig : IEntityConfig
    {
        public const string ID = "Beached_Astrobar";
        public static ComplexRecipe recipe;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.FOOD.ASTROBAR.NAME,
                STRINGS.ITEMS.FOOD.ASTROBAR.DESC,
                1f,
                false,
                Assets.GetAnim("beached_astrobar_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.5f,
                true);

            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                1000f * 1000f,
                FOOD.FOOD_QUALITY_AMAZING,
                FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                FOOD.DEFAULT_ROT_TEMPERATURE,
                FOOD.SPOIL_TIME.VERYSLOW,
                false);

            var gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return gameObject;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
