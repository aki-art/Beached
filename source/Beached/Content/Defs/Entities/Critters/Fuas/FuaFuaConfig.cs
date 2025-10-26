using Beached.Content.DefBuilders;
using Beached.Content.Defs.Entities.Critters.Rotmongers;
using Beached.Content.Defs.Foods;
using Beached.Content.Navigation;
using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Fuas
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class FuaFuaConfig : BaseFuaFuaConfig, IEntityConfig
	{
		public static readonly string ID = "Beached_FuaFua";

		protected override string AnimFile => "beached_fuafua_kanim";

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Tags([GameTags.OriginalCreature]);
		}

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(this);

			SetupBasicSolidDiet(
				prefab,
				GnawicaBerryDiet(),
				RotmongerTuning.BASE.CALORIES_PER_KG_OF_ORE * 1000.0f,
				RotmongerTuning.BASE.MIN_POOP_SIZE_IN_KG);

			return prefab;
		}

		public static Diet.Info[] GnawicaBerryDiet() => [
				new(
					[GnawicaBerryConfig.ID],
					SimHashes.Phosphorite.CreateTag(),
					RotmongerTuning.BASE.CALORIES_PER_KG_OF_ORE * 1000.0f,
					CREATURES.CONVERSION_EFFICIENCY.NORMAL)

			//TODO. tuning
			//TODO: garden
			];

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst)
		{
			var navigator = inst.GetComponent<Navigator>();
			navigator.transitionDriver.overrideLayers.Add(new FuaFuaTransitionLayer(navigator));
		}
	}
}
