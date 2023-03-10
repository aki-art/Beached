using Beached.Content.Defs.Ores;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Items.Foods
{
    public class JellyConfig : IEntityConfig
    {
        public const string ID = "Beached_Jelly";

        public GameObject CreatePrefab()
        {
            CROPS.CROP_TYPES.Add(new(JellyConfig.ID, 3f * CONSTS.CYCLE_LENGTH));

            var foodInfo = new FoodInfo(
                ID,
                DlcManager.VANILLA_ID,
                800_000f,
                FOOD.FOOD_QUALITY_TERRIBLE,
                FOOD.DEFAULT_PRESERVE_TEMPERATURE,
                FOOD.DEFAULT_ROT_TEMPERATURE,
                FOOD.SPOIL_TIME.DEFAULT,
                true);

            var prefab = BEntityTemplates.CreateFood(
                ID,
                "beached_jelly_kanim",
                0.6f,
                0.35f,
                foodInfo);

            prefab.AddBTag(BTags.meat);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst)
        {
            if (inst.HasTag("test"))
            { }
        }
    }
}
