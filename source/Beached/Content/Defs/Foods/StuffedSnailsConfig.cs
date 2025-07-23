using System;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class StuffedSnailsConfig : IEntityConfig
	{
		public const string ID = "Beached_StuffedSnails";

		public GameObject CreatePrefab()
		{
			var foodInfo = new FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				2800_000f,
				FOOD.FOOD_QUALITY_AMAZING,
				FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				FOOD.DEFAULT_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.DEFAULT,
				true);

			var prefab = BEntityTemplates.CreateFood(
				ID,
				"beached_stuffed_snails_kanim",
				0.7f,
				0.5f,
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
