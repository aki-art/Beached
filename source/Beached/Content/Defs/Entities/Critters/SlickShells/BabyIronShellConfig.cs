using Beached.Content.DefBuilders;
using Beached.Content.Defs.Items;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.SlickShells
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyIronShellConfig : BaseSnailConfig, IEntityConfig
	{
		public const string ID = "Beached_IronShellBaby";

		protected override string AnimFile => "beached_baby_slickshell_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(IronShellConfig.ID)
				.TemperatureCelsius(150, 240, 340, 410)
				.DefaultTemperatureC(200)
				.Drops(IronShellShellConfig.ID, 0.25f)
				.Mass(25f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
