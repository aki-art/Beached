using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class AstrobarConfig : IEntityConfig
	{
		public const string ID = "Beached_Astrobar";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				3000_000f,
				FOOD.FOOD_QUALITY_AMAZING,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				false);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_astrobar_kanim",
				0.8f,
				0.5f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
