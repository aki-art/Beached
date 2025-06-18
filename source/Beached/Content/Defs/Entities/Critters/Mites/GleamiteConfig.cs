using Beached.Content.DefBuilders;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities.AI;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Mites
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class GleamiteConfig : BaseMiteConfig, IEntityConfig
	{
		public const string ID = "Beached_Gleamite";
		public const string EGG_ID = "Beached_GleamiteEgg";

		protected override string AnimFile => "beached_slagmite_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(this);

			SetupBasicSolidDiet(
				prefab,
				SlagmiteConfig.SlagDiet(),
				MiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
				MiteTuning.BASE.MIN_POOP_SIZE_IN_KG);

			prefab.AddOrGet<MineableCreature>().allowMining = true;

			var growthMonitor = prefab.AddOrGetDef<ShellGrowthMonitor.Def>();
			growthMonitor.levelCount = 9;
			growthMonitor.dropMass = 50f;
			growthMonitor.defaultGrowthRate = 50f / CONSTS.CYCLE_LENGTH;
			growthMonitor.lootTableId = BDb.lootTables.gleamiteShellDrops.Id;

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 3)
				.Tag(GameTags.OriginalCreature)
				.Egg(BabySlagmiteConfig.ID, "beached_egg_slagmite_kanim")
					.Fertility(10f)
					.Incubation(1)
					.Mass(1f)
					.EggChance(EGG_ID, 0.98f)
					.EggChance(SlagmiteConfig.EGG_ID, 0.02f)
					.Done();
		}

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			inst.GetComponent<KBatchedAnimController>().TintColour = Color.cyan;
		}
	}
}
