using System.Collections.Generic;

namespace Beached.Content.Scripts.CellQueries
{
	public class DryLandQuery : PathFinderQuery
	{
		public List<int> results = [];
		private int maxResults;

		public DryLandQuery Reset(int maxResults)
		{
			this.maxResults = maxResults;
			results.Clear();

			return this;
		}

		public override bool IsMatch(int cell, int parentCell, int cost)
		{
			if (!results.Contains(cell) && CheckValidFloorCell(cell))
				results.Add(cell);

			return results.Count >= maxResults;
		}


		private bool CheckValidFloorCell(int testCell)
		{
			if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell))
				return false;

			if (!IsEmptyCell(testCell))
				return false;

			var cellAbove = Grid.GetCellInDirection(testCell, Direction.Up);
			var cellUnder = Grid.GetCellInDirection(testCell, Direction.Down);

			if (Grid.ObjectLayers[1].ContainsKey(testCell)
				|| !Grid.IsValidCell(cellUnder)
				|| !Grid.IsSolidCell(cellUnder)
				|| !Grid.IsValidCell(cellAbove)
				|| Grid.IsSolidCell(cellAbove))
				return false;

			return true;
		}

		private bool IsEmptyCell(int testCell) => Grid.IsGas(testCell) || Grid.Mass[testCell] == 0;
	}
}
