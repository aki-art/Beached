using Beached.Content.ModDb;
using System;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class MeatPlatterConfig : IEntityConfig
	{
		public const string ID = "Beached_MeatPlatter";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				6_000_000f,
				FOOD.FOOD_QUALITY_MORE_WONDERFUL,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				true);

			foodInfo.AddEffects([BEffects.MEAT_PLATTER]);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_meatplatter_kanim",
				0.8f,
				0.4f,
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
