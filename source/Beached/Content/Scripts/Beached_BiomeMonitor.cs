using static ProcGen.SubWorld;

namespace Beached.Content.Scripts
{
	public class Beached_BiomeMonitor : GameStateMachine<Beached_BiomeMonitor, Beached_BiomeMonitor.Instance, IStateMachineTarget>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.Update(OnUpdate, UpdateRate.SIM_200ms);
		}

		private void OnUpdate(Instance smi, float dt)
		{
			var cell = Grid.PosToCell(smi);
			var zoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(cell);

			if (zoneType != smi.previousZoneType)
			{
				smi.master.Trigger(ModHashes.enteredZoneType, zoneType);
				smi.previousZoneType = zoneType;
			}
		}

		public new class Instance(IStateMachineTarget master) : GameInstance(master)
		{
			public ZoneType previousZoneType = (ZoneType)(-1);
		}
	}
}
