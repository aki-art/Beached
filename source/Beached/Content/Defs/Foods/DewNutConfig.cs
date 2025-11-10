using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class DewNutConfig : IEntityConfig
	{
		public const string ID = "Beached_DewNut";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				0f,
				FOOD.FOOD_QUALITY_TERRIBLE,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				false);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_dewnut_kanim",
				0.55f,
				0.55f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
