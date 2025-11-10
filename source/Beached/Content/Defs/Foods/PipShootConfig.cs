using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class PipShootConfig : IEntityConfig
	{
		public static string ID = "Beached_PipShoot";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				800_000f,
				FOOD.FOOD_QUALITY_AWFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_pipshoot_kanim",
				0.8f,
				0.6f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
