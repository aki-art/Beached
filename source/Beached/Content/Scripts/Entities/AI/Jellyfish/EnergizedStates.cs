namespace Beached.Content.Scripts.Entities.AI.Jellyfish
{

	public class EnergizedStates : GameStateMachine<EnergizedStates, EnergizedStates.Instance, IStateMachineTarget, EnergizedStates.Def>
	{
		public DrowsyStates drowsy;
		public HasConnectorStates connector;
		public State behaviourcomplete;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = connector.moveToSleepLocation;

			root
				.EventTransition(ModHashes.depleted, behaviourcomplete)
				.Exit(CleanUp);

			connector.moveToSleepLocation
				.ToggleStatusItem("moveToSleepLocation", "", category: Db.Get().StatusItemCategories.Main)
				.MoveTo(GetTargetCell, drowsy, behaviourcomplete);

			drowsy
				.ToggleStatusItem("drowsy", "", category: Db.Get().StatusItemCategories.Main)
				.Enter((smi =>
				{
					if (smi.jellyfish.isEnergized)
						smi.GoTo(connector.sleep);
				}))
				.DefaultState(drowsy.loop);

			drowsy.loop
				.PlayAnim("drowsy_pre")
				.QueueAnim("drowsy_loop", true)
				.EventTransition(ModHashes.medusaSignal, drowsy.pst);

			drowsy.pst
				.PlayAnim("drowsy_pst")
				.OnAnimQueueComplete(connector.sleep);


			connector.sleep
				.ToggleStatusItem("producing energy", "", category: Db.Get().StatusItemCategories.Main)
				.Enter(OnSleep);

			connector.sleep.connected
				.Enter((smi => smi.animController.SetSceneLayer(Grid.SceneLayer.SolidConduitBridges)))
				.Exit((smi => smi.animController.SetSceneLayer(Grid.SceneLayer.Creatures)))
				.EventTransition(ModHashes.depleted, connector.connectedWake)
				.Transition(connector.sleep.noConnection, (smi => !smi.jellyfish.IsConnected()))
				.PlayAnim("working_pre")
				.QueueAnim("working_loop", true);

			connector.sleep.noConnection
				.PlayAnim("working_pre") // not connected
				.QueueAnim("working_loop", true) // not connected
				.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected)
				.EventTransition(ModHashes.depleted, connector.noConnectionWake)
				.Transition(connector.sleep.connected, (smi => smi.jellyfish.IsConnected()));

			connector.connectedWake
				.QueueAnim("working_pst")
				.OnAnimQueueComplete(behaviourcomplete);

			connector.noConnectionWake
				.QueueAnim("working_pst") // not connected
				.OnAnimQueueComplete(behaviourcomplete);

			behaviourcomplete
				.BehaviourComplete(GameTags.Creatures.WantsConduitConnection);
		}

		private void OnSleep(Instance smi)
		{
			if (!smi.jellyfish.IsConnectorBuildingSpawned())
			{
				smi.GoTo(behaviourcomplete);
			}
			else
			{
				smi.jellyfish.EnableConnector();

				if (smi.jellyfish.IsConnected())
					smi.GoTo(connector.sleep.connected);
				else
					smi.GoTo(connector.sleep.noConnection);
			}
		}

		private static int GetTargetCell(Instance smi) => smi.pulseMonitor.sm.targetSleepCell.Get(smi.pulseMonitor);

		private static void CleanUp(Instance smi)
		{
			smi.pulseMonitor?.sm.targetSleepCell.Set(Grid.InvalidCell, smi.pulseMonitor);
			smi.jellyfish.DestroyOrphanedConnectorBuilding();
		}

		public class Def : BaseDef
		{
		}

		public new class Instance(IStateMachineTarget master, Def def) : GameInstance(master, def)
		{
			[MyCmpReq] public KBatchedAnimController animController;
			[MyCmpReq] public Jellyfish jellyfish;
			public PulseMonitor.Instance pulseMonitor = master.gameObject.GetSMI<PulseMonitor.Instance>();
		}

		public class SleepStates : State
		{
			public State connected;
			public State noConnection;
		}

		public class DrowsyStates : State
		{
			public State loop;
			public State pst;
		}

		public class HasConnectorStates : State
		{
			public State moveToSleepLocation;
			public SleepStates sleep;
			public State noConnectionWake;
			public State connectedWake;
		}
	}
}
