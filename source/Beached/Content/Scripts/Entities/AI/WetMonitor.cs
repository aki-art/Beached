using Beached.Content.ModDb;
using Klei.AI;

namespace Beached.Content.Scripts.Entities.AI
{
	public class WetMonitor : GameStateMachine<WetMonitor, WetMonitor.Instance, IStateMachineTarget, WetMonitor.Def>
	{
		private State dry;
		private State wet;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = dry;

			dry
				.UpdateTransition(wet, IsInLiquid)
				.ToggleAttributeModifier("dry", smi => smi.baseMoistureModifier);

			wet
				.UpdateTransition(dry, (smi, dt) => !IsInLiquid(smi, dt))
				.ToggleAttributeModifier("wet", smi => smi.wetMoistureModifier);
		}

		private static bool IsInLiquid(Instance smi, float _) => Grid.IsSubstantialLiquid(Grid.PosToCell(smi), 0.05f);

		public class Def : BaseDef
		{
			public float defaultDryRate = -30f / CONSTS.CYCLE_LENGTH;
			public float defaultSoakRate = 6000f / CONSTS.CYCLE_LENGTH;
		}

		public new class Instance : GameInstance
		{
			public AmountInstance moisture;
			public AttributeModifier baseMoistureModifier;
			public AttributeModifier wetMoistureModifier;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				moisture = BAmounts.Moisture.Lookup(gameObject);
				moisture.value = moisture.GetMax();

				baseMoistureModifier = new AttributeModifier(
					moisture.amount.deltaAttribute.Id,
					def.defaultDryRate,
					STRINGS.CREATURES.MODIFIERS.MOISTURE_LOSS_RATE.NAME);

				wetMoistureModifier = new AttributeModifier(
					moisture.amount.deltaAttribute.Id,
					def.defaultSoakRate,
					STRINGS.CREATURES.MODIFIERS.MOISTURE_GAIN_RATE.NAME);

			}
		}
	}
}
