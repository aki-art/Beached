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
        public float rotation;

        [Serialize]
        public Direction foundation;

        [Serialize]
        public bool rotationSet;

        [SerializeField]
        public bool allowDiagonalGrowth;

        private static Vector3 leftOffset = new Vector3(-0.5f, 0.5f);
        private static Vector3 rightOffset = new Vector3(0.5f, 0.5f);

        public void GrowToRandomLength()
        {
            var length = Random.Range(1, 5);

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

        private bool TestLength(float length)
        {
            var rad = Mathf.Deg2Rad * rotation;
            var testX = (int)(transform.position.x + length * Mathf.Cos(rad)); //radians!!
            var testY = (int)(transform.position.y + length * Mathf.Sin(rad));
            testX = (int)(testX + 0.5f);
            testY = (int)(testY + 0.5f);

            var cell = Grid.XYToCell(testX, testY);

            if (!Grid.Solid[cell])
            {
                if (Grid.TestLineOfSight((int)transform.position.x, (int)transform.position.y, testX, testY, Grid.IsSolidCell))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if(!rotationSet)
            {
                RotateRandomly();
            }

            GrowToRandomLength();
        }

        public void SetRotation(float angle)
        {
            //kbac.Rotation = angle;
            transform.Rotate(Vector3.forward, angle);
            
            switch (foundation)
            {
                case Direction.Down:
                    kbac.Offset = Vector3.zero;
                    break;
                case Direction.Up:
                    kbac.Offset = Vector3.up;
                    break;
                case Direction.Left:
                    kbac.Offset = leftOffset;
                    break;
                case Direction.Right:
                    kbac.Offset = rightOffset;
                    break;
            }

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
                    foundation = direction;
                    SetRotation(GetGrowthDirection(direction));
                    return;
                }
            }

            // TODO: shatter if none are valid
        }
    }
}
