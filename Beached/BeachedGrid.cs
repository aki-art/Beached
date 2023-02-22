using KSerialization;
using System;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Beached
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedGrid : KMonoBehaviour
    {
        public const int INVALID_FORCEFIELD_OFFSET = -1;

        [Serialize]
        private Dictionary<int, NaturalTileInfo> naturalTiles = new();

        [Serialize]
        public Dictionary<int, ZoneType> zoneTypeOverrides = new();

        public static Dictionary<Vector2I, ZoneType> worldgenZoneTypes;
        public static Dictionary<int, int> forceFieldLevelPerWorld = new();

        [Serialize]
        private bool initialized;

        public static BeachedGrid Instance;

        public override void OnPrefabInit() => Instance = this;

        public override void OnCleanUp() => Instance = null;

        public override void OnSpawn()
        {
            Log.Debug("BeachedGrid OnSpawn");
            if (worldgenZoneTypes != null)
            {
                Log.Debug("beachedgrid has worldgen data " + worldgenZoneTypes.Count);
                foreach (var cell in worldgenZoneTypes)
                {
                    zoneTypeOverrides[Grid.PosToCell(cell.Key)] = cell.Value;
                }

                //worldgenZoneTypes.Clear();
            }

            RegenerateBackwallTexture();
            World.Instance.zoneRenderData.OnActiveWorldChanged();
        }

        public void RegenerateBackwallTexture()
        {
            if (World.Instance.zoneRenderData == null)
            {
                Debug.Log("Subworld zone render data is not yet initialized.");
                return;
            }

            var zoneRenderData = World.Instance.zoneRenderData;

            var zoneIndices = zoneRenderData.colourTex.GetRawTextureData();
            var colors = zoneRenderData.indexTex.GetRawTextureData();

            foreach (var tile in zoneTypeOverrides)
            {
                var cell = tile.Key;
                var zoneType = (byte)tile.Value;

                var color = World.Instance.zoneRenderData.zoneColours[zoneType];
                colors[cell] = (tile.Value == ZoneType.Space) ? byte.MaxValue : zoneType;

                zoneIndices[cell * 3] = color.r;
                zoneIndices[cell * 3 + 1] = color.g;
                zoneIndices[cell * 3 + 2] = color.b;

                World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
            }

            zoneRenderData.colourTex.LoadRawTextureData(zoneIndices);
            zoneRenderData.indexTex.LoadRawTextureData(colors);
            zoneRenderData.colourTex.Apply();
            zoneRenderData.indexTex.Apply();

            zoneRenderData.OnShadersReloaded();
        }

        public bool TryGetNaturalTileInfo(int cell, out NaturalTileInfo info) => naturalTiles.TryGetValue(cell, out info);

        public void Initialize()
        {
            if (!initialized)
            {
                for (int cell = 0; cell < Grid.CellCount; cell++)
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
