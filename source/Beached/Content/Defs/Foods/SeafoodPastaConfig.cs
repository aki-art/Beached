using Beached.Content.ModDb;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class SeafoodPastaConfig : IEntityConfig
	{
		public const string ID = "Beached_SeafoodPasta";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				3200_000f,
				FOOD.FOOD_QUALITY_AMAZING,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			foodInfo.AddEffects([BEffects.VANILLA.SEAFOOD_RADIATION_RESISTANCE], DlcManager.AVAILABLE_EXPANSION1_ONLY);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_seafood_pasta_kanim",
				0.8f,
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
