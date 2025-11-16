using Beached.Content.DefBuilders;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Squirrels
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyMerpipConfig : BaseMerpipConfig, IEntityConfig
	{
		public const string ID = "Beached_MerpipBaby";

		protected override string AnimFile => "baby_squirrel_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(MerpipConfig.ID, forceNavType: true)
				.Mass(10f);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
