using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class SmallAquarium : StateMachineComponent<SmallAquarium.SMInstance>
	{
		[SerializeField] public float minimumWaterLevel;

		[MyCmpReq] private PlantablePlot plantablePlot;
		[SerializeField] public Storage storage;

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class SMInstance(SmallAquarium master) : GameStateMachine<States, SMInstance, SmallAquarium, object>.GameInstance(master)
		{
			public KBatchedAnimController kbac = master.GetComponent<KBatchedAnimController>();

			public bool HasWater()
			{
				return master.storage.MassStored() > master.minimumWaterLevel;
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
					.PlayAnim("off")
					.EventTransition(GameHashes.OccupantChanged, full, smi => smi.master.plantablePlot.Occupant != null);

				empty.wet
					.EventTransition(GameHashes.OnStorageChange, empty.dry, smi => !smi.HasWater());

				empty.dry
					.EventTransition(GameHashes.OnStorageChange, empty.wet, smi => !smi.HasWater());

				full
					.PlayAnim("on")
					.ScheduleActionNextFrame("tint liquid", TintLiquid)
					.EventTransition(GameHashes.OccupantChanged, empty, smi => smi.master.plantablePlot.Occupant == null);

				full.wet
					.EventTransition(GameHashes.OnStorageChange, full.dry, smi => !smi.HasWater());

				full.dry
					.EventTransition(GameHashes.OnStorageChange, full.wet, smi => !smi.HasWater());
			}

			private void TintLiquid(SMInstance smi)
			{
				if (smi.master.storage.items == null || smi.master.storage.items.Count == 0)
					return;

				var item = smi.master.storage.items[0];

				if (item.TryGetComponent(out PrimaryElement primary) && primary.Element != null && smi.kbac.layering != null)
				{
					var color = primary.Element.substance.colour with { a = byte.MaxValue };
					smi.kbac.layering.foregroundController.GetComponent<KBatchedAnimController>().SetSymbolTint("liquid_middle_fg", color);
				}
			}

			public class FarmStates : State
			{
				public State wet;
				public State dry;
			}
		}
	}
}
