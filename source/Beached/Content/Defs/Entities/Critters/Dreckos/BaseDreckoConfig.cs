using Beached.Content.DefBuilders;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Dreckos
{
	public abstract class BaseDreckoConfig : BaseCritterConfig
	{
		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(0, 5, 55, 60)
				.Mass(200)
				.Trappable()
				.Baggable()
				.Movable()
				.Faction(FactionManager.FactionID.Pest)
				.SortAfter(DreckoConfig.ID)
				.MaxPenSize(12)
				.Navigator(CritterBuilder.NAVIGATION.DRECKO, 1f)
				.Brain(GameTags.Creatures.Species.DreckoSpecies)
					.Configure(ConfigureAI)
				.Tag(GameTags.Creatures.CrabFriend)
				.Traits()
					.HP(25)
					.MaxAge(150)
					.Stomach(DreckoTuning.STANDARD_CALORIES_PER_CYCLE * 10, DreckoTuning.STANDARD_CALORIES_PER_CYCLE)
					.Done();
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);

			prefab.GetComponent<KPrefabID>().prefabInitFn += inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);

			return prefab;
		}

		protected override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
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
				.Add(new AttackStates.Def(), !isBaby)
				.PushInterruptGroup()
				.Add(new FixedCaptureStates.Def())
				.Add(new RanchedStates.Def(), !isBaby)
				.Add(new LayEggStates.Def(), !isBaby)
				.Add(new EatStates.Def())
				.Add(new DrinkMilkStates.Def()
				{
					shouldBeBehindMilkTank = isBaby
				})
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def(), isBaby)
				.Add(new CritterCondoStates.Def(), !isBaby)
				.PopInterruptGroup()
				.Add(new CreatureSleepStates.Def())
				.Add(new IdleStates.Def()
				{
					customIdleAnim = new IdleStates.Def.IdleAnimCallback(global::BaseDreckoConfig.CustomIdleAnim)
				});
		}
	}
}
