using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class DBAndJConfig : IEntityConfig
	{
		public const string ID = "Beached_DBAndJ";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				2400_000f,
				FOOD.FOOD_QUALITY_AMAZING,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_dbandj_kanim",
				0.8f,
				0.4f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
