using Beached.Content.DefBuilders;
using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.SlickShells
{
	// Most basic form of a Slickshell, shared by adults, babies and all morphs
	public abstract class BaseSnailConfig : BaseCritterConfig
	{
		public const int SORTING_ORDER = 83;

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);
			prefab.AddTag(GameTags.Creatures.CrabFriend);
			prefab.AddTag(GameTags.Amphibious);

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(10, 20, 40, 50)
				.Size(1, 1)
				.Mass(50f)
				.Trappable()
				.Baggable()
				.CanNotDrown()
				.SortAfter(CrabConfig.ID)
				.MaxPenSize(CREATURES.SPACE_REQUIREMENTS.TIER3)
				.Navigator(CritterBuilder.NAVIGATION.FLOOR_NOJUMP_1X1, NavType.Floor, 0.25f, 16)
				.Brain(BTags.Species.snail)
					.Configure(ConfigureAI)
				.Traits()
					.HP(25)
					.MaxAge(25)
					.Stomach(SlickShellTuning.STANDARD_STOMACH_SIZE, SlickShellTuning.STANDARD_CALORIES_PER_CYCLE)
					.Done()
				.Tag(GameTags.Amphibious);
		}

		protected override sealed void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
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
				//.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new MucusSecretionStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new RanchedStates.Def(), isAdult)
				.Add(new LayEggStates.Def(), conditions.Contains(CritterBuilder.ADULT))
				.Add(new EatStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def(), !isAdult)
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}
