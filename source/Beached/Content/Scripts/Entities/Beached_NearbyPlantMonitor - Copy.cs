namespace Beached.Content.Scripts.Entities
{
	public class Beached_AgeFertilityMonitor :
	  GameStateMachine<Beached_AgeFertilityMonitor, Beached_AgeFertilityMonitor.Instance, IStateMachineTarget>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.Update((smi, dt) => smi.Update(dt), UpdateRate.SIM_1000ms);
		}

		public new class Instance(IStateMachineTarget master) : GameInstance(master)
		{
			public AgeMonitor.Instance ageMonitor = master.gameObject.GetSMI<AgeMonitor.Instance>();

			public delegate void IsAgedDelegate(float dt, float treshhold);

			public event IsAgedDelegate OnAged;

			public void Update(float dt)
			{
				OnAged(dt, ageMonitor.age.value);
			}
		}
	}
}
