using System;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	internal class KibbleConfig : IEntityConfig
	{
		public const string ID = "Beached_Kibble";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				400_000f,
				FOOD.FOOD_QUALITY_AWFUL,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				false);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_kibble_kanim",
				0.8f,
				0.65f,
				foodInfo);

			prefab.AddTag(BTags.meat);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
