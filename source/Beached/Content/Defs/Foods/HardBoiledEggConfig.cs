using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class HardBoiledEggConfig : IEntityConfig
	{
		public const string ID = "Beached_HardBoiledEgg";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				800_000f,
				FOOD.FOOD_QUALITY_TERRIBLE,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.QUICK,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_hardboiledegg_kanim",
				0.5f,
				0.4f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
