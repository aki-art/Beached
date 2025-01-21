using Beached.Content.Scripts.CellQueries;

namespace Beached.Content.Scripts.Entities.AI.Jellyfish
{
	public class PulseMonitor : GameStateMachine<PulseMonitor, PulseMonitor.Instance, IStateMachineTarget, PulseMonitor.Def>
	{
		private State idle;
		private SleepSearchStates searching;

		public IntParameter targetSleepCell = new(Grid.InvalidCell);


		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;
			serializable = SerializeType.ParamsOnly;

			idle
				.Enter((smi =>
				{
					targetSleepCell.Set(Grid.InvalidCell, smi);
					smi.GetComponent<CritterGeneratorSpawner>().DestroyOrphanedConnectorBuilding();
				}))
				.EventTransition(ModHashes.medusaSignal, searching.looking);

			searching
				.Enter(TryRecoverSave)
				.EventTransition(ModHashes.depleted, idle)
				.Exit((smi =>
				{
					targetSleepCell.Set(Grid.InvalidCell, smi);
					smi.GetComponent<CritterGeneratorSpawner>().DestroyOrphanedConnectorBuilding();
				}));

			searching.looking
				.Update(((smi, dt) => FindSleepLocation(smi)), UpdateRate.SIM_1000ms)
				.ToggleStatusItem(Db.Get().CreatureStatusItems.NoSleepSpot)
				.ParamTransition(targetSleepCell, searching.found, (smi, sleepCell) => sleepCell != Grid.InvalidCell);

			searching.found
				.Enter((smi => smi.critter.SpawnConnectorBuilding(targetSleepCell.Get(smi))))
				.ParamTransition(targetSleepCell, searching.looking, (smi, sleepCell) => sleepCell == Grid.InvalidCell)
				.ToggleBehaviour(GameTags.Creatures.WantsConduitConnection, (smi => targetSleepCell.Get(smi) != Grid.InvalidCell && HasPulseSignal(smi)));
		}

		private void FindSleepLocation(Instance smi)
		{
			var query = BPathFinderQueries.jellyFishQuery.Reset(10, smi.navigator);
			smi.navigator.RunQuery(query);

			if (query.results.Count <= 0)
				return;

			// prefer a cell with a wire
			foreach (int resultCell in query.results)
			{
				if (Grid.Objects[resultCell, (int)ObjectLayer.Wire] != null)
				{
					targetSleepCell.Set(resultCell, smi);
					break;
				}
			}

			// otherwise keep previus pick
			if (targetSleepCell.Get(smi) != Grid.InvalidCell)
				return;

			// or pick random
			targetSleepCell.Set(query.results.GetRandom(), smi);
		}

		private void TryRecoverSave(Instance smi)
		{
			if (smi.critter == null)
				return;

			if (targetSleepCell.Get(smi) != Grid.InvalidCell || !smi.critter.IsConnectorBuildingSpawned())
				return;

			targetSleepCell.Set(Grid.PosToCell(smi.critter.GetConnectorBuilding()), smi);
		}

		private bool HasPulseSignal(Instance smi) => smi.Elapsed() < 200;

		private class SleepSearchStates : State
		{
			public State looking;
			public State found;
		}

		public class Def : BaseDef
		{
		}

		public new class Instance(IStateMachineTarget master, PulseMonitor.Def def) : GameInstance(master, def)
		{
			public CritterGeneratorSpawner critter = master.GetComponent<CritterGeneratorSpawner>();
			public Jellyfish jellyFish = master.GetComponent<Jellyfish>();
			public Navigator navigator = master.GetComponent<Navigator>();

			public float Elapsed() => jellyFish.ElapsedSinceLastPulse;
		}

	}
}
