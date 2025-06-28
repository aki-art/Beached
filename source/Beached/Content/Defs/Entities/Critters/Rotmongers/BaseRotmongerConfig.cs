using Beached.Content.DefBuilders;
using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;

namespace Beached.Content.Defs.Entities.Critters.Rotmongers
{
	public abstract class BaseRotmongerConfig : BaseCritterConfig
	{
		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(30, 40, 80, 90)
				.Mass(100f)
				.Drops(MeatConfig.ID)
				.Trappable()
				.Baggable()
				.Faction(FactionManager.FactionID.Pest)
				.SortAfter(HatchConfig.ID)
				.CritterDensityTolerance(8)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_1X1, 2f)
				.Brain(BTags.Species.rotMonger)
					.Configure(ConfigureAI)
				.Traits()
					.HP(50)
					.MaxAge(50)
					.Stomach(MuffinTuning.KCAL_PER_CYCLE * 10, MuffinTuning.KCAL_PER_CYCLE)
					.Done();
		}

		protected sealed override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
		{
			var adult = conditions.Contains(CritterBuilder.ADULT);

			builder
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new GrowUpStates.Def(), !adult)
				.Add(new TrappedStates.Def())
				.Add(new IncubatingStates.Def(), !adult)
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new MinedStates.Def(), adult)
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new EatStates.Def())
				.Add(new RanchedStates.Def(), adult)
				.Add(new LayEggStates.Def(), adult)
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}
