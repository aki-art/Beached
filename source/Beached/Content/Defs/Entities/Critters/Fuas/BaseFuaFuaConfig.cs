using Beached.Content.DefBuilders;
using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Defs.Foods;
using System.Collections.Generic;

namespace Beached.Content.Defs.Entities.Critters.Fuas
{
	public abstract class BaseFuaFuaConfig : BaseCritterConfig
	{
		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(15, 25, 65, 70)
				.Size(1, 1)
				.Speed(0.5f)
				.Mass(10f)
				.Trappable()
				.Baggable()
				.Movable()
				.CanNotDrown()
				.Drops(RawSnailConfig.ID, 0.5f)
				.Faction(FactionManager.FactionID.Pest)
				.SortBefore(OilFloaterConfig.ID)
				.CritterDensityTolerance(36)
				.Navigator(CritterBuilder.NAVIGATION.FUAFUA, 0.5f)
				.Brain(BTags.Species.fuafua)
					.Configure(ConfigureAI)
				.Traits()
					.HP(25)
					.MaxAge(200)
					.Stomach(MuffinTuning.KCAL_PER_CYCLE * 10, MuffinTuning.KCAL_PER_CYCLE)
					.Done();
		}

		protected override void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions)
		{
			builder
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new TrappedStates.Def())
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.PopInterruptGroup()
				.Add(new IdleStates.Def()
				{
					customIdleAnim = new IdleStates.Def.IdleAnimCallback(CustomIdleAnim)
				});
		}

		private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
		{
			var offset = new CellOffset(0, -1);
			var facing = smi.GetComponent<Facing>().GetFacing();
			var navigator = smi.GetComponent<Navigator>();

			switch (navigator.CurrentNavType)
			{
				case NavType.Floor:
					offset = facing ? new CellOffset(1, -1) : new CellOffset(-1, -1);
					break;
				case NavType.Ceiling:
					offset = facing ? new CellOffset(1, 1) : new CellOffset(-1, 1);
					break;
			}

			var cell = Grid.OffsetCell(Grid.PosToCell(smi), offset);
			return (Grid.IsValidCell(cell) && !Grid.Solid[cell]) ? "idle_loop_hang" : "idle_loop";
		}
	}
}
