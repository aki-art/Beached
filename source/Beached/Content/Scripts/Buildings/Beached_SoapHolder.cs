using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class Beached_SoapHolder : StateMachineComponent<Beached_SoapHolder.StatesInstance>
	{
		[SerializeField] public Storage soapStorage;
		[SerializeField] public Tag soapTag;
		[SerializeField] public float minimumSoap;

		[MyCmpGet] private KPrefabID prefabID;

		public override void OnSpawn() => smi.StartSM();

		public void OnCompleteShower(WorkerBase worker)
		{
			if (worker == null)
				return;

			if (worker.TryGetComponent(out Effects effects))
				effects.Add(BEffects.NICE_SCENT, true);

			soapStorage.ConsumeIgnoringDisease(soapTag, 10f);
		}

		public class StatesInstance(Beached_SoapHolder master) : GameStateMachine<States, StatesInstance, Beached_SoapHolder, object>.GameInstance(master)
		{
		}

		public class States : GameStateMachine<States, StatesInstance, Beached_SoapHolder>
		{
			public State empty;
			public State stocked;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = empty;

				empty
					.EventHandlerTransition(GameHashes.OnStorageChange, stocked, HasEnoughSoap)
					.EnterTransition(stocked, HasEnoughSoap);

				stocked
					.ToggleTag(BTags.soaped)
					.ToggleStatusItem("Soapy", "");
			}

			private bool HasEnoughSoap(StatesInstance smi)
			{
				return smi.master.soapStorage.GetMassAvailable(smi.master.soapTag) > smi.master.minimumSoap;
			}

			private bool HasEnoughSoap(StatesInstance instance, object _) => HasEnoughSoap(instance);
		}
	}
}
