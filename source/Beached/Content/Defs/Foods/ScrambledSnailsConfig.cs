using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class ScrambledSnailsConfig : IEntityConfig
	{
		public const string ID = "Beached_ScrambledSnails";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				3200_000f,
				FOOD.FOOD_QUALITY_AMAZING,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.QUICK,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_scrambledsnails_kanim",
				0.6f,
				0.6f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
