using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class DryNoodlesConfig : IEntityConfig
	{
		public const string ID = "Beached_DryNoodles";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				700_000f,
				FOOD.FOOD_QUALITY_TERRIBLE,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				false);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_drynoodles_kanim",
				0.7f,
				0.5f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
