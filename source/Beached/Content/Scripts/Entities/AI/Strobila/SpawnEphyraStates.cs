namespace Beached.Content.Scripts.Entities.AI.Strobila
{
	public class SpawnEphyraStates : GameStateMachine<SpawnEphyraStates, SpawnEphyraStates.Instance, IStateMachineTarget, SpawnEphyraStates.Def>
	{
		private State idle;
		private State spawning;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
			}
		}
	}
}
