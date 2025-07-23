namespace Beached.Content.Scripts.Entities
{
	public class Beached_Smoker : StateMachineComponent<Beached_Smoker.StatesInstance>
	{
		public override void OnSpawn() => smi.StartSM();

		public class StatesInstance : GameStateMachine<States, StatesInstance, Beached_Smoker, object>.GameInstance
		{
			public ElementEmitter emitter;

			public StatesInstance(Beached_Smoker master) : base(master)
			{
				master.TryGetComponent(out emitter);
				emitter.SetEmitting(true);
			}
		}

		public class States : GameStateMachine<States, StatesInstance, Beached_Smoker>
		{
			public State idle;
			public State eruptingPre;
			public State erupting;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.PlayAnim("idle")
					.EnterTransition(eruptingPre, smi => !smi.emitter.isEmitterBlocked)
					.EventTransition(GameHashes.EmitterUnblocked, eruptingPre);

				eruptingPre
					.PlayAnim("erupting_pre")
					.OnAnimQueueComplete(erupting);

				erupting
					.PlayAnim("erupting", KAnim.PlayMode.Loop)
					.EnterTransition(idle, smi => smi.emitter.isEmitterBlocked)
					.EventTransition(GameHashes.EmitterBlocked, idle);
			}
		}
	}
}
