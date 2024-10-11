using System;
using System.Collections.Generic;

namespace Beached.Content.Scripts.Entities
{
	public class Beached_NearbyPlantMonitor :
	  GameStateMachine<Beached_NearbyPlantMonitor, Beached_NearbyPlantMonitor.Instance, IStateMachineTarget>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.Update((smi, dt) => smi.UpdateNearbyCreatures(dt), UpdateRate.SIM_1000ms);
		}

		public new class Instance(IStateMachineTarget master) : GameInstance(master)
		{
			public event Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyPlants;

			public void UpdateNearbyCreatures(float dt)
			{
				var cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(gameObject));

				if (cavityForCell == null)
					return;

				OnUpdateNearbyPlants(dt, cavityForCell.plants, cavityForCell.eggs);
			}
		}
	}
}
