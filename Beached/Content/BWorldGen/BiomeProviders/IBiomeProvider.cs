using Klei;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Beached.Content.BWorldGen.BiomeProviders
{
    public interface IBiomeProvider
    {
        public ZoneType GetZoneType(int x, int y);

        public List<WorldDetailSave.OverworldCell> GetOverworldCells(int x, int y, int worldWidth, int worldHeight);
    }
}
