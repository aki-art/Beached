using Beached.Content.DefBuilders;
using Beached.Content.Scripts;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Karacoos
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyKaracooConfig : BaseKaracooConfig, IEntityConfig
	{
		public const string ID = "Beached_KaracooBaby";

		protected override string AnimFile => "beached_baby_karacoo_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = base.CreatePrefab(this);

			prefab.GetComponent<Karacoo>().randomizeColors = false;

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(KaracooConfig.ID, forceNavType: true)
				.Mass(10f)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_BABY, .75f);
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
