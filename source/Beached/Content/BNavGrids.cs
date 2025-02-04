namespace Beached.Content
{
	public class BNavGrids
	{
		public const string WALKER_NOJUMP_1X1 = "Beached_WalkerNoJump1X1";

		public static void CreateNavGrids(GameNavGrids navGrids, Pathfinding pathfinding)
		{
			CreateSnailNavigation(navGrids, pathfinding, WALKER_NOJUMP_1X1, [CellOffset.none]);
		}

		private static NavGrid CreateSnailNavigation(GameNavGrids navGrids, Pathfinding pathfinding, string id, CellOffset[] bounds)
		{
			var transitions = navGrids.MirrorTransitions(
			[
				OneSideFloor(),
				OneUpDiagonalFloor(),
				OneDownDiagonalFloor(),
			]);

			var data = new[]
			{
				new NavGrid.NavTypeData()
				{
					navType = NavType.Floor,
					idleAnim = "idle_loop"
				}
			};


			var navGrid = new NavGrid(
				id,
				transitions,
				data,
				bounds,
				[new GameNavGrids.FloorValidator(false)],
				2,
				3,
				transitions.Length);

			pathfinding.AddNavGrid(navGrid);

			return navGrid;
		}

		private static NavGrid.Transition OneSideFloor() => FloorTransition(1, 0, [], true);
		private static NavGrid.Transition OneUpDiagonalFloor() => FloorTransition(1, 1, [new(0, 1)]);
		private static NavGrid.Transition OneDownDiagonalFloor() => FloorTransition(1, -1, [new(1, 0)]);
		private static NavGrid.Transition FloorTransition(int x, int y, CellOffset[] voidOffsets, bool isLooping = false)
		{
			return new NavGrid.Transition(
				NavType.Floor,
				NavType.Floor,
				x,
				y,
				NavAxis.NA,
				isLooping,
				isLooping,
				true,
				1,
				"",
				voidOffsets,
				[],
				[],
				[],
				true,
				0.5f);
		}
	}
}
