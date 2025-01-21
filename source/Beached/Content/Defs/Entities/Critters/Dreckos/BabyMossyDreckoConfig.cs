using Beached.Content.DefBuilders;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Dreckos
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	internal class BabyMossyDreckoConfig : BaseDreckoConfig, IEntityConfig
	{
		public const string ID = "Beached_MossyDrecko_Baby";

		protected override string AnimFile => "beached_baby_mossy_drecko_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = base.CreatePrefab(this);

			SymbolOverrideControllerUtil.AddToPrefab(prefab);

			// TODO: temporary for placeholder kanim
			if (!prefab.TryGetComponent(out SymbolOverrideController controller))
				controller = SymbolOverrideControllerUtil.AddToPrefab(prefab);

			controller.ApplySymbolOverridesByAffix(Assets.GetAnim(AnimFile), "fbr_");

			prefab.AddOrGet<CreatureBrain>().symbolPrefix = "fbr_";

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(MossyDreckoConfig.ID)
				.Navigator(CritterBuilder.NAVIGATION.DRECKO_BABY);
		}
		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
