namespace Beached.Content.Scripts.Entities.AI
{
	// workaround to stress reactions always wanting a chore, even if it doesn't make sense for the reaction
	// Minnow's stress reaction Siren is an example of this
	public class EmptyChore : Chore<EmptyChore.StatesInstance>
	{
		public EmptyChore(IStateMachineTarget target) : base(
			Db.Get().ChoreTypes.StressIdle,
			target,
			target.GetComponent<ChoreProvider>(),
			false,
			null,
			null,
			null,
			PriorityScreen.PriorityClass.compulsory)
		{
			smi = new StatesInstance(this);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, EmptyChore, object>.GameInstance
		{
			public StatesInstance(EmptyChore master) : base(master) { }
		}

		public class States : GameStateMachine<States, StatesInstance, EmptyChore>
		{
			public override void InitializeStates(out BaseState default_state)
			{
				default_state = root;

				root
					.ReturnSuccess();
			}
		}
	}
}