using Beached.Content.DefBuilders;
using Beached.Content.Defs.Buildings;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.AI.Jellyfish;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class JellyfishConfig : BaseJellyfishConfig, IEntityConfig
	{
		public const string ID = "Beached_Jellyfish";
		public const string EGG_ID = "Beached_JellyfishEgg";
		public const string BASE_TRAIT_ID = "Beached_JellyfishTrait";

		protected override string AnimFile => "beached_jellyfish_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 2)
				.Speed(0.25f)
				/*				.Egg(BabyJellyfishConfig.ID, "beached_egg_slickshell_kanim")
									.Incubation(20)
									.Fertility(60)
									.NotRanchable()
									.EggChance(EGG_ID, 1)
									.Mass(1)
									.Done()*/
				.Tags([GameTags.OriginalCreature]);
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);
			var electricEmitter = prefab.AddComponent<ElectricEmitter>();
			electricEmitter.maxCells = 128;
			electricEmitter.powerLossMultiplier = 1;
			electricEmitter.minPathSecs = 0.3f;
			electricEmitter.maxPathSecs = 2f;

			var nesting = prefab.AddOrGetDef<NestingFertilityMonitor.Def>();
			nesting.baseFertileCycles = 0.5f;

			var jellyFish = prefab.AddOrGet<Jellyfish>();
			jellyFish.connectorDefId = JellyfishGeneratorConfig.ID;
			jellyFish.pulseDurationSeconds = 10;

			prefab.AddOrGetDef<PulseMonitor.Def>();

			// discover baby when discovering adult
			var kPrefabId = prefab.GetComponent<KPrefabID>();
			kPrefabId.prefabSpawnFn += (inst =>
			{
				DiscoveredResources.Instance.Discover(BabyJellyfishConfig.ID, DiscoveredResources.GetCategoryForTags(kPrefabId.Tags));
			});

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
