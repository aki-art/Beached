using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Muffins
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyMuffinConfig : BaseMuffinConfig, IEntityConfig
	{
		public const string ID = "Beached_BabyMuffin";

		protected override string AnimFile => "beached_muffin_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(MuffinConfig.ID)
				.Mass(25f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst)
		{
			inst.GetComponent<KBatchedAnimController>().animScale *= 0.5f;
		}
	}
}
