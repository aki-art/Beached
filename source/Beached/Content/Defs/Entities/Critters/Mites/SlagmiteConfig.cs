﻿using Beached.Content.DefBuilders;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities.AI;
using Beached.Integration;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Mites
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class SlagmiteConfig : BaseMiteConfig, IEntityConfig
	{
		public const string ID = "Beached_Slagmite";
		public const string EGG_ID = "Beached_SlagmiteEgg";

		protected override string AnimFile => "beached_slagmite_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(this);

			SetupBasicSolidDiet(
				prefab,
				SlagDiet(),
				MiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
				MiteTuning.BASE.MIN_POOP_SIZE_IN_KG);

			prefab.AddOrGet<MineableCreature>().allowMining = true;

			var growthMonitor = prefab.AddOrGetDef<ShellGrowthMonitor.Def>();
			growthMonitor.levelCount = 9;
			growthMonitor.dropMass = 50f;
			growthMonitor.defaultGrowthRate = 100f / CONSTS.CYCLE_LENGTH;
			growthMonitor.lootTableId = BDb.lootTables.slagmiteShellDrops.Id;

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
					.EggChance(GleamiteConfig.EGG_ID, 0.02f)
					.Done();
		}

		public static Diet.Info[] SlagDiet()
		{
			var result = new List<Diet.Info>
			{
				new(
					[Elements.slag.CreateTag()],
					SimHashes.Granite.CreateTag(),
					MiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
					CREATURES.CONVERSION_EFFICIENCY.BAD_1),
				new(
					[SimHashes.Regolith.CreateTag()],
					SimHashes.Granite.CreateTag(),
					MiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
					CREATURES.CONVERSION_EFFICIENCY.GOOD_1)
			};

			if (Mod.integrations.IsModPresent(Integrations.CHEMICAL_PROCESSING))
			{
				result.Add(
					new(
						[Elements.ChemicalProcessing.solidSlag.CreateTag()],
						SimHashes.Granite.CreateTag(),
						MiteTuning.BASE.CALORIES_PER_KG_OF_ORE,
						CREATURES.CONVERSION_EFFICIENCY.BAD_1));
			}

			return result.ToArray();
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
