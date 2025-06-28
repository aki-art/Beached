using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class SpongeCakeConfig : IEntityConfig
	{
		public const string ID = "Beached_SpongeCake";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				1900_000f,
				FOOD.FOOD_QUALITY_GOOD,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_spongecake_kanim",
				0.9f,
				0.55f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
