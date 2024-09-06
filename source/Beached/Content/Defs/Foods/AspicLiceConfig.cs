using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class AspicLiceConfig : IEntityConfig
	{
		public const string ID = "Beached_AspicLice";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				1200_000f,
				FOOD.FOOD_QUALITY_MEDIOCRE,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_aspiclice_kanim",
				0.6f,
				0.5f,
				foodInfo);

			if (!Mod.settings.General.IsMealLiceMeat)
				prefab.AddTag(BTags.meat);
			else
				prefab.AddTag(BTags.nonVegetarian);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
