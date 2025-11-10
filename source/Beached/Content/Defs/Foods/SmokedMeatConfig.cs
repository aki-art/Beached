using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class SmokedMeatConfig : IEntityConfig
	{
		public const string ID = "Beached_SmokedMeat";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				1_800_000f,
				FOOD.FOOD_QUALITY_GOOD,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_smoked_meat_kanim",
				0.8f,
				0.6f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
