using System.Collections.Generic;

namespace Beached.Content.Scripts.CellQueries
{
	public class JellyfishQuery : PathFinderQuery
	{
		public List<int> results = [];
		private Navigator navigator;
		private int maxResults;
		private int avoidConnector;

		public JellyfishQuery Reset(int maxResults, Navigator navigator)
		{
			this.navigator = navigator;
			this.maxResults = maxResults;
			avoidConnector = (int)ObjectLayer.WireConnectors;
			results.Clear();

			return this;
		}

		public override bool IsMatch(int cell, int parentCell, int cost)
		{
			if (!results.Contains(cell) && CheckValidCell(cell))
				results.Add(cell);

			return results.Count >= maxResults;
		}

		private bool CheckValidCell(int testCell)
		{
			if (!navigator.NavGrid.NavTable.IsValid(testCell, NavType.Swim))
				return false;

			return Grid.IsValidCell(testCell)
				&& Grid.IsValidBuildingCell(testCell)
				&& Grid.IsLiquid(testCell)
				&& !Grid.ObjectLayers[(int)ObjectLayer.Building].ContainsKey(testCell)
				&& Grid.Objects[testCell, avoidConnector];
		}
	}
}
