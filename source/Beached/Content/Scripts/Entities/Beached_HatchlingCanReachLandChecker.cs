using Beached.Content.Scripts.CellQueries;
using System.Collections;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Beached_HatchlingCanReachLandChecker : KMonoBehaviour
	{
		public bool checkForFallenLocation;

		[SerializeField] public Tag removedEggChance;

		[MyCmpReq] private Navigator navigator;
		[MySmiReq] private FertilityMonitor.Instance fertilityMonitor;

		private NavGrid navGrid;
		private NavType navType;

		public override void OnSpawn()
		{
			base.OnSpawn();
			StartCoroutine(NextFrame());
		}

		private IEnumerator NextFrame()
		{
			yield return SequenceUtil.waitForEndOfFrame;
			Subscribe(DetailsScreen.Instance.gameObject, (int)GameHashes.UIRefreshData, OnUIRefresh);
		}

		private void OnUIRefresh(object _) => UpdateChances();

		public override void OnCleanUp()
		{
			if (Game.IsQuitting() || DetailsScreen.Instance.IsNullOrDestroyed())
				return;

			Unsubscribe(DetailsScreen.Instance.gameObject, (int)GameHashes.UIRefreshData, OnUIRefresh);
			base.OnCleanUp();
		}

		public void SetNavigationType(string navGrid, NavType navType)
		{
			this.navGrid = Pathfinding.Instance.GetNavGrid(navGrid);
			this.navType = navType;
		}

		public bool CanReachDryLand()
		{
			var cellQuery = BPathFinderQueries.dryLandQuery.Reset(1);
			navigator.RunQuery(cellQuery);

			var flags = PathFinder.PotentialPath.Flags.None;

			var cell = Grid.PosToCell(this);

			// find floor cell under where egg will drop
			if (checkForFallenLocation)
			{
				while (!Grid.IsSolidCell(cell))
					cell = Grid.CellBelow(cell);

				cell = Grid.CellAbove(cell);
			}

			PathFinder.Run(
				navGrid,
				navigator.GetCurrentAbilities(),
				new PathFinder.PotentialPath(cell, navType, flags),
				cellQuery);

			return BPathFinderQueries.dryLandQuery.results.Count > 0;
		}

		public void UpdateChances()
		{
			if (!CanReachDryLand())
			{
				fertilityMonitor.breedingChances[0].weight = 100f;

				for (int i = 1; i < fertilityMonitor.breedingChances.Count; i++)
					fertilityMonitor.breedingChances[i].weight = 0;
			}
		}
	}
}
