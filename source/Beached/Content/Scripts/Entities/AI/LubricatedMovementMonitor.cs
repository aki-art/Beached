﻿using Beached.Content.ModDb;
using Klei.AI;

namespace Beached.Content.Scripts.Entities.AI
{
	public class LubricatedMovementMonitor : GameStateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>
	{
		public State idle;
		public State moving;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			idle
				.EnterTransition(moving, smi => smi.GetComponent<Navigator>().IsMoving())
				.EventHandlerTransition(GameHashes.ObjectMovementStateChanged, moving, IsMoving);

			moving
				.ToggleAttributeModifier("DryingOutVeryFast", smi => smi.movementMoistureModifier)
				.EventHandlerTransition(GameHashes.ObjectMovementStateChanged, idle, (smi, data) => !IsMoving(smi, data));
		}

		private bool IsMoving(Instance smi, object data) => data is GameHashes hash && hash == GameHashes.ObjectMovementWakeUp;

		public class Def : BaseDef { }

		public new class Instance : GameInstance
		{
			public AmountInstance moisture;
			public AttributeModifier movementMoistureModifier;

			public float movingDryRate = -500f / CONSTS.CYCLE_LENGTH;


			public Instance(IStateMachineTarget master) : base(master)
			{
				moisture = BAmounts.Moisture.Lookup(gameObject);

				movementMoistureModifier = new AttributeModifier(
					moisture.amount.deltaAttribute.Id,
					movingDryRate,
					STRINGS.CREATURES.MODIFIERS.MOVEMENT_MOISTURE_LOSS.NAME,
					false,
					false,
					true);
			}
		}
	}
}
