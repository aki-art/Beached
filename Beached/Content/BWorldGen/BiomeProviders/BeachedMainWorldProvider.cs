using Klei;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.BWorldGen.BiomeProviders
{
    public class BeachedMainWorldProvider : IBiomeProvider
    {
        public List<WorldDetailSave.OverworldCell> GetOverworldCells(int x, int y, int worldWidth, int worldHeight)
        {
            var cell = new WorldDetailSave.OverworldCell()
            {
                zoneType = SubWorld.ZoneType.Forest,
                poly = new Delaunay.Geo.Polygon(new Rect(x, y, worldWidth, worldHeight))
            };

            return new List<WorldDetailSave.OverworldCell>()
            {
                cell
            };
        }

        public SubWorld.ZoneType GetZoneType(int x, int y)
        {
            return SubWorld.ZoneType.Forest;
        }
    }
}
