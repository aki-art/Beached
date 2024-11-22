using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class CrabCakesConfig : IEntityConfig
	{
		public const string ID = "Beached_CrabCakes";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				1600_000f,
				FOOD.FOOD_QUALITY_MEDIOCRE,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_crabcakes_kanim",
				0.6f,
				0.5f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
