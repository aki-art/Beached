using TUNING;
using UnityEngine;
using static EdiblesManager;


namespace Beached.Content.Defs.Foods
{
	public class VeggieBurgerConfig : IEntityConfig
	{
		public const string ID = "Beached_VeggieBurger";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				6_000_000f,
				FOOD.FOOD_QUALITY_MORE_WONDERFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.QUICK,
				true)
				.AddEffects(["GoodEats"])
				.AddEffects(["SeafoodRadiationResistance"], DlcManager.EXPANSION1);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"frost_burger_kanim",
				0.8f,
				0.4f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
