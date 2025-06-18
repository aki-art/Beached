using Beached.Content.DefBuilders;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Pufts
{
	public abstract class BasePuftConfig : BaseCritterConfig
	{
		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);

			prefab.AddOrGetDef<LureableMonitor.Def>().lures =
			[
				GameTags.SlimeMold,
				GameTags.Creatures.FlyersLure
			];

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(15, 15, 55, 70)
				.Mass(50f)
				.Drops(MeatConfig.ID)
				.Trappable()
				.Baggable()
				.Faction(FactionManager.FactionID.Pest)
				.SortAfter(PuftConfig.ID)
				.CritterDensityTolerance(PuftTuning.PEN_SIZE_PER_CREATURE)
				.Navigator(CritterBuilder.NAVIGATION.FLYER_1X1, 2f)
				.Brain(GameTags.Creatures.Species.PuftSpecies)
					.Configure(ConfigureAI)
				.Traits()
					.HP(75)
					.MaxAge(25)
					.Stomach(PuftTuning.STANDARD_STOMACH_SIZE, PuftTuning.STANDARD_CALORIES_PER_CYCLE)
					.Done();
		}

		protected sealed override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
		{
			var isBaby = !conditions.Contains(CritterBuilder.ADULT);

			builder
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new GrowUpStates.Def(), isBaby)
				.Add(new IncubatingStates.Def(), isBaby)
				.Add(new TrappedStates.Def())
				.Add(new BaggedStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new DrowningStates.Def())
				.PushInterruptGroup()
				.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new RanchedStates.Def(), !isBaby)
				.Add(new UpTopPoopStates.Def())
				.Add(new LayEggStates.Def(), !isBaby)
				.Add(new InhaleStates.Def()
				{
					inhaleSound = isBaby ? "PuftBaby_air_intake" : "Puft_air_intake"
				})
				.Add(new DrinkMilkStates.Def()
				{
					shouldBeBehindMilkTank = !isBaby
				})
				.Add(new MoveToLureStates.Def())
				.Add(new CallAdultStates.Def(), isBaby)
				.Add(new CritterCondoStates.Def
				{
					working_anim = "cc_working_puft"
				}, !isBaby)
				.PopInterruptGroup()
				.Add(new IdleStates.Def()
				{
					customIdleAnim = new IdleStates.Def.IdleAnimCallback(global::BasePuftConfig.CustomIdleAnim)
				});
		}
	}
}
