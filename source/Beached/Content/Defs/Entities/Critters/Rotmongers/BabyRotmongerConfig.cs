using Beached.Content.DefBuilders;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Rotmongers
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyRotmongerConfig : BaseRotmongerConfig, IEntityConfig
	{
		public const string ID = "Beached_RotmongerBaby";

		protected override string AnimFile => "beached_rotmonger_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(RotmongerConfig.ID, forceNavType: true)
				.Size(1, 1)
				.Mass(50f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
