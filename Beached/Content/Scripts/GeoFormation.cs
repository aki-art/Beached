using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class GeoFormation : KMonoBehaviour
    {
        [MyCmpReq]
        private KBatchedAnimController kbac;

        [Serialize]
        public EightDirection direction;

        [Serialize]
        public Direction foundation;

        [SerializeField]
        public bool allowDiagonalGrowth;

        private static Vector3 leftOffset = new Vector3(-0.5f, 0.5f);
        private static Vector3 rightOffset = new Vector3(0.5f, 0.5f);

        public void GrowToRandomLength()
        {
            var length = Random.Range(1, 5);
            kbac.Play(length.ToString());
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            GrowToRandomLength();
        }

        public void SetRotation(EightDirection direction)
        {
            var angle = EightDirectionUtil.GetAngle(direction);
            Log.Debug("angle " + angle);

            kbac.Rotation = angle;

            switch (direction)
            {
                case EightDirection.Up:
                    kbac.Offset = Vector3.zero;
                    break;
                case EightDirection.UpLeft:
                    kbac.Offset = foundation == Direction.Down ? Vector3.zero : rightOffset;
                    break;
                case EightDirection.UpRight:
                    kbac.Offset = foundation == Direction.Down ? Vector3.zero : leftOffset;
                    break;
                case EightDirection.Down:
                    kbac.Offset = Vector3.up;
                    break;
                case EightDirection.DownLeft:
                    kbac.Offset = foundation == Direction.Up ? Vector3.up : rightOffset;
                    break;
                case EightDirection.DownRight:
                    kbac.Offset = foundation == Direction.Up ? Vector3.up : leftOffset;
                    break;
                case EightDirection.Right:
                    kbac.Offset = leftOffset;
                    break;
                case EightDirection.Left:
                    kbac.Offset = rightOffset;
                    break;
            }

            this.direction = direction;
        }

        public List<Direction> validFoundationDirections = new List<Direction>()
        {
            Direction.Down,
            Direction.Right,
            Direction.Left,
            Direction.Up
        };

        private EightDirection GetGrowthDirection(Direction foundationDir)
        {
            if(allowDiagonalGrowth)
            {
                return possibleRootings[foundationDir].GetRandom();
            }
            else
            {
                switch (foundationDir)
                {
                    case Direction.Up:
                        return EightDirection.Down;
                    case Direction.Right:
                        return EightDirection.Left;
                    case Direction.Down:
                        return EightDirection.Up;
                    case Direction.Left:
                    default:
                        return EightDirection.Right;
                }
            }
        }

        private static Dictionary<Direction, EightDirection[]> possibleRootings = new Dictionary<Direction, EightDirection[]>()
        {
            {
                Direction.Up, new[]
                {
                    EightDirection.DownLeft,
                    EightDirection.Down,
                    EightDirection.DownRight
                }
            },
            {
                Direction.Down, new[]
                {
                    EightDirection.UpLeft,
                    EightDirection.Up,
                    EightDirection.UpRight
                }
            },
            {
                Direction.Left, new[]
                {
                    EightDirection.DownRight,
                    EightDirection.Right,
                    EightDirection.UpRight
                }
            },
            {
                Direction.Right, new[]
                {
                    EightDirection.DownLeft,
                    EightDirection.Left,
                    EightDirection.UpLeft
                }
            },
        };

        public void RotateRandomly()
        {
            validFoundationDirections.Shuffle();
            var originalCell = Grid.PosToCell(this);

            foreach (var direction in validFoundationDirections)
            {
                var cell = Grid.GetCellInDirection(originalCell, direction);
                if(Grid.IsSolidCell(cell))
                {
                    foundation = direction;
                    SetRotation(GetGrowthDirection(direction));
                    return;
                }
            }
        }
    }
}
