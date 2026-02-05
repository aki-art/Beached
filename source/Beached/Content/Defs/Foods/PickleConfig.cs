using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class PickleConfig : IEntityConfig
	{
		public const string ID = "Beached_Pickle";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				800_000f,
				FOOD.FOOD_QUALITY_GOOD,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.SLOW,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_raw_kelp_kanim",
				1f,
				0.4f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
