using Klei.AI;
using System;

namespace Beached.Content.Scripts
{

	public class MinionWetnessMonitor : GameStateMachine<MinionWetnessMonitor, MinionWetnessMonitor.Instance, IStateMachineTarget, MinionWetnessMonitor.Def>
	{

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.Update(UpdateWetness);
		}

		private void UpdateWetness(Instance instance, float dt)
		{
			throw new NotImplementedException();
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			private AmountInstance amount;
			private AttributeModifier modifier;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
			}
		}
	}
}
