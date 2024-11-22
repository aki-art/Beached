using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class GlazedDewnutConfig : IEntityConfig
	{
		public const string ID = "Beached_GlazedDewnut";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				3200_000f,
				FOOD.FOOD_QUALITY_WONDERFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_glazeddewnut_kanim",
				0.6f,
				0.4f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
