using Beached.Utils;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class GeoFormation : KMonoBehaviour
    {
        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpGet]
        private CrystalFoundationMonitor foundationMonitor;

        [Serialize]
        public float rotation;

        [Serialize]
        public Direction foundation;

        [Serialize]
        public bool rotationSet;

        [SerializeField]
        public bool allowDiagonalGrowth;

        [SerializeField]
        public bool drawDebugLines = true;

        private LineRenderer transformLineRenderer;
        private LineRenderer checkLineRenderer;

        private static Vector3 leftOffset = new Vector3(-0.5f, 0.5f);
        private static Vector3 rightOffset = new Vector3(0.5f, 0.5f);

        private Vector3 originOffset;

        public void GrowToRandomLength()
        {
            startPoint = transform.position + new Vector3(0, 0.5f);

            var length = 4f; //
            Random.Range(1, 5);

            while(length > 0)
            {
                if(TestLength(length))
                {
                    kbac.Play(length.ToString());
                    break;
                }

                length--;
            }
        }

        public static Vector3 RotateRadians(Vector3 vector, float radians)
        {
            var cos = Mathf.Cos(radians);
            var sin = Mathf.Sin(radians);
            return new Vector3(cos * vector.x - sin * vector.y, sin * vector.x + cos * vector.y);
        }

        private Vector3 startPoint;

        public static Vector3 RadianToVector2(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        private bool TestLength(float length)
        {
            length -= 0.6f;

            var rad = Mathf.Deg2Rad * rotation;

            var testX = Mathf.FloorToInt(startPoint.x + length * Mathf.Cos(rad));
            var testY = Mathf.FloorToInt(startPoint.y + length * Mathf.Sin(rad));


            var cell = Grid.XYToCell(testX, testY);

            if (!Grid.Solid[cell])
            {
                if (Grid.TestLineOfSight((int)(transform.position.x), (int)(transform.position.y), testX, testY, Grid.IsSolidCell))
                {
                    DrawDebugLines(length, rad, testX, testY);
                    return true;
                }
            }

            return false;
        }

        private void DrawDebugLines(float length, float rad, int testX, int testY)
        {
            if (drawDebugLines)
            {
                if (transformLineRenderer == null)
                {
                    SetupDebugLineRenderer();
                }

                transformLineRenderer.positionCount = 2;
                transformLineRenderer.SetPositions(new[]
                {
                    transform.position,
                    new Vector3(
                        startPoint.x + length * Mathf.Cos(rad),
                        startPoint.y + length * Mathf.Sin(rad))
                });

                checkLineRenderer.positionCount = 2;
                checkLineRenderer.SetPositions(new[]
                {
                    startPoint,
                    new Vector3(testX + 0.5f, testY + 0.5f),
                });
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (!rotationSet)
            {
                RotateRandomly();
            }

            SetFoundation(foundation);
            GrowToRandomLength(); // world spawn only

        }

        private void SetupDebugLineRenderer()
        {
            transformLineRenderer = ModDebug.AddSimpleLineRenderer(transform, Color.green, Color.red);
            checkLineRenderer = ModDebug.AddSimpleLineRenderer(transform, Color.blue, Color.yellow);

            kbac.TintColour = new Color(1, 1, 1, 0.5f);
        }

        public void SetRotation(float angle)
        {
            switch (foundation)
            {
                case Direction.Down:
                    originOffset = Vector3.down;
                    break;
                case Direction.Up:
                    originOffset = Vector3.up;
                    break;
                case Direction.Left:
                    originOffset = leftOffset;
                    break;
                case Direction.Right:
                    originOffset = rightOffset;
                    break;
            }

            Log.Debug("POSITION CHANGE ");
            transform.position += originOffset;
            transform.Rotate(Vector3.forward, angle);

            //kbac.Offset = originOffset;
            rotation = angle;
            rotationSet = true;
        }

        public List<Direction> validFoundationDirections = new List<Direction>()
        {
            Direction.Down,
            Direction.Right,
            Direction.Left,
            Direction.Up
        };

        private float GetGrowthDirection(Direction foundationDir)
        {
            float angle;

            switch (foundationDir)
            {
                case Direction.Up:
                    angle = 0f;
                    break;
                case Direction.Right:
                    angle = 270f;
                    break;
                case Direction.Down:
                    angle = 180f;
                    break;
                case Direction.Left:
                default:
                    angle = 90f;
                    break;
            }

            angle += Random.Range(-45f, 45f);
            angle -= 90f;

            return angle;
        }

        public void RotateRandomly()
        {
            var originalCell = Grid.PosToCell(this);

            validFoundationDirections.Shuffle();

            foreach (var direction in validFoundationDirections)
            {
                var cell = Grid.GetCellInDirection(originalCell, direction);
                if(Grid.IsSolidCell(cell))
                {
                    SetRotation(GetGrowthDirection(direction));
                    //SetFoundation(direction);

                    return;
                }
            }

            // TODO: shatter if none are valid
        }

        private void SetFoundation(Direction direction)
        {
            foundation = direction;
            var offset = CellOffset.none;

            switch (direction)
            {
                case Direction.Up:
                    offset = new CellOffset(0, 1);
                    break;
                case Direction.Right:
                    offset = new CellOffset(1, 0);
                    break;
                case Direction.Down:
                    offset = new CellOffset(0, -1);
                    break;
                case Direction.Left:
                    offset = new CellOffset(-1, 0);
                    break;
            }

            //foundationMonitor.SetFoundationAndStartMonitoring(offset);
        }
    }
}
