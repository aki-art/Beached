using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class SmokedLiceConfig : IEntityConfig
	{
		public const string ID = "Beached_SmokedLice";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				1200_000f,
				FOOD.FOOD_QUALITY_GREAT,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_smoked_lice_kanim",
				0.7f,
				0.5f,
				foodInfo);

			if (!Mod.settings.General.IsMealLiceMeat)
				prefab.AddTag(BTags.meat);
			else
				prefab.AddTag(BTags.nonVegetarian);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
