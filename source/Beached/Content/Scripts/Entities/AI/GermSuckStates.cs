namespace Beached.Content.Scripts.Entities.AI
{
	public class GermSuckStates : GameStateMachine<GermSuckStates, GermSuckStates.Instance, IStateMachineTarget, GermSuckStates.Def>
	{
		public State goingtoeat;
		public State behaviourcomplete;
		public InhalingStates inhaling;
		public IntParameter targetCell;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = goingtoeat;

			root
				.Enter("SetTarget", smi => targetCell.Set(smi.monitor.targetCell, smi));

			goingtoeat
				.MoveTo(targetCell.Get, inhaling)
				.ToggleMainStatusItem(GetMovingStatusItem);

			inhaling
				.DefaultState(inhaling.inhale)
				.ToggleStatusItem(global::STRINGS.CREATURES.STATUSITEMS.INHALING.NAME, global::STRINGS.CREATURES.STATUSITEMS.INHALING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);

			inhaling.inhale
				.PlayAnim(smi => smi.def.inhaleAnimPre)
				.QueueAnim(smi => smi.def.inhaleAnimLoop, true)
				.Update("Consume", (smi, dt) => smi.monitor.Consume(dt))
				.EventTransition(GameHashes.ElementNoLongerAvailable, inhaling.pst)
				.Enter("StartInhaleSound", smi => smi.StartInhaleSound())
				.Exit("StopInhaleSound", smi => smi.StopInhaleSound())
				.ScheduleGoTo(smi => smi.def.inhaleTime, inhaling.pst);

			inhaling.pst
				.Transition(inhaling.full, smi => smi.def.alwaysPlayPstAnim || IsFull(smi))
				.Transition(behaviourcomplete, Not(IsFull));

			inhaling.full
				.QueueAnim(smi => smi.def.inhaleAnimPst)
				.OnAnimQueueComplete(behaviourcomplete);

			behaviourcomplete
				.PlayAnim("idle_loop", KAnim.PlayMode.Loop)
				.BehaviourComplete(smi => smi.def.behaviourTag);
		}

		private static StatusItem GetMovingStatusItem(Instance smi) => smi.def.useStorage
			? smi.def.storageStatusItem
			: Db.Get().CreatureStatusItems.LookingForFood;

		private static bool IsFull(Instance smi)
		{
			if (smi.def.useStorage)
			{
				if (smi.storage != null)
				{
					return smi.storage.IsFull();
				}
			}
			else
			{
				var creatureCalorieMonitor = smi.GetSMI<CreatureCalorieMonitor.Instance>();
				if (creatureCalorieMonitor != null)
				{
					return (double)creatureCalorieMonitor.stomach.GetFullness() >= 1.0;
				}
			}

			return false;
		}

		public class Def : BaseDef
		{
			public string inhaleSound;
			public float inhaleTime = 3f;
			public Tag behaviourTag = GameTags.Creatures.WantsToEat;
			public bool useStorage;
			public string inhaleAnimPre = "inhale_pre";
			public string inhaleAnimLoop = "inhale_loop";
			public string inhaleAnimPst = "inhale_pst";
			public bool alwaysPlayPstAnim;
			public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
		}

		public new class Instance : GameInstance
		{
			public string inhaleSound;

			[MySmiGet] public GermConsumerMonitor.Instance monitor;
			[MyCmpGet] public Storage storage;
			[MyCmpGet] public LoopingSounds loopingSounds;

			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
				inhaleSound = GlobalAssets.GetSound(def.inhaleSound);
			}

			public void StartInhaleSound()
			{
				if (loopingSounds != null)
				{
					loopingSounds.StartSound(smi.inhaleSound);
				}
			}

			public void StopInhaleSound()
			{
				if (loopingSounds != null)
				{
					loopingSounds.StopSound(smi.inhaleSound);
				}
			}
		}

		public class InhalingStates : State
		{
			public State inhale;
			public State pst;
			public State full;
		}
	}
}
