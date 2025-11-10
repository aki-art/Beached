using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class SpicyCracklingsConfig : IEntityConfig
	{
		public const string ID = "Beached_SpicyCracklings";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				1800_000f,
				FOOD.FOOD_QUALITY_GREAT,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_spicycracklings_kanim",
				0.6f,
				0.5f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
