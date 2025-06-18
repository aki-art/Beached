using Beached.Content.DefBuilders;
using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Rotmongers
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class RotmongerConfig : BaseRotmongerConfig, IEntityConfig
	{
		public const string ID = "Beached_Rotmonger";
		public const string EGG_ID = "Beached_RotmongerEgg";

		protected override string AnimFile => "beached_rotmonger_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(this);

			SetupBasicSolidDiet(
				prefab,
				RotDiet(),
				RotmongerTuning.BASE.CALORIES_PER_KG_OF_ORE,
				RotmongerTuning.BASE.MIN_POOP_SIZE_IN_KG);

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 1)
				.Tag(GameTags.OriginalCreature)
				.Egg(BabyRotmongerConfig.ID, "beached_egg_slagmite_kanim")
					.Fertility(10f)
					.Incubation(1)
					.Mass(1f)
					.EggChance(EGG_ID, 1f)
					.Done();
		}

		public static Diet.Info[] RotDiet() => [
				new(
					[Elements.rot.CreateTag()],
					SimHashes.Katairite.CreateTag(),
					RotmongerTuning.BASE.CALORIES_PER_KG_OF_ORE,
					CREATURES.CONVERSION_EFFICIENCY.NORMAL)
			];

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
