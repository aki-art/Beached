using ImGuiNET;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Content.Scripts.Entities.Plant
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GnawicaCore : MultiPartPlantCore, IImguiDebug
	{
		[SerializeField] public Tag nestPrefabId, mawPrefabId, gardenPrefabId, stalkPrefabId;
		[SerializeField] public int maxStalkCount;
		[SerializeField] public int maxWalkers, minWalkerSteps, maxWalkerSteps;
		[SerializeField] public float windyness;
		[SerializeField] public float allowAdjacentSpawnChance;

		[Serialize] private List<Walker> _walkers;

		private HashSet<int> halo;

		public override void OnSpawn()
		{
			_walkers ??= [];
			halo = [];

			foreach (var connected in runtimePlantPieces.Keys)
			{
				foreach (var offset in ModAssets.cardinals)
					halo.Add(Grid.GetCellInDirection(connected, offset));
			}
		}

		[Serializable]
		private class Walker
		{
			public int cell;
			public Direction looking;
			public int steps;
			public int maxSteps;

			[NonSerialized]
			public Text debugOverlay;
		}

		private bool IsHaloOverlapping(int cell)
		{
			return (!halo.Contains(cell) || UnityEngine.Random.value < allowAdjacentSpawnChance);
		}

		public override bool CanGrowIntoCell(int cell)
		{
			return base.CanGrowIntoCell(cell) && !Beached_Grid.hasClimbable[cell];
		}

		public override bool TryGrow()
		{
			if (runtimePlantPieces.Count >= maxStalkCount)
				return false;

			if (_walkers.Count == 0 || (_walkers.Count < maxWalkers && runtimePlantPieces.Count > 3 && (UnityEngine.Random.value < 0.5f)))
			{
				var cell = -1;
				var direction = Direction.Up;

				if (runtimePlantPieces.Count == 0)
				{
					cell = Grid.PosToCell(this);
				}
				else
				{
					var eligibleCells = runtimePlantPieces
						.Keys
						.Skip(1)
						.Where(piece => !_walkers.Any(w => w.cell == piece));

					if (eligibleCells.Count() > 0)
					{
						cell = eligibleCells.GetRandom();
						direction = FindGrowthDirection(cell);
					}
				}

				if (direction != Direction.None)
				{
					_walkers.Add(new Walker()
					{
						cell = cell,
						steps = 0,
						looking = direction,
						maxSteps = UnityEngine.Random.Range(minWalkerSteps, maxWalkerSteps)
					});
				}
			}

			if (_walkers.Count > 0)
			{
				var walker = _walkers.GetRandom();
				var nextCell = Grid.GetCellInDirection(walker.cell, walker.looking);

				if (UnityEngine.Random.value < windyness)
					TryTurn(walker);

				var canGrowIntoCell = CanGrowIntoCell(nextCell) && (walker.steps == 0 || !IsHaloOverlapping(nextCell));

				if (canGrowIntoCell || TryTurn(walker))
				{
					SpawnStalk(nextCell, walker.cell);
					walker.cell = nextCell;
					walker.steps++;

					if (walker.debugOverlay != null)
						walker.debugOverlay.transform.position = Grid.CellToPosCCC(nextCell, Grid.SceneLayer.FXFront2);

					if (walker.steps >= walker.maxSteps)
						_walkers.Remove(walker);

					return true;
				}

				_walkers.Remove(walker);
			}

			return false;
		}

		private bool TryTurn(Walker walker)
		{

			var looking = UnityEngine.Random.value > 0.5f ? walker.looking.RotateClockWise() : walker.looking.RotateCounterClockWise();

			var nextCell = Grid.GetCellInDirection(walker.cell, looking);

			if (CanGrowIntoCell(nextCell))
			{
				walker.looking = looking;
				return true;
			}

			looking = looking.Opposite();

			nextCell = Grid.GetCellInDirection(walker.cell, looking);

			if (CanGrowIntoCell(nextCell))
			{
				walker.looking = looking;
				return true;
			}

			return false;
		}

		private Direction FindGrowthDirection(int cell)
		{
			foreach (var direction in ModAssets.cardinals)
			{
				var targetCell = Grid.GetCellInDirection(cell, direction);
				if (CanGrowIntoCell(targetCell))
					return direction;
			}

			return Direction.None;
		}

		private void SpawnStalk(int cell, int towardsRoot)
		{
			var stalk = FUtility.Utils.Spawn(stalkPrefabId, Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingBack));
			if (stalk.TryGetComponent(out MultiPartPlantPiece piece))
				AttachPart(piece, towardsRoot);

			foreach (var offset in ModAssets.cardinals)
			{
				halo.Add(Grid.GetCellInDirection(cell, offset));
			}
		}

		private void GrowToSize(int newSize)
		{
			while (Size() < newSize)
			{
				if (!TryGrow())
					break;
			}
		}

		private static bool _showDebugTexts;

		public void OnImguiDraw()
		{
			ImGui.Text("Piece count: " + runtimePlantPieces.Count);
			if (ImGui.Button("Grow to max size"))
				GrowToSize(maxStalkCount);

			if (ImGui.Button("Grow once"))
				GrowToSize(Size() + 1);

			if (ImGui.Checkbox("Show Line Renderers", ref _showDebugTexts))
			{
				if (_showDebugTexts)
				{
					for (var i = 0; i < _walkers.Count; i++)
					{
						var walker = _walkers[i];
						var text = ModDebug.AddText(Grid.CellToPosCCC(walker.cell, Grid.SceneLayer.FXFront2), Color.green, MiscUtil.DirectionToString(walker.looking));

						walker.debugOverlay = text;
					}
				}
				else
				{
					for (var i = 0; i < _walkers.Count; i++)
					{
						var walker = _walkers[i];

						if (walker.debugOverlay != null)
							Destroy(walker.debugOverlay);
					}
				}
			}
		}
	}
}
