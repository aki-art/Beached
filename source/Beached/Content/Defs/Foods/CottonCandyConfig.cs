using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class CottonCandyConfig : IEntityConfig
	{
		public const string ID = "Beached_CottonCandy";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				800_000f,
				FOOD.FOOD_QUALITY_GREAT,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				CONSTS.CYCLE_LENGTH * 2,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_cottoncandy_kanim",
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
