using Beached.Content.DefBuilders;
using Beached.Content.Defs.Foods;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Mites
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabySlagmiteConfig : BaseMiteConfig, IEntityConfig
	{
		public const string ID = "Beached_BabySlagmite";

		protected override string AnimFile => "beached_baby_slagmite_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(SlagmiteConfig.ID, forceNavType: true)
				.Drops(CracklingsConfig.ID, 1f)
				.Size(1, 1)
				.Mass(5f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
