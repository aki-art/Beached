using Beached.Content.ModDb;

namespace Beached.Content.Scripts.Entities.AI
{
	public class HunterStates : GameStateMachine<HunterStates, HunterStates.Instance, IStateMachineTarget, HunterStates.Def>
	{
		public TargetParameter target;
		public HuntStates huntEntity;
		public State behaviourcomplete;

		private static readonly CellOffset[] offsets =
		[
			new CellOffset(0, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		];

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = huntEntity.moveToThreat;

			root
				.Enter(SetMainTarget)
				.ToggleStatusItem(BStatusItems.hunting);

			huntEntity.moveToThreat
				.InitializeStates(masterTarget, target, huntEntity.attackThreat, override_offsets: offsets);

			huntEntity.attackThreat
				.PlayAnim("bite")
				.ScheduleAction("Bite", 0.5f, Bite)
				.OnAnimQueueComplete(behaviourcomplete);

			behaviourcomplete
				.BehaviourComplete(BTags.Creatures.hunting);
		}

		private void Bite(Instance smi)
		{
			smi.GetComponent<Weapon>().AttackTarget(target.Get(smi));
		}

		private void SetMainTarget(Instance smi)
		{
			target.Set(smi.GetSMI<PreyMonitor.Instance>().mainThreat, smi, false);
		}

		public class Def : BaseDef { }

		public new class Instance : GameInstance
		{
			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, BTags.Creatures.hunting);
			}
		}

		public class HuntStates : State
		{
			public ApproachSubState<AttackableBase> moveToThreat;
			public State attackThreat;
		}
	}
}
