namespace Beached.Content.Scripts.Entities.Plant
{
	public class Filament : StateMachineComponent<Filament.StatesInstance>
	{
		public override void OnSpawn() => smi.StartSM();

		public class StatesInstance(Filament master) : GameStateMachine<States, StatesInstance, Filament, object>.GameInstance(master)
		{
			[MyCmpGet] public Light2D light2D;
		}

		public class States : GameStateMachine<States, StatesInstance, Filament>
		{
			public State emitting;
			public State dormant;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = dormant;

				dormant
					.TagTransition(GameTags.Decoration, emitting)
					.Enter(smi => smi.light2D.enabled = false);

				emitting
					.TagTransition(GameTags.Decoration, dormant, true)
					.Enter(smi => smi.light2D.enabled = true);
			}
		}
	}
}
