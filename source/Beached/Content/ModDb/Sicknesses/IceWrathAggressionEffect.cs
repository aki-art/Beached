using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb.Sicknesses
{
	public class IceWrathAggressionEffect : Sickness.SicknessComponent
	{
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			var smi = new StatesInstance(diseaseInstance, this);
			smi.StartSM();

			return smi;
		}

		public override void OnCure(GameObject go, object instance_data)
		{
			((StateMachine.Instance)instance_data).StopSM("Cured");
		}

		public override List<Descriptor> GetSymptoms()
		{
			return
			[
				new("Destructive", "This Duplicant is compelled to periodically become destructive.")
			];
		}

		public class StatesInstance :
		  GameStateMachine<States, StatesInstance, SicknessInstance, object>.GameInstance
		{
			public IceWrathAggressionEffect effect;
			public SicknessInstance sicknessInstance;

			public StatesInstance(SicknessInstance master, IceWrathAggressionEffect effect)
			  : base(master)
			{
				this.effect = effect;
				sicknessInstance = master.gameObject.GetSicknesses().Get(BSicknesses.iceWrath);
			}

			public float NextLashOutTimer()
			{
				if (sicknessInstance == null)
					return 30f;

				return Mathf.Clamp(sicknessInstance.GetPercentCured(), 0.1f, 1f) * 10f;
			}
		}

		public class States :
		  GameStateMachine<States, StatesInstance, SicknessInstance>
		{
			public State idle;
			public State lashingOut;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.ScheduleGoTo(smi => smi.NextLashOutTimer(), lashingOut);

				lashingOut
					.ToggleChore(smi => new AggressiveChore(smi.master), idle);
			}
		}
	}
}
