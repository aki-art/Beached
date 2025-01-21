using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	/// Very similar to <see cref="StaterpillarGenerator"/>, but that one is hardcoded to only work for worm bois.
	public class CritterGeneratorBuilding : Generator
	{
		private StatesInstance smi;

		[Serialize] public Ref<KPrefabID> parent;

		public CritterGeneratorBuilding() => parent = new Ref<KPrefabID>();

		public override void OnSpawn()
		{
			var target = parent.Get();

			if (target == null
				|| !target.TryGetComponent(out IPowerGeneratingEntity generator)
				|| generator.GetGenerator() != this)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			smi = new StatesInstance(this);
			smi.StartSM();

			base.OnSpawn();
		}

		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);

			operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);

			if (!operational.IsOperational || WattageRating <= 0.0)
				return;

			GenerateJoules(Mathf.Max(WattageRating * dt, dt));
		}


		public class StatesInstance(CritterGeneratorBuilding master) :
		  GameStateMachine<States, StatesInstance, CritterGeneratorBuilding, object>.GameInstance(master)
		{
			public Operational operational = master.GetComponent<Operational>();
		}

		public class States : GameStateMachine<States, StatesInstance, CritterGeneratorBuilding>
		{
			public State idle;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = root;

				root
					.EventTransition(GameHashes.OperationalChanged, idle, smi => smi.operational.IsOperational);

				idle
					.EventTransition(GameHashes.OperationalChanged, root, smi => !smi.operational.IsOperational)
					.Enter(smi => smi.operational.SetActive(true));
			}
		}
	}
}
