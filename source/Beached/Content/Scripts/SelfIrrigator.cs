using UnityEngine;

namespace Beached.Content.Scripts
{
	/// <see cref="Patches.IrrigationMonitorPatch.IrrigationMonitor_InitializeStates_Patch"/>
	public class SelfIrrigator : StateMachineComponent<SelfIrrigator.StatesInstance>
	{
		[SerializeField] public Storage targetStorage;
		[MyCmpGet] public Storage selfStorage;
		public ElementConsumer[] elementConsumers;

		public override void OnSpawn()
		{
			elementConsumers = gameObject.GetComponents<ElementConsumer>();
			smi.StartSM();
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

		public class StatesInstance : GameStateMachine<States, StatesInstance, SelfIrrigator, object>.GameInstance
		{
			public Storage storage;
			public IrrigationMonitor.Instance irrigationMonitor;
			public Tag targetTag;

			public StatesInstance(SelfIrrigator master) : base(master)
			{
				irrigationMonitor = gameObject.GetSMI<IrrigationMonitor.Instance>();

				var def = gameObject.GetDef<IrrigationMonitor.Def>();

				targetTag = SimHashes.Water.CreateTag();

				foreach (var elements in def.consumedElements)
				{
					var element = ElementLoader.GetElement(elements.tag);
					if (element.IsLiquid)
					{
						targetTag = elements.tag;
						break;
					}
				}
			}
		}

		public class States : GameStateMachine<States, StatesInstance, SelfIrrigator>
		{
			public State idle;
			public State irrigated;
			public State dead;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.EventHandlerTransition(GameHashes.ReceptacleMonitorChange, irrigated, (smi, dt) => IsPlanted(smi))
					.EnterTransition(irrigated, IsPlanted)
					.Enter(smi => ToggleConsumers(smi, false));

				irrigated
					.Enter(smi =>
					{
						ToggleConsumers(smi, true);
						smi.irrigationMonitor.UpdateIrrigation(1f / 30f);
					})
					.ToggleStatusItem("Self Irrigating", "")
					.Update(TransferWater, UpdateRate.SIM_1000ms)
					.EventTransition(GameHashes.Died, dead);

				dead
					.DoNothing();
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

			private bool IsPlanted(StatesInstance smi)
			{
				return smi.gameObject.TryGetComponent(out ReceptacleMonitor receptacleMonitor) && receptacleMonitor.Replanted;
			}

			private void ToggleConsumers(StatesInstance smi, bool enabled)
			{
				foreach (var consumer in smi.master.elementConsumers)
					consumer.EnableConsumption(enabled);
			}
		}
	}
}
