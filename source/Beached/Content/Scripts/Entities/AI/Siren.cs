using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class Siren : GameStateMachine<Siren, Siren.Instance, IStateMachineTarget>
	{
		private State idle;
		private State angry;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			idle
				.EventTransition(GameHashes.StressedHadEnough, angry);

			angry
				.Enter(TintPurple)
				.Exit(ResetTint)
				.EventTransition(GameHashes.NotStressed, idle)
				.ToggleStatusItem("angry", "");
		}

		private void TintPurple(Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().TintColour = new Color(1, 0, 1);
		}

		private void ResetTint(Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().TintColour = Color.white;
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master) : base(master) { }
		}
	}
}
