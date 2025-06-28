using ImGuiNET;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Crystal : KMonoBehaviour, IImguiDebug
	{
		[Serialize][SerializeField] public Direction growthDirection;

		[Serialize] public float angleDeg;
		[SerializeField] public float initialAngleTreshold;

		[MyCmpGet] private CrystalDebug debugger;
		[MyCmpReq] private KBatchedAnimController kbac;

		[SerializeField]
		public List<Direction> validFoundationDirections =
		[
			Direction.Down,
			Direction.Right,
			Direction.Left,
			Direction.Up
		];

		private static Vector3 leftOffset = new(-0.5f, 0.5f);
		private static Vector3 rightOffset = new(0.5f, 0.5f);

		public Crystal()
		{
			initialAngleTreshold = 45f;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (growthDirection == Direction.None)
			{
				RandomizeDirection();
			}

			SnapToTerrain();
			SetRotation(angleDeg);
		}

		private void SetRotation(float angle)
		{
			var oppositeAngle = angle + 180f;
			if (angle >= 360f)
				angle -= 360f;

			kbac.Rotation = oppositeAngle;
			Trigger(ModHashes.crystalRotated, angle);
		}

		private void SnapToTerrain()
		{
			switch (growthDirection)
			{
				case Direction.Up:
					kbac.Offset = Vector3.zero;
					break;
				case Direction.Right:
					kbac.Offset = leftOffset;
					break;
				case Direction.Down:
					kbac.Offset = Vector3.up;
					break;
				case Direction.Left:
					kbac.Offset = rightOffset;
					break;
			}
		}

		private void RandomizeDirection()
		{
			var originalCell = Grid.PosToCell(this);

			foreach (var direction in validFoundationDirections)
			{
				var cell = Grid.GetCellInDirection(originalCell, direction);
				if (Grid.IsSolidCell(cell))
				{
					growthDirection = MiscUtil.GetOpposite(direction);
					angleDeg = GetRandomGrowthAngle(direction);
					SetRotation(angleDeg);

					if (debugger != null)
						debugger.UpdateVisualizers();

					return;
				}
			}
		}

		private float GetRandomGrowthAngle(Direction growthDirection)
		{
			var angle = growthDirection switch
			{
				Direction.Up => 0f,
				Direction.Left => 90f,
				Direction.Down => 180f,
				_ => 270f,
			};

			angle += Random.Range(-initialAngleTreshold, initialAngleTreshold);

			return angle;
		}

		public void OnImguiDraw()
		{
			if (ImGui.SliderFloat("rotation", ref angleDeg, 0, 360))
			{
				SetRotation(angleDeg);
			}
		}
	}
}
