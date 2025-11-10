using Beached.Content.DefBuilders;
using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
	public class WashuSpongeConfig : IEntityConfig
	{
		public const string ID = "Beached_Coral_WashuSponge";
		public const string SEED_ID = "Beached_Coral_WashuSpongeSeed";

		public GameObject CreatePrefab()
		{
			var (prefab, seedPrefab) = new CoralBuilder(ID, "beached_washu_sponge_kanim")
				.InitialAnim("idle_full")
				.Frag("beached_washu_sponge_frag_kanim")
				.Build();

			var foodInfo = new EdiblesManager.FoodInfo(
				SEED_ID,
				0,
				FOOD.FOOD_QUALITY_TERRIBLE,
				FOOD.HIGH_PRESERVE_TEMPERATURE,
				FOOD.HIGH_ROT_TEMPERATURE,
				FOOD.SPOIL_TIME.VERYSLOW,
				false);

			BEntityTemplates.CreateFood(
				ID,
				"beached_spongecake_kanim",
				0.7f,
				0.4f,
				foodInfo);

			EntityTemplates.ExtendEntityToFood(seedPrefab, foodInfo);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
