using System;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class AmbientReservoir : ElementConsumer
    {
        [SerializeField]
        public Storage storage;

        [SerializeField]
        public int volumeCubicMeter;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
        }

        private void OnStorageChange(object obj)
        {
            var cell = GetSampleCell();
            var worldElement = Grid.Element[cell];
            var worldMass = Grid.Mass[cell];

            var storedElement = storage.FindFirst(worldElement.tag);
            if(storedElement.TryGetComponent(out PrimaryElement primaryElement))
            {
                var storedMass = primaryElement.Mass;

                if(Mathf.Approximately(storedMass, Grid.Mass[cell])) 
                {
                    return;
                }

                var massToMove = Mathf.Abs(storedMass - worldMass) / 2f;
                if (storedMass > Grid.Mass[cell])
                {
                    // we have more stored, give back
                    storage.DropSome(worldElement.tag, massToMove, true, true);
                }
                else
                {
                    
                }
            }
        }

        protected override bool IsActive()
        {
            return true;
        }

        private int GetSampleCell()
        {
            return Grid.PosToCell(transform.GetPosition() + sampleCellOffset);
        }
    }
}
