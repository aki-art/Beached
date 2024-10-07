using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Squirrels
{
	internal class MerpipConfig : BaseMerpipConfig, IEntityConfig
	{
		public const string ID = "Beached_MerPip";

		protected override string AnimFile => "beached_merpip_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Drops(MeatConfig.ID);
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);
			prefab.AddOrGetDef<SeedPlantingMonitor.Def>();
			prefab.AddComponent<Storage>();

			return prefab;
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
	}
}
