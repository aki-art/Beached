using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	internal class RawSnailConfig : IEntityConfig
	{
		public static string ID = "Beached_RawSnail";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				800_000f,
				FOOD.FOOD_QUALITY_AWFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_rawsnail_kanim",
				0.8f,
				0.25f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
