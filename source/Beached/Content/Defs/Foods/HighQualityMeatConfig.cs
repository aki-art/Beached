using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class HighQualityMeatConfig : IEntityConfig
	{
		public const string ID = "Beached_HighQualityMeat";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				1200_000f,
				FOOD.FOOD_QUALITY_GOOD,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_high_quality_meat_kanim",
				0.9f,
				0.55f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
