using System;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	internal class GnawicaBerryConfig : IEntityConfig
	{
		public const string ID = "Beached_GnawicaBerry";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				1200_000f,
				FOOD.FOOD_QUALITY_GOOD,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.SPOIL_TIME.QUICK,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_gnawberry_kanim",
				0.8f,
				0.5f,
				foodInfo);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
