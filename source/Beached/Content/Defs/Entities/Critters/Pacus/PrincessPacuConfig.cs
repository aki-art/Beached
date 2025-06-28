using Beached.Content.DefBuilders;
using Beached.Integration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Pacus
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class PrincessPacuConfig : BasePrincessPacuConfig, IEntityConfig
	{
		public const string ID = "Beached_PrincessPacu";
		public const string EGG_ID = "Beached_PrincessPacuEgg";

		protected override string AnimFile => "beached_princess_pacu_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Egg(BabyPrincessPacuConfig.ID, "egg_pacu_kanim")
					.Fertility(15f)
					.Incubation(5f)
					.Mass(1f)
					.EggChance(EGG_ID, 0.1f)
					.EggChance(EGG_ID, 0.9f)
					.Done();
		}

		public static void ConfigureEggChancesToOtherPacus()
		{
			var set = new HashSet<string>()
			{
				PacuConfig.ID,
				PacuTropicalConfig.ID,
				BabyPacuCleanerConfig.ID
			};

			if (Mod.integrations.IsModPresent(Integrations.PACU_MORPHS))
			{
				set.Add(Integrations.IDS.PipMorphs.SQUIRREL_WINTER);
				set.Add(Integrations.IDS.PipMorphs.SQUIRREL_AUTUMN);
				set.Add(Integrations.IDS.PipMorphs.SQUIRREL_SPRING);
			}

			foreach (var id in set)
			{
				var critter = Assets.TryGetPrefab(id);
				if (critter == null)
					continue;

				critter.GetDef<FertilityMonitor.Def>().initialBreedingWeights.Add(new FertilityMonitor.BreedingChance()
				{
					egg = EGG_ID,
					weight = 0
				});
			}
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
	}
}
