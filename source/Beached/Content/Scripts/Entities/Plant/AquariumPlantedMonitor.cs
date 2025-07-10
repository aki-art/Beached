using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{

	public class AquariumPlantedMonitor : StateMachineComponent<AquariumPlantedMonitor.StatesInstance>, IWiltCause
	{
		[SerializeField] public Storage targetStorage;
		[MyCmpGet] public Storage selfStorage;
		public ElementConsumer[] elementConsumers;

		public string WiltStateString => "Incorrect element";

		public WiltCondition.Condition[] Conditions => [WiltCondition.Condition.DryingOut];

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

		public class StatesInstance(AquariumPlantedMonitor master) : GameStateMachine<States, StatesInstance, AquariumPlantedMonitor, object>.GameInstance(master)
		{
			public Tag[] tags = master.elementConsumers.Select(c => c.elementToConsume.CreateTag()).ToArray();
		}

		public class States : GameStateMachine<States, StatesInstance, AquariumPlantedMonitor>
		{
			public State idle;
			public State dead;
			public PlantedStates planted;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.TagTransition(BTags.aquariumPlanted, planted)
					.EnterTransition(planted, smi => smi.HasTag(BTags.aquariumPlanted));

				planted
					.DefaultState(planted.empty)
					.Enter(smi =>
					{
						ToggleConsumers(smi, true);
						Log.Debug($"entered planted {smi.tags.Join()}");
					})
					.ToggleStatusItem("Self Irrigating", "")
					.UpdateTransition(planted.wrongElement, TransferWater, UpdateRate.SIM_1000ms)
					.EventTransition(GameHashes.Died, dead);

				planted.wrongElement
					.ToggleStatusItem("Wrong Element", "")
					.UpdateTransition(planted.empty, HasCorrectElement, UpdateRate.RENDER_1000ms);

				dead
					.DoNothing();
			}

			private bool HasCorrectElement(StatesInstance smi, float _)
			{
				if (smi.master.targetStorage == null)
				{
					Log.Warning("target storage null");
					return false;
				}

				return smi.master.targetStorage.HasAnyTags(smi.tags);
			}

			private bool TransferWater(StatesInstance smi, float _)
			{
				if (smi.master.selfStorage == null)
				{
					Log.Warning("self storage null");
					return false;
				}

				if (smi.master.targetStorage == null)
				{
					Log.Warning("target storage null");
					return false;
				}

				if (smi.master.selfStorage.IsFull())
					return false;

				var hasCorrectElement = false;

				foreach (var tag in smi.tags)
				{
					var water = smi.master.targetStorage.FindFirst(tag);
					if (water != null)
					{
						smi.master.targetStorage.Transfer(water, smi.master.selfStorage, false, true);
						hasCorrectElement = true;
					}
				}

				return !hasCorrectElement;
			}

			private void ToggleConsumers(StatesInstance smi, bool enabled)
			{
				foreach (var consumer in smi.master.elementConsumers)
					consumer.EnableConsumption(enabled);
			}

			public class PlantedStates : State
			{
				public State empty;
				public State wrongElement;
				public State happy;
			}
		}
	}
}
