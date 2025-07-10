using UnityEngine;

namespace Beached.Content
{
	public class BNavGrids
	{
		public const string
			WALKER_NOJUMP_1X1 = "Beached_WalkerNoJump1X1",
			FUAFUA = "Beached_FuaFua";


		public static void CreateNavGrids(GameNavGrids navGrids, Pathfinding pathfinding)
		{
			CreateSnailNavigation(navGrids, pathfinding, WALKER_NOJUMP_1X1, [CellOffset.none]);
			CreateFuaFuaNavigation(navGrids, pathfinding, FUAFUA, [CellOffset.none]);
		}

		// TransitionDriver.BeginTransition

		private static void CreateFuaFuaNavigation(GameNavGrids navGrids, Pathfinding pathfinding, string id, CellOffset[] bounds)
		{
			var transitions = new NavGrid.Transition[]
			{
				// basic floor movement
				OneSideFloor(),
				OneUpDiagonalFloor(),
				OneDownDiagonalFloor(),

				// walls, drecko baby like
				UpRightWall(),
				DownLeftWall(),
				FloorToLeftWall(),
				FloorToRightWall(),
				CeilingToLeftWall(),
				CeilingToRightWall(),
				LeftWallToFloor(),
				RightWallToFloor(),
				CeilingSide(),
				RightWallToCeiling(),
				LeftWallToCeiling(),
				// ceiling to floor
				// left wall to right wall

				// stalk
				FloorToStalk(),
				FloorToStalkUp(),
				StalkTransition(0, 1, true),
				StalkTransition(0, -1, true),
				StalkTransition(1, 0, true)
			};

			var data = new NavGrid.NavTypeData[]
			{
				new()
				{
					navType = NavType.Floor,
					idleAnim = "idle_loop"
				},
				new()
				{
					navType = NavType.RightWall,
					idleAnim = "idle_loop",
					animControllerOffset = new Vector2(0.5f, -0.5f),
					rotation = Mathf.Deg2Rad * -90f
				},
				new()
				{
					navType = NavType.Ceiling,
					idleAnim = "idle_loop",
					animControllerOffset = new Vector2(0.0f, -1f),
					rotation = Mathf.Deg2Rad * -180f
				},
				new()
				{
					navType = NavType.LeftWall,
					idleAnim = "idle_loop",
					animControllerOffset = new Vector2(-0.5f, -0.5f),
					rotation = Mathf.Deg2Rad * -270f
				},
				new()
				{
					navType = NavType.Ladder,
					idleAnim = "climb_loop",
					animControllerOffset = new Vector2(0f, +0.5f),
				}
			};

			transitions = navGrids.MirrorTransitions(transitions);

			var navGrid = new NavGrid(
				id,
				transitions,
				data,
				bounds,
				[
					new GameNavGrids.FloorValidator(false),
					new GameNavGrids.WallValidator(),
					new ClimbableValidator(),
					new GameNavGrids.CeilingValidator()],
				2,
				3,
				16);

			pathfinding.AddNavGrid(navGrid);
		}

		private static NavGrid.Transition StalkTransition(int x, int y, bool looping)
		{
			return new NavGrid.Transition(
				NavType.Ladder,
				NavType.Ladder,
				x,
				y,
				NavAxis.NA,
				looping,
				false,
				true,
				1,
				"climb_loop",
				[],
				[],
				[],
				[]);
		}

		private static NavGrid.Transition FloorToStalkUp()
		{
			return new NavGrid.Transition(
				NavType.Floor,
				NavType.Ladder,
				0,
				1,
				NavAxis.NA,
				false,
				false,
				true,
				1,
				"climb_loop",
				[],
				[],
				[],
				[]);
		}

		private static NavGrid.Transition FloorToStalk()
		{
			return new NavGrid.Transition(
				NavType.Floor,
				NavType.Ladder,
				0,
				0,
				NavAxis.NA,
				false,
				false,
				true,
				1,
				"climb_loop",
				[],
				[],
				[],
				[]);
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

		private static NavGrid.Transition LeftWallToFloor() => WallToFloorTransition(0, 0, NavType.LeftWall, []);

		private static NavGrid.Transition RightWallToFloor() => WallToFloorTransition(1, 1, NavType.RightWall, [new CellOffset(0, 1)]);

		private static NavGrid.Transition CeilingToLeftWall() => CeilingToWallTransition(0, 0, NavType.LeftWall);

		private static NavGrid.Transition CeilingToRightWall() => CeilingToWallTransition(-1, 1, NavType.RightWall);

		private static NavGrid.Transition FloorToLeftWall() => FloorToWallTransition(1, -1, NavType.LeftWall);

		private static NavGrid.Transition FloorToRightWall() => FloorToWallTransition(0, 0, NavType.RightWall);

		private static NavGrid.Transition UpRightWall() => WallTransition(0, 1, NavType.RightWall, [], true);

		private static NavGrid.Transition DownLeftWall() => WallTransition(0, -1, NavType.LeftWall, [], true);

		private static NavGrid.Transition OneSideFloor() => FloorTransition(1, 0, [], true);

		private static NavGrid.Transition OneUpDiagonalFloor() => FloorTransition(1, 1, [new(0, 1)]);

		private static NavGrid.Transition OneDownDiagonalFloor() => FloorTransition(1, -1, [new(1, 0)]);

		private static NavGrid.Transition RightWallToCeiling() => WallToCeiling(0, 0, NavType.RightWall, []);

		private static NavGrid.Transition LeftWallToCeiling() => WallToCeiling(-1, -1, NavType.LeftWall, [new(1, 0)]);

		private static NavGrid.Transition WallToCeiling(int x, int y, NavType direction, CellOffset[] voidOffsets)
		{
			return new NavGrid.Transition(
				direction,
				NavType.Ceiling,
				x,
				y,
				NavAxis.NA,
				false,
				false,
				true,
				1,
				"floor_wall_0_0",
				voidOffsets,
				[],
				[],
				[]);
		}

		private static NavGrid.Transition CeilingToWallTransition(int x, int y, NavType direction)
		{
			return new NavGrid.Transition(
				NavType.Ceiling,
				direction,
				x,
				y,
				NavAxis.NA,
				false,
				false,
				true,
				1,
				"floor_wall_0_0",
				[],
				[],
				[],
				[]);
		}

		private static NavGrid.Transition CeilingSide(bool looping = true)
		{
			return new NavGrid.Transition(
				NavType.Ceiling,
				NavType.Ceiling,
				-1,
				0,
				NavAxis.NA,
				looping,
				true,
				true,
				1,
				"floor_floor_1_0",
				[],
				[],
				[],
				[]);
		}

		private static NavGrid.Transition WallToFloorTransition(int x, int y, NavType direction, CellOffset[] voidOffsets)
		{
			return new NavGrid.Transition(
				direction,
				NavType.Floor,
				x,
				y,
				NavAxis.NA,
				false,
				false,
				true,
				1,
				"floor_wall_0_0",
				voidOffsets,
				[],
				[],
				[]);
		}

		private static NavGrid.Transition FloorToWallTransition(int x, int y, NavType direction, bool isLooping = false)
		{
			return new NavGrid.Transition(
				NavType.Floor,
				direction,
				x,
				y,
				NavAxis.NA,
				false,
				false,
				true,
				1,
				"floor_wall_0_0",
				[],
				[],
				[],
				[]);
		}

		private static NavGrid.Transition WallTransition(int x, int y, NavType direction, CellOffset[] voidOffsets, bool isLooping = false)
		{
			return new NavGrid.Transition(
				direction,
				direction,
				x,
				y,
				NavAxis.NA,
				true,
				true,
				true,
				1,
				"floor_floor_1_0",
				voidOffsets,
				[],
				[],
				[]);
		}

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
