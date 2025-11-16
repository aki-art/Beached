using Beached.Content.DefBuilders;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Pufts
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY)]
	public class BabyAmmoniaPuftConfig : BasePuftConfig, IEntityConfig
	{
		public const string ID = "Beached_AmmoniaPuftBaby";

		protected override string AnimFile => "baby_puft_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Baby(AmmoniaPuftConfig.ID)
				.Size(1, 1);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
