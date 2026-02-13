using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class FilterElementConsumer : StateMachineComponent<FilterElementConsumer.StatesInstance>
	{
		[SerializeField] public Storage targetStorage;
		[SerializeField] public Tag targetTag;
		[MyCmpGet] public Storage selfStorage;
		public ElementConsumer[] elementConsumers;

		[Serialize] public bool consumptionEnabled;

		public override void OnSpawn()
		{
			elementConsumers = gameObject.GetComponents<ElementConsumer>();
			smi.StartSM();

			Toggle(consumptionEnabled);
		}

		public void Toggle(bool on)
		{
			smi.GoTo(on ? smi.sm.consuming : smi.sm.idle);
			consumptionEnabled = on;
		}

		public void SetStorage(Storage storage)
		{
			targetStorage = storage;
			elementConsumers ??= gameObject.GetComponents<ElementConsumer>();

			foreach (var consumer in elementConsumers)
			{
				consumer.storage = storage;
				consumer.RefreshConsumptionRate();
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, FilterElementConsumer, object>.GameInstance
		{
			public Storage storage;
			public Tag targetTag;

			public StatesInstance(FilterElementConsumer master) : base(master)
			{
				targetTag = master.targetTag;
			}
		}

		public class States : GameStateMachine<States, StatesInstance, FilterElementConsumer>
		{
			public State idle;
			public State consuming;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.EnterTransition(consuming, IsEnabled)
					.Enter(smi => ToggleConsumers(smi, false));

				consuming
					.Enter(smi =>
					{
						ToggleConsumers(smi, true);
					})
					.ToggleStatusItem("Self Irrigating", "")
					.Update(TransferWater, UpdateRate.SIM_1000ms);
			}

			private void TransferWater(StatesInstance smi, float _)
			{
				if (smi.master.selfStorage == null)
				{
					Log.Warning("self storage null");
					return;
				}

				if (smi.master.targetStorage == null)
				{
					// Log.Warning("target storage null");
					return;
				}

				var water = smi.master.selfStorage.FindFirst(smi.targetTag);
				if (water != null)
					smi.master.selfStorage.Transfer(water, smi.master.targetStorage, false, true);
			}

			private bool IsEnabled(StatesInstance smi)
			{
				return smi.master.consumptionEnabled;
			}

			private void ToggleConsumers(StatesInstance smi, bool enabled)
			{
				foreach (var consumer in smi.master.elementConsumers)
					consumer.EnableConsumption(enabled);
			}
		}
	}
}
