using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Mites
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyGleamiteConfig : BaseMiteConfig, IEntityConfig
	{
		public const string ID = "Beached_GleamiteBaby";

		protected override string AnimFile => "beached_baby_slagmite_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(GleamiteConfig.ID, forceNavType: true)
				.Size(1, 1)
				.Mass(5f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
