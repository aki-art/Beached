using Beached.Content.DefBuilders;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Pacus
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyPrincessPacuConfig : BasePrincessPacuConfig, IEntityConfig
	{
		public const string ID = "Beached_BabyPrincessPacu";

		protected override string AnimFile => "baby_pacu_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(PrincessPacuConfig.ID, forceNavType: true)
				.Mass(10f);
		}

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
