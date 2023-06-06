using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class FilteringCoral : StateMachineComponent<FilteringCoral.StatesInstance>
	{
		[SerializeField] public Tag gunkTag;
		[SerializeField] public float gunkClearTimeSeconds;
		[SerializeField] public float emitMass;
		[SerializeField] public float gunkLimit;
		[SerializeField] public Vector3 emitOffset = Vector3.zero;
		[SerializeField] public Vector2 initialVelocity;
		[SerializeField] public Storage storage;

		public FilteringCoral()
		{
			gunkTag = GameTags.Solid;
			gunkClearTimeSeconds = 30f;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class States : GameStateMachine<States, StatesInstance, FilteringCoral>
		{
			public State alive;
			public State gunked;
			public State dead;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = alive;

				alive
					.EventHandlerTransition(GameHashes.OnStorageChange, gunked, IsGunked)
					.Enter(smi => ToggleConsumers(smi, true));

				gunked
					// .PlayAnim("full")
					.EventHandlerTransition(GameHashes.OnStorageChange, alive, (smi, data) => !IsGunked(smi, data))
					.ScheduleAction("clear gunk", GetGunkClearTime, ClearGunk) // TODO: do this part smarter
					.Enter(smi => ToggleConsumers(smi, false));

			}

			private void ToggleConsumers(StatesInstance smi, bool enabled)
			{
				foreach (var consumer in smi.elementConsumers)
					consumer.EnableConsumption(enabled);
			}

			private float GetGunkClearTime(StatesInstance smi) => smi.master.gunkClearTimeSeconds;

			private void ClearGunk(StatesInstance smi) => smi.master.storage.Drop(smi.master.gunkTag);

			private bool IsGunked(StatesInstance smi, object data) => smi.master.storage.GetMassAvailable(smi.master.gunkTag) > smi.master.gunkLimit;
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, FilteringCoral, object>.GameInstance
		{
			public readonly ElementConsumer[] elementConsumers;

			public StatesInstance(FilteringCoral master) : base(master)
			{
				elementConsumers = gameObject.GetComponents<ElementConsumer>();
			}
		}
	}
}
