using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyJellyfishConfig : BaseJellyfishConfig, IEntityConfig
	{
		public const string ID = "Beached_JellyfishBaby";

		protected override string AnimFile => "beached_baby_jellyfish_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 1)
				.Baby(JellyfishConfig.ID)
				.Speed(0.15f);
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
