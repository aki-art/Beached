using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Karacoos
{
    [EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyKaracooConfig : BaseKaracooConfig, IEntityConfig
	{
		public const string ID = "Beached_BabyKaracoo";

		protected override string AnimFile => "beached_karacoo_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(KaracooConfig.ID, forceNavType: true)
				.Mass(10f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			// TODO: remove this when real animation is added
			inst.GetComponent<KBatchedAnimController>().animScale *= 0.5f;
		}
	}
}
