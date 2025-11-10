using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class FoulPoffConfig : IEntityConfig
	{
		public const string ID = "Beached_FoulPoff";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				200_000,
				FOOD.FOOD_QUALITY_AWFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				CONSTS.CYCLE_LENGTH,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_foulpoff_kanim",
				0.6f,
				0.35f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
