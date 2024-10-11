using Beached.Content.DefBuilders;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.Defs.Entities.Critters.Squirrels
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public abstract class BaseMerpipConfig : BaseCritterConfig
	{
		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(0, 10, 35, 60)
				.Size(1, 1)
				.Mass(100f)
				.Trappable()
				.Baggable()
				.Swimmer()
				//.Condo(UnderwaterCritterCondoConfig.ID, false)
				.Sorting(CREATURES.SORTING.CRITTER_ORDER[SquirrelConfig.ID])
				.MaxPenSize(CREATURES.SPACE_REQUIREMENTS.TIER3)
				.Navigator(CritterBuilder.NAVIGATION.SWIMMER, NavType.Swim, 2f, 16)
				.Brain(GameTags.Creatures.Species.SquirrelSpecies)
					.Configure(ConfigureAI)
				.Traits()
					.HP(25)
					.MaxAge(100)
					.Stomach(SquirrelTuning.STANDARD_STOMACH_SIZE, SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / CONSTS.CYCLE_LENGTH)
					.Done()
				.Tag(GameTags.Amphibious);
		}

		protected sealed override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
		{
			var isBaby = !conditions.Contains(CritterBuilder.ADULT);

			builder
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new GrowUpStates.Def(), isBaby)
				.Add(new TrappedStates.Def())
				.Add(new IncubatingStates.Def(), isBaby)
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DrowningStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				.Add(new AttackStates.Def())
				.PushInterruptGroup()
				.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				//.Add(new RanchedStates.Def(), !isBaby)
				.Add(new LayEggStates.Def(), !isBaby)
				//.Add(new TreeClimbStates.Def())
				.Add(new EatStates.Def())
				.Add(new DrinkMilkStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def(), isBaby)
				.Add(new SeedPlantingStates.Def(""))
				.Add(new CritterCondoStates.Def(), !isBaby)
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}
