using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class SmallAquarium : StateMachineComponent<SmallAquarium.SMInstance>
	{
		[SerializeField] public float minimumWaterLevel;

		[MyCmpReq] private PlantablePlot plantablePlot;
		[MyCmpReq] private Storage storage;

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class SMInstance : GameStateMachine<States, SMInstance, SmallAquarium, object>.GameInstance
		{
			public SMInstance(SmallAquarium master) : base(master)
			{
			}

			public bool HasWater()
			{
				var water = master.storage.FindPrimaryElement(SimHashes.Water);
				return water != null && water.Mass > master.minimumWaterLevel;
			}
		}

		public class States : GameStateMachine<States, SMInstance, SmallAquarium>
		{
			public FarmStates empty;
			public FarmStates full;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = empty;

				empty
					.EventTransition(GameHashes.OccupantChanged, full, smi => smi.master.plantablePlot.Occupant != null);

				empty.wet
					.EventTransition(GameHashes.OnStorageChange, empty.dry, smi => !smi.HasWater());

				empty.dry
					.EventTransition(GameHashes.OnStorageChange, empty.wet, smi => !smi.HasWater());

				full
					.EventTransition(GameHashes.OccupantChanged, empty, smi => smi.master.plantablePlot.Occupant == null);

				full.wet
					.EventTransition(GameHashes.OnStorageChange, full.dry, smi => !smi.HasWater());

				full.dry
					.EventTransition(GameHashes.OnStorageChange, full.wet, smi => !smi.HasWater());
			}

			public class FarmStates : State
			{
				public State wet;
				public State dry;
			}
		}
	}
}
