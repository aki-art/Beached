namespace Beached.Content.Scripts.Entities
{

	public class Strobila : StateMachineComponent<Strobila.StatesInstance>
	{
		public override void OnSpawn()
		{
			smi.StartSM();
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Strobila, object>.GameInstance
		{
			public StatesInstance(Strobila master) : base(master)
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, Strobila>
		{
			public State growing;
			public State emitting;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = growing;
			}
		}
	}
}
