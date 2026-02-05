using Beached.Content.DefBuilders;
using Beached.Content.Defs.Foods;
using Beached.Content.Navigation;
using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
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

			prefab.GetComponent<KPrefabID>().prefabSpawnFn += go =>
			{
				if (go.TryGetComponent(out Navigator navigator))
					navigator.transitionDriver.overrideLayers.Add(new SadSnailTransitionLayer(navigator));
			};

			prefab.AddOrGet<LoopingSounds>();
			prefab.AddOrGet<OilFloaterMovementSound>().sound = "OilFloater_move_LP";

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return builder
				.TemperatureCelsius(-5, 35, 145, 230)
				.DefaultTemperatureC(40)
				.Drops(RawSnailConfig.ID, 1f)
				.Size(1, 1)
				.Mass(50f)
				.Trappable()
				.Baggable()
				.CanNotDrown()
				.SortAfter(CrabConfig.ID)
				.CritterDensityTolerance(TUNING.CREATURES.SPACE_REQUIREMENTS.TIER2)
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
				.Add(new DrinkMilkStates.Def()
				{
					shouldBeBehindMilkTank = true
				})
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def(), !isAdult)
				.Add(new CritterCondoStates.Def(), isAdult)
				.PopInterruptGroup()
				.Add(new IdleStates.Def()
				{
					customIdleAnim = new IdleStates.Def.IdleAnimCallback(CustomIdleAnim)
				});
		}

		private static readonly HashedString
			IDLE_LOOP = "idle_loop",
			IDLE_LOOP_SAD = "idle_loop_sad";

		private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
		{
			var desiccationMonitor = smi.GetSMI<DesiccationMonitor.Instance>();
			return (desiccationMonitor == null || !desiccationMonitor.IsDesiccating()) ? IDLE_LOOP : IDLE_LOOP_SAD;
		}

		public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
		{
			return Grid.IsValidCell(Grid.PosToCell(smi.transform.GetPosition()));
		}

		public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
		{
			if (IsValidTimeToDrop(smi) && IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry))
			{
				return smi.prefabID.HasTag(GameTags.Creatures.Happy);
			}

			return false;
		}

		public static bool IsValidTimeToDrop(MoltDropperMonitor.Instance smi)
		{
			if (!smi.spawnedThisCycle)
			{
				if (!(smi.timeOfLastDrop <= 0f))
				{
					return GameClock.Instance.GetTime() - smi.timeOfLastDrop > 600f;
				}

				return true;
			}

			return false;
		}
	}
}
