using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI.Fua
{

	public class FuaNest : StateMachineComponent<FuaNest.StatesInstance>
	{
		[SerializeField] public float furPerCycle;

		public override void OnSpawn()
		{
			smi.StartSM();
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, FuaNest, object>.GameInstance
		{
			public Storage storage;
			public PrimaryElement primaryElement;
			public float furPerSecond;
			public Element element;

			public StatesInstance(FuaNest master) : base(master)
			{
				storage = master.GetComponent<Storage>();
				primaryElement = master.GetComponent<PrimaryElement>();
				furPerSecond = master.furPerCycle / CONSTS.CYCLE_LENGTH;
				element = ElementLoader.FindElementByHash(Elements.fuzz);
			}
		}

		public class States : GameStateMachine<States, StatesInstance, FuaNest>
		{
			public override void InitializeStates(out BaseState default_state)
			{
				default_state = root;

				root
					.ToggleStatusItem("Producing Fuzz", "This nest is accumulting fuzz, via magical invisible unimplemented critters.")
					.Update(AccumulateFur);
			}

			private void AccumulateFur(StatesInstance smi, float dt)
			{
				if (smi.storage.IsFull())
				{
					smi.storage.DropAll();
				};

				var material = smi.element.substance.SpawnResource(
					smi.transform.position,
					smi.furPerSecond * dt,
					smi.primaryElement.Temperature,
					byte.MaxValue,
					0,
					true);

				smi.storage.Store(material, true);

			}
		}
	}
}
