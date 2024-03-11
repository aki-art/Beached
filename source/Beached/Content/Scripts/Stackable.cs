using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Stackable : FMonoBehaviour
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[SerializeField] public Direction directionConstriction;
		[SerializeField] public Tag stackTag;
		[SerializeField] public float lethalHighTemperatureK;
		[SerializeField] public bool allowMerging;
		[SerializeField] public string animFileName;

		[Serialize] private Direction tipDirection;
		[Serialize] private Thickness thickness;

		private int cell;

		private int CellTowards(Direction direction) => Grid.GetCellInDirection(cell, direction);

		public override void OnSpawn()
		{
			base.OnSpawn();

			DebugLog("on spawn");

			cell = Grid.PosToCell(this);
			tipDirection = CalculateTipDirection(directionConstriction == Direction.None ? Direction.Up : directionConstriction);

			DebugLog(tipDirection);
			if (tipDirection == Direction.None)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			SetThickness(CalculateThickness(tipDirection, false));
			UpdateNeighbors();
		}

		private void UpdateNeighbors()
		{
			if (GetSegment(CellTowards(tipDirection)) is Stackable ahead)
				ahead.Trigger(
					ModHashes.stackableChanged,
					new UpdateData(tipDirection, this, true));

			if (GetSegment(CellTowards(tipDirection.Opposite())) is Stackable below)
				below.Trigger(
					ModHashes.stackableChanged,
					new UpdateData(tipDirection.Opposite(), this, false));
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			UpdateNeighbors();
		}

		public void OnNeighborUpdate(object data)
		{
			if (data is UpdateData updated)
			{
				DebugLog("updating neighbor " + updated.direction);
				var thickness = CalculateThickness(tipDirection, allowMerging && updated.stackable.thickness == Thickness.TipMerged);

				SetThickness(thickness);

				if (updated.propagate)
				{
					if (GetSegment(CellTowards(updated.direction)) is Stackable ahead)
						ahead.Trigger(
							ModHashes.stackableChanged,
							new UpdateData(updated.direction, updated.stackable, true));
				}
			}
		}

		private void SetThickness(Thickness thickness)
		{
			this.thickness = thickness;
			kbac.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim($"{animFileName}_{thickness.ToString().ToLowerInvariant()}_kanim")
			});

			kbac.Play("idle");
		}

		private Thickness CalculateThickness(Direction direction, bool tipsMerge)
		{
			var opposite = direction.Opposite();
			var stateAhead = GetSegment(CellTowards(direction));

			if (IsStackableWithDirection(stateAhead, opposite))
			{
				DebugLog("opposite ahead");
				return !tipsMerge && stateAhead.thickness != Thickness.TipMerged
					? Thickness.Tip
					: Thickness.TipMerged;
			}

			if (!IsStackableWithDirection(stateAhead, direction))
			{
				DebugLog("no stackable ahead, this is a tip");
				return Thickness.Tip;
			}

			var thicknessAhead = stateAhead.thickness;
			if (!IsTip(thicknessAhead))
			{
				DebugLog("not a tip ahead, base or middle"); ;
				var blockStateBehind = GetSegment(CellTowards(opposite));
				return !IsStackableWithDirection(blockStateBehind, direction)
						? Thickness.Base
						: Thickness.Middle;
			}

			return Thickness.Frustum;
		}

		private static bool IsTip(Thickness thickness) => thickness == Thickness.Tip || thickness == Thickness.TipMerged;

		private bool IsStackableWithDirection(Stackable stackable, Direction direction)
		{
			return stackable != null && stackable.tipDirection == direction;
		}

		private bool IsValidPlacement(int cell, Direction direction)
		{
			return CanPlace(cell, direction)
					|| IsStackableWithDirection(GetSegment(Grid.GetCellInDirection(cell, direction.Opposite())), direction);
		}

		private Direction CalculateTipDirection(Direction preferredDirection)
		{
			DebugLog($"checking for {preferredDirection}");
			if (IsValidPlacement(cell, preferredDirection))
			{
				DebugLog($"valid placement");
				return preferredDirection;
			}

			if (directionConstriction == Direction.None)
			{
				var opposite = preferredDirection.Opposite();
				if (IsValidPlacement(cell, opposite))
					return opposite;
			}

			return Direction.None;
		}

		private bool CanPlace(int cell, Direction direction)
		{
			if (directionConstriction != Direction.None && direction != directionConstriction)
				return false;

			DebugLog("\t direction ok");
			return CanSurvive(cell, direction);
		}

		public bool CanSurvive(int cell, Direction direction)
		{
			if (!(Grid.IsGas(cell) || Grid.IsLiquid(cell)))
				return false;


			var cellBelow = Grid.GetCellInDirection(cell, direction.Opposite());

			if (Grid.IsSolidCell(cellBelow)) return true;
			DebugLog("\t\t not on solid cell");

			Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(cellBelow, out var go3);
			DebugLog(go3 == null ? "null" : go3.name);

			if (!(Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(cellBelow, out GameObject go) && go.HasTag(stackTag)))
			{
				DebugLog("\t\t NOT on another stackable");
				return false;
			}

			DebugLog("\t\t is on another stackable");

			if (Grid.Temperature[cell] > lethalHighTemperatureK)
				return false;

			return true;
		}

		public Stackable GetSegment(int cell)
		{
			return Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(cell, out var go)
					&& go.HasTag(stackTag)
					&& go.TryGetComponent(out Stackable segment)
				? segment
				: null;
		}

		public enum Thickness
		{
			Base,
			Middle,
			Tip,
			Frustum,
			TipMerged
		}

		private struct UpdateData(Direction direction, Stackable stackable, bool propagate)
		{
			public Direction direction = direction;
			public Stackable stackable = stackable;
			public bool propagate = propagate;
		}
	}
}
