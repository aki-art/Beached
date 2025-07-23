using Beached.Content.DefBuilders;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Pufts
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class AmmoniaPuftConfig : BasePuftConfig, IEntityConfig
	{
		public const string ID = "Beached_AmmoniaPuft";
		public const string EGG_ID = "Beached_AmmoniaPuftEgg";

		protected override string AnimFile => "beached_ammonia_puft_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(this);

			global::BasePuftConfig.SetupDiet(
				prefab,
				Elements.ammonia.CreateTag(),
				Elements.rot.CreateTag(),
				PuftConfig.CALORIES_PER_KG_OF_ORE,
				TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2,
				null,
				0.0f,
				PuftConfig.MIN_POOP_SIZE_IN_KG);

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 1)
				.Egg(BabyAmmoniaPuftConfig.ID, "egg_puft_kanim")
					.Fertility(45f)
					.Incubation(15f)
					.Mass(PuftTuning.EGG_MASS)
					.EggChance(EGG_ID, 0.98f)
					.EggChance(PuftAlphaConfig.EGG_ID, 0.02f)
					.Done();
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
