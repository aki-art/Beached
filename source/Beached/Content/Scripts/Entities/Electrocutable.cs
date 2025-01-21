namespace Beached.Content.Scripts.Entities
{
	public class Electrocutable : StateMachineComponent<Electrocutable.StatesInstance>
	{
		public override void OnSpawn()
		{
			smi.StartSM();
			Beached_Grid.Instance.OnElectricChargeAdded += OnElectricCharge;
		}

		private void OnElectricCharge(int cell, float power)
		{
			if (Grid.PosToCell(this) == cell)
				smi.sm.electricCharge.Set(power, smi);
		}

		protected virtual void GetElectrocuted(float power)
		{
			Debug.Log("zapped");

			if (smi.health != null)
				smi.health.Damage(power * 100f);

			smi.sm.electricCharge.Set(0, smi);
		}

		public class StatesInstance(Electrocutable master) : GameStateMachine<States, StatesInstance, Electrocutable, object>.GameInstance(master)
		{
			public Health health = master.GetComponent<Health>();
		}

		public class States : GameStateMachine<States, StatesInstance, Electrocutable>
		{
			public FloatParameter electricCharge;

			public State idle;
			public State zapped;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.ParamTransition(electricCharge, zapped, IsLTZero);

				zapped
					.Enter(smi => smi.master.GetElectrocuted(smi.sm.electricCharge.Get(smi)))
					.ScheduleGoTo(1f, idle);
			}
		}
	}
}
