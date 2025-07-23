using Beached.Content.DefBuilders;
using Beached.Integration;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Squirrels
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class MerpipConfig : BaseMerpipConfig, IEntityConfig
	{
		public const string ID = "Beached_MerPip";
		public const string EGG_ID = "Beached_MerPipEgg";

		protected override string AnimFile => "beached_merpip_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Egg(BabyMerpipConfig.ID, "beached_egg_slagmite_kanim")
					.Fertility(0.1f)
					.Incubation(0.1f)
					.Mass(1f)
					.EggChance(EGG_ID, 0.1f)
					.EggChance(SquirrelConfig.EGG_ID, 0.9f)
					.Done();
		}

		public static void ConfigureEggChancesToMerpip()
		{
			var set = new HashSet<string>()
			{
				SquirrelConfig.ID,
				SquirrelHugConfig.ID
			};

			if (Mod.integrations.IsModPresent(Integrations.PIP_MORPHS))
			{
				set.Add(Integrations.IDS.PipMorphs.SQUIRREL_WINTER);
				set.Add(Integrations.IDS.PipMorphs.SQUIRREL_AUTUMN);
				set.Add(Integrations.IDS.PipMorphs.SQUIRREL_SPRING);
			}

			foreach (var id in set)
			{
				var pip = Assets.TryGetPrefab(id);
				if (pip == null)
					continue;

				pip.GetDef<FertilityMonitor.Def>().initialBreedingWeights.Add(new FertilityMonitor.BreedingChance()
				{
					egg = EGG_ID,
					weight = 0
				});
			}
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

		public string[] GetDlcIds() => null;
	}
}
