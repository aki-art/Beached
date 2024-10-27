using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	[EntityConfigOrder(100)]
	public class BabyJellyfishConfig : BaseJellyfishConfig, IEntityConfig
	{
		public const string ID = "Beached_Jellyfish_Baby";

		protected override string AnimFile => "beached_jellyfish_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 1)
				.Baby(JellyfishConfig.ID)
				.Speed(0.15f);
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst)
		{
			if (inst.TryGetComponent(out KBatchedAnimController kbac))
			{
				kbac.animScale *= 0.5f; // TODO: custom anim
			}
		}
	}
}
