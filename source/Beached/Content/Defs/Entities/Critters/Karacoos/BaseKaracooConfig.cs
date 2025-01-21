using Beached.Content.DefBuilders;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Karacoos
{
	public abstract class BaseKaracooConfig : BaseCritterConfig
	{
		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.Decor(10, 2)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_1X2, 1f)
				.TemperatureCelsius(-20, -5, 45, 60)
				.Trappable()
				.Baggable()
				.SortAfter(SquirrelConfig.ID)
				.MaxPenSize(TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3)
				.Brain(BTags.Species.karacoo)
					.Configure(ConfigureAI)
				.Traits()
					.HP(50)
					.MaxAge(60)
					.Stomach(50_000, 5_000)
					.Done();
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			return base.CreatePrefab(config)
				.AddComponent<Karacoo>()
				.gameObject;
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
				//.Add(new DefendStates.Def())
				//.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				//.Add(new RanchedStates.Def())
				.Add(new LayEggStates.Def(), isAdult)
				.Add(new EatStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def())
				.Add(new SitOnEggStates.Def(), isAdult)
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}
