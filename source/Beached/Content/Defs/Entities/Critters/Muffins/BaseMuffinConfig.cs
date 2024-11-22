using Beached.Content.DefBuilders;
using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Muffins
{
	public abstract class BaseMuffinConfig : BaseCritterConfig
	{
		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(-70, -55, 65, 80)
				.Size(1, 1)
				.Mass(100f)
				.Trappable()
				.Baggable()
				.SortAfter(SquirrelConfig.ID)
				.MaxPenSize(32)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_1X2, 2f)
				.Brain(BTags.Species.muffin)
					.Configure(ConfigureAI)
				.Traits()
					.HP(100)
					.MaxAge(150)
					.Stomach(MuffinTuning.KCAL_PER_CYCLE * 10, MuffinTuning.KCAL_PER_CYCLE)
					.Done()
				.Tag(BTags.Creatures.doNotTargetMeByCarnivores);
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);
			prefab.AddOrGetDef<PreyMonitor.Def>().allyTags = [BTags.Creatures.doNotTargetMeByCarnivores];
			prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			prefab.AddOrGetDef<SolidConsumerMonitor.Def>();

			return prefab;
		}

		protected sealed override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
		{
			var isAdult = conditions.Contains(CritterBuilder.ADULT);

			builder
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
				.Add(new RanchedStates.Def(), isAdult) // needs excited_loop animation
				.Add(new HunterStates.Def())
				.Add(new LayEggStates.Def(), isAdult)
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}
