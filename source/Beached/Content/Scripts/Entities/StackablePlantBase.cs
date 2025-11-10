using UnityEngine;
using static Beached.Utils.SpawnUtil;

namespace Beached.Content.Scripts.Entities
{
	public class StackablePlantBase : KMonoBehaviour, ISim4000ms
	{
		[SerializeField] public bool isBase;
		[SerializeField] public int maxLength;
		[SerializeField] public Direction growthDirection;
		[SerializeField] public Tag segmentPrefabId;
		[SerializeField] public Tag dropPrefabId;
		[SerializeField] public float lethalHighTemperatureK;
		[SerializeField] public Spawnable loot;

		private int cell;
		private bool canGrow;

		private Direction Opposite => MiscUtil.GetOpposite(growthDirection);

		private int CellAbove() => Grid.GetCellInDirection(cell, growthDirection);
		private int CellBelow() => Grid.GetCellInDirection(cell, Opposite);

		public override void OnSpawn()
		{
			base.OnSpawn();
			cell = Grid.PosToCell(this);

			var root = FindRoot();
			if (root != null)
			{
				var currentCell = cell;
				var length = 0;
				while (TryGetSegment(currentCell, out var currentSegment))
				{
					length++;
					if (length > maxLength)
					{

					}
				}
			}

			Subscribe(ModHashes.stackableChanged, OnStackableChanged);
		}

		private void UpdateNeighbors()
		{
			if (GetSegmentBelow() is StackablePlantBase segmentBelow)
				segmentBelow.Trigger(ModHashes.stackableChanged);

			if (GetSegmentAbove() is StackablePlantBase segmentAbove)
				segmentAbove.Trigger(ModHashes.stackableChanged);
		}

		private void OnStackableChanged(object _)
		{
			if (!CanSurvive(cell))
			{
				Snip();
			}

			var cellAbove = CellAbove();
			canGrow = Grid.IsValidCellInWorld(cellAbove, this.GetMyWorldId())
				&& GetSegmentAbove() == null
				&& CanSurvive(cellAbove);
		}

		private void Snip()
		{
			DropLoot();
			Util.KDestroyGameObject(gameObject);
		}

		private void DropLoot()
		{
			if (!DebugHandler.InstantBuildMode)
				loot.Spawn(cell);
		}

		public bool CanSurvive(int cell)
		{
			var cellBelow = Grid.GetCellInDirection(cell, Opposite);

			if (Grid.IsSolidCell(Grid.PosToCell(this))) return false;

			if (!(Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(cellBelow, out var go) && go.IsPrefabID(segmentPrefabId)))
				return false;


			if (Grid.Temperature[Grid.PosToCell(this)] > lethalHighTemperatureK)
				return false;

			return true;
		}


		private StackablePlantBase FindRoot()
		{
			int currentCell = cell;
			StackablePlantBase result = null;

			for (int y = 0; y < maxLength; y++)
			{
				currentCell = Grid.GetCellInDirection(currentCell, Opposite);

				if (!Grid.IsValidCell(currentCell) || TryGetSegment(currentCell, out result))
					return result;
			}

			return null;
		}

		public StackablePlantBase GetSegmentBelow()
		{
			return TryGetSegment(Grid.GetCellInDirection(cell, growthDirection), out var result)
				? result
				: null;
		}

		public StackablePlantBase GetSegmentAbove()
		{
			return TryGetSegment(Grid.GetCellInDirection(cell, MiscUtil.GetOpposite(growthDirection)), out var result)
				? result
				: null;
		}

		public bool TryGetSegment(int cell, out StackablePlantBase segment)
		{
			segment = null;
			return Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(cell, out var go)
					&& go.IsPrefabID(segmentPrefabId)
					&& go.TryGetComponent(out segment);
		}

		private void Grow()
		{
			var targetCell = Grid.GetCellInDirection(cell, growthDirection);

			if (TryGetSegment(targetCell, out _))
				return;

			MiscUtil.Spawn(segmentPrefabId, Grid.CellToPos(targetCell), Grid.SceneLayer.Creatures);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			OnSegmentDestroyed();
		}

		public void OnSegmentDestroyed()
		{
			if (GetSegmentBelow() is StackablePlantBase segmentBelow)
				segmentBelow.Trigger(ModHashes.stackableSegmentDestroyed);

			if (GetSegmentAbove() is StackablePlantBase segmentAbove)
				segmentAbove.Trigger(ModHashes.stackableSegmentDestroyed);
		}

		public void Sim4000ms(float dt)
		{
			if (canGrow)
			{
				Grow();
			}
		}

		public enum Segment
		{
			Base,
			Middle,
			Tip,
			YoungTip
		}
	}
}
