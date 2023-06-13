namespace Beached.Content.Scripts.LifeGoals
{
	public class AssignableGoal : StateMachineComponent<AssignableGoal.StatesInstance>
	{
		public override void OnSpawn()
		{
			smi.StartSM();
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, AssignableGoal, object>.GameInstance
		{
			[MyCmpGet]
			public Beached_LifeGoalTracker lifeGoals;

			[MyCmpGet]
			public Equipment equipment;

			public StatesInstance(AssignableGoal master) : base(master)
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, AssignableGoal>
		{
			public State idle;
			public State celebrate;
			public State satisfied;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.EventHandlerTransition(GameHashes.AssignablesChanged, satisfied, HasWantedAssignable);

				satisfied
					.TriggerOnEnter(ModHashes.lifeGoalFulfilled)
					.EventHandlerTransition(GameHashes.AssignablesChanged, idle, HasWantedAssignable)
					.TriggerOnExit(ModHashes.lifeGoalLost);
			}

			private bool HasWantedAssignable(StatesInstance smi, object data) => smi.lifeGoals.HasWantedAssignable(data as AssignableSlotInstance);
		}
	}
}
