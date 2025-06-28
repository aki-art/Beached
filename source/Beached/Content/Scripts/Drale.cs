using KSerialization;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	// TODO: GameHashes.MissileDamageEncountered
	public class Drale : GameStateMachine<Drale, Drale.Instance, IStateMachineTarget, Drale.Def>
	{
		private State idle;
		private State travelling;
		private State eating;
		private State dying;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			travelling
				.Enter(smi => smi.RefreshDestination())
				.EventTransition(GameHashes.ClusterDestinationReached, eating, null);

			dying
				.TriggerOnEnter(GameHashes.Died, null);
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			[MyCmpGet] private ClusterTraveler traveler;
			[MyCmpGet] private ClusterDestinationSelector destinationSelector;
			[Serialize] private float speed = 1f;
			[Serialize] public AxialI destination = AxialI.ZERO;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				Beached.Log.Debug("assigning cluster traveler getSpeedCB");
				traveler.getSpeedCB = GetSpeed;
				traveler.onTravelCB = OnTravellerMoved;
			}

			private float GetSpeed() => speed;

			private void OnTravellerMoved() => Game.Instance.Trigger((int)GameHashes.ClusterMapMeteorShowerMoved, this);

			private static readonly HashSet<Tag> targetableClusterEntities =
				[
				HarvestablePOIConfig.GasGiantCloud,
				HarvestablePOIConfig.RadioactiveGasCloud,
				HarvestablePOIConfig.HeliumCloud,
				HarvestablePOIConfig.ChlorineCloud,
				HarvestablePOIConfig.OxygenRichAsteroidField
				];

			public void RefreshDestination()
			{
				if (destination != AxialI.ZERO && ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(destination, EntityLayer.POI) != null)
					return;

				foreach (var location in ClusterGrid.Instance.cellContents)
				{
					if (location.Value != null)
					{
						foreach (var item in location.Value)
						{
							if (targetableClusterEntities.Contains(item.PrefabID()))
							{
								destination = item.Location;
								destinationSelector.SetDestination(destination);
								traveler.RevalidatePath(false);
							}
						}
					}
				}
			}
		}
	}
}
