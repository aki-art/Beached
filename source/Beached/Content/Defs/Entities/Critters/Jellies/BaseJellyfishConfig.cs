using Beached.Content.DefBuilders;
using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities.AI;
using Beached.Content.Scripts.Entities.AI.Jellyfish;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	public abstract class BaseJellyfishConfig : BaseCritterConfig
	{
		public const int SORTING_ORDER = 53;

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);
			prefab.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;

			var planktonDiet = new Diet(new Diet.Info(
				[PlanktonGerms.ID],
				SimHashes.SlimeMold.ToString(),
				JellyfishTuning.CALORIES_PER_GERM,
				JellyfishTuning.CONVERSION_RATE,
				food_type: Diet.Info.FoodType.EatSolid));//BDb.FoodTypes.GermDiet));

			var creatureCalorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			creatureCalorieMonitor.diet = planktonDiet;

			var germConsumerMonitor = prefab.AddOrGetDef<GermConsumerMonitor.Def>();
			germConsumerMonitor.diet = planktonDiet;
			germConsumerMonitor.consumableGermIdx = Db.Get().Diseases.GetIndex(BDiseases.plankton.id);

			prefab.AddOrGetDef<LureableMonitor.Def>().lures =
				[
					GameTags.Creatures.FishTrapLure
				];

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(0, 5, 55, 60)
				.Mass(10f)
				.Trappable()
				.Baggable()
				.Movable()
				.CanNotDrown()
				.Drops(SimHashes.Water.ToString(), 10f)
				.Faction(FactionManager.FactionID.Prey)
				.SortAfter(PacuConfig.ID)
				.CritterDensityTolerance(36)
				.Navigator(CritterBuilder.NAVIGATION.SWIMMER, 0.25f)
				.Brain(BTags.Species.jellyfish)
					.Configure(ConfigureAI)
				.Tag(GameTags.Creatures.CrabFriend)
				.Tag(BTags.germDiet)
				.Traits()
					.HP(25)
					.MaxAge(200)
					.Stomach(JellyfishTuning.STANDARD_STOMACH_SIZE, JellyfishTuning.STANDARD_CALORIES_PER_CYCLE)
					.Done();
		}

		protected override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
		{
			var isAdult = conditions.Contains(CritterBuilder.ADULT);

			builder
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new GrowUpStates.Def(), !isAdult)
				.Add(new TrappedStates.Def())
				.Add(new IncubatingStates.Def(), !isAdult)
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				//.Add(new DefendStates.Def())
				.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				//.Add(new RanchedStates.Def())
				//.Add(new LayEggStates.Def(), isAdult)
				.Add(new StrobilaLayingStates.Def(), isAdult)
				.Add(new GermSuckStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new EnergizedStates.Def(), isAdult)
				.Add(new CallAdultStates.Def(), !isAdult)
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}

