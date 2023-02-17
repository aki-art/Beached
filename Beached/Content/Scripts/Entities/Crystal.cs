using Beached.Utils;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
    public class Crystal : KMonoBehaviour
    {
        [Serialize]
        [SerializeField]
        public Direction growthDirection;

        [Serialize]
        public float angle;

        [MyCmpGet]
        private CrystalDebug debugger;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [SerializeField]
        public List<Direction> validFoundationDirections = new List<Direction>()
        {
            Direction.Down,
            Direction.Right,
            Direction.Left,
            Direction.Up
        };

        private static Vector3 leftOffset = new Vector3(-0.5f, 0.5f);
        private static Vector3 rightOffset = new Vector3(0.5f, 0.5f);

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (growthDirection == Direction.None)
            {
                RandomizeDirection();
            }

            SnapToTerrain();
        }

        private void Rotate(float angle)
        {
            kbac.Rotation = angle;
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
                    angle = GetRandomGrowthAngle(direction);
                    Rotate(angle);

                    if (debugger != null)
                    {
                        debugger.UpdateVisualizers();
                    }

                    return;
                }
            }
        }

        private float GetRandomGrowthAngle(Direction growthDirection)
        {
            float angle;

            switch (growthDirection)
            {
                case Direction.Up:
                    angle = 0f;
                    break;
                case Direction.Left:
                    angle = 90f;
                    break;
                case Direction.Down:
                    angle = 180f;
                    break;
                case Direction.Right:
                default:
                    angle = 270f;
                    break;
            }

            angle += Random.Range(-45f, 45f);
            angle -= 90f;

            return angle;
        }
    }
}
