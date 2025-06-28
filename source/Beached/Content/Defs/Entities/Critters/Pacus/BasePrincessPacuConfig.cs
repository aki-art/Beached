using Beached.Content.DefBuilders;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Pacus
{
	public abstract class BasePrincessPacuConfig : BaseCritterConfig
	{
		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);

			prefab.AddOrGetDef<LureableMonitor.Def>().lures =
			[
				GameTags.Creatures.FishTrapLure,
				GameTags.Creatures.FlyersLure
			];

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(10, 15, 50, 60)
				.Drops(FishMeatConfig.ID)
				.Decor(3, 30)
				.Fish()
				.Size(1, 1)
				.Mass(200f)
				.Trappable()
				.Baggable()
				.Swimmer()
				.Condo(UnderwaterCritterCondoConfig.ID, false)
				.Sorting(CREATURES.SORTING.CRITTER_ORDER[PacuConfig.ID])
				.CritterDensityTolerance(CREATURES.SPACE_REQUIREMENTS.TIER3)
				.Navigator(CritterBuilder.NAVIGATION.SWIMMER, NavType.Swim, 2f, 16)
				.Brain(GameTags.Creatures.Species.PacuSpecies)
					.Configure(ConfigureAI)
				.Traits()
					.HP(25)
					.MaxAge(100)
					.Stomach(PacuTuning.STANDARD_STOMACH_SIZE, PacuTuning.STANDARD_CALORIES_PER_CYCLE / CONSTS.CYCLE_LENGTH)
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
				.Add(new FallStates.Def()
				{
					getLandAnim = BasePacuConfig.GetLandAnim
				})
				.Add(new DebugGoToStates.Def())
				.Add(new FlopStates.Def())
				.PushInterruptGroup()
				.Add(new FixedCaptureStates.Def())
				//.Add(new RanchedStates.Def(), !isBaby)
				.Add(new LayEggStates.Def(), !isBaby)
				//.Add(new TreeClimbStates.Def())
				.Add(new EatStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "lay_egg_pre", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new MoveToLureStates.Def())
				.Add(new CritterCondoStates.Def(), !isBaby)
				.PopInterruptGroup()
				.Add(new IdleStates.Def());
		}
	}
}
