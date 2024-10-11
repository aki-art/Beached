using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Squirrels
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyMerpipConfig : BaseMerpipConfig, IEntityConfig
	{
		public const string ID = "Beached_BabyMerpip";

		protected override string AnimFile => "baby_squirrel_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(MerpipConfig.ID, forceNavType: true)
				.Mass(10f);
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
