using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class RawKelpConfig : IEntityConfig
	{
		public const string ID = "Beached_RawKelp";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				700_000f,
				FOOD.FOOD_QUALITY_AWFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				false);

			foodInfo.AddEffects(["SeafoodRadiationResistance"], [DlcManager.EXPANSION1_ID]);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_raw_kelp_kanim",
				1f,
				0.4f,
				foodInfo);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
