using Beached.Content.Defs.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Beached.Content.Scripts.Entities.AI.Jellyfish
{
	public class StrobilaLayingStates : GameStateMachine<StrobilaLayingStates, StrobilaLayingStates.Instance, IStateMachineTarget, StrobilaLayingStates.Def>
	{
		public State findBuildLocation;
		public State moveToBuildLocation;
		public State doBuild;
		public State behaviourcomplete;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = findBuildLocation;

			findBuildLocation
				.Enter((smi =>
				{
					FindBuildLocation(smi);
					smi.GoTo(smi.targetBuildCell != Grid.InvalidCell
						? moveToBuildLocation
						: behaviourcomplete);
				}));

			moveToBuildLocation
				.MoveTo(smi => smi.targetBuildCell, doBuild, behaviourcomplete);

			doBuild
				.PlayAnim("fall_pst")
				.TriggerOnExit(ModHashes.builtNest)
				.EventHandlerTransition(GameHashes.AnimQueueComplete, behaviourcomplete, TryBuildHome);

			behaviourcomplete
				.BehaviourComplete(GameTags.Creatures.WantsToMakeHome);
		}

		private bool TryBuildHome(Instance smi, object _)
		{
			smi.builtHome = true;
			smi.BuildHome();

			return true;
		}

		private void FindBuildLocation(Instance smi)
		{
			smi.targetBuildCell = Grid.InvalidCell;

			var prefab = Assets.GetPrefab(JellyfishStrobilaConfig.ID);
			var query = PathFinderQueries.buildingPlacementQuery.Reset(1, prefab);
			smi.GetComponent<Navigator>().RunQuery(query);

			if (query.result_cells.Count <= 0)
				return;

			smi.targetBuildCell = query.result_cells[Random.Range(0, query.result_cells.Count)];
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public int targetBuildCell;
			public bool builtHome;

			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToMakeHome);
			}

			public void BuildHome()
			{
				var pos = Grid.CellToPos(targetBuildCell, CellAlignment.Bottom, Grid.SceneLayer.Creatures);
				var go = Util.KInstantiate(Assets.GetPrefab(JellyfishStrobilaConfig.ID), pos, Quaternion.identity);
				var primaryElement = go.GetComponent<PrimaryElement>();
				primaryElement.ElementID = SimHashes.Creature;
				primaryElement.Temperature = gameObject.GetComponent<PrimaryElement>().Temperature;

				go.SetActive(true);
				//go.GetSMI<BeeHive.StatesInstance>().SetUpNewHive();
			}
		}
	}
}
