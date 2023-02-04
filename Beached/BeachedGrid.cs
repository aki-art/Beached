using KSerialization;
using System;
using System.Collections.Generic;

namespace Beached
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedGrid : KMonoBehaviour
    {
        [Serialize]
        private readonly Dictionary<int, NaturalTileInfo> naturalTiles = new();

        [Serialize]
        private bool initialized;

        public static BeachedGrid Instance;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public bool TryGetNaturalTileInfo(int cell, out NaturalTileInfo info) => naturalTiles.TryGetValue(cell, out info);

        public void Initialize()
        {
            if(!initialized)
            {
                for(int cell = 0; cell < Grid.CellCount; cell++)
                {
                    var element = Grid.Element[cell];
                    if (element.IsSolid && element.id != SimHashes.Unobtanium)
                    {
                        naturalTiles[cell] = new NaturalTileInfo()
                        {
                            id = element.id,
                            mass = Grid.Mass[cell]
                        };
                    }
                }

                initialized = true;
            }
        }

        [Serializable]
        public class NaturalTileInfo
        {
            public SimHashes id;
            public float mass;
        }
    }
}
