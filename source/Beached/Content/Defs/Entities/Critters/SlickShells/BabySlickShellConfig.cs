using Beached.Content.DefBuilders;
using Beached.Content.Defs.Items;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.SlickShells
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabySlickShellConfig : BaseSnailConfig, IEntityConfig
	{
		public const string ID = "Beached_SlickShellBaby";

		protected override string AnimFile => "beached_baby_slickshell_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Drops(SlickShellShellConfig.ID, 0.25f)
				.Baby(SlickShellConfig.ID)
				.Mass(25f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
