using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT + 1)]
	public class BaseMuffinConfig
	{
		public static GameObject CreatePrefab(string id, string name, string desc, string animFile, bool isBaby, string symbolOverridePrefix = null)
		{
			var prefab = CritterBuilder.Create(id, animFile)
				.Name(name)
				.Description(desc)
				.Mass(100)
				.Size(1, 1)
				.Navigation(CONSTS.NAV_GRID.WALKER_1X2, NavType.Floor)
				.Temperatures(-60, -80, 50, 80)
				.DeathDrops((MeatConfig.ID, 1), (BasicFabricConfig.ID, 1))
				.Decor(DECOR.BONUS.TIER2)
				.Faction(FactionManager.FactionID.Predator)
				.CanDrown()
				.CanEntomb()
				.ShedFur(0.25f, Util.ColorFromHex("d7dfed"))
				.Build();

			ConfigureAI(prefab, symbolOverridePrefix, BTags.Species.muffin, isBaby);
			EntityTemplates.CreateAndRegisterBaggedCreature(prefab, true, true);

			prefab.AddTag(GameTags.Creatures.Walker);

			prefab.AddWeapon(2f, 3f);
			prefab.AddOrGetDef<CreatureFallMonitor.Def>();
			prefab.AddOrGet<Trappable>();
			//placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;

			return prefab;
		}

		public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
		{
			var diet = new Diet([.. diet_infos]);
			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minConsumedCaloriesBeforePooping = referenceCaloriesPerKg * minPoopSizeInKg;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return prefab;
		}

		public static void ConfigureAI(GameObject gameObject, string symbolOverridePrefix, Tag species, bool isBaby)
		{
			var choreTable = new ChoreTable.Builder()
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				//.Add(new GrowUpStates.Def())
				.Add(new TrappedStates.Def())
				//.Add(new IncubatingStates.Def())
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new EatStates.Def())
				.Add(new RanchedStates.Def())
				.Add(new HunterStates.Def(), isBaby)
				//.Add(new LayEggStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(gameObject, choreTable, species, symbolOverridePrefix);
		}
	}
}
