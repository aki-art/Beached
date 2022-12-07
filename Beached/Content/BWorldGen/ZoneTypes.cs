using System.Collections.Generic;
using ZoneTypeAPI;
using static ProcGen.SubWorld;

namespace Beached.Content.BWorldGen
{
    public class ZoneTypes
    {
        public static HashSet<ZoneType> values = new();

        public static ZoneType basaltShore;
        public static ZoneType beach;
        public static ZoneType bamboo;
        public static ZoneType depths;
        public static ZoneType sea;
        public static ZoneType coralReef;
        public static ZoneType icy;
        public static ZoneType pearly;

        public static void Initialize()
        {
            var zoneTypes = new ZoneTypes2();

            // texture loading is very temporary, ZoneType API is far from done
            var texName = "biome_backgrounds";
            zoneTypes.Add("BasaltShore", ModAssets.Colors.Zones.basaltShores, 7, ZoneType.FrozenWastes, "beached_assets", texName);
            zoneTypes.Add("Bamboo", ModAssets.Colors.Zones.bamboo, 1, ZoneType.Sandstone, "beached_assets", texName);
            zoneTypes.Add("Beach", ModAssets.Colors.Zones.beach, 7, ZoneType.FrozenWastes, "beached_assets", texName);
            zoneTypes.Add("Depths", ModAssets.Colors.Zones.depths, 4, ZoneType.MagmaCore, "beached_assets", texName);
            zoneTypes.Add("Sea", ModAssets.Colors.Zones.sea, 2, ZoneType.MagmaCore, "beached_assets", texName);
            zoneTypes.Add("CoralReef", ModAssets.Colors.Zones.reefs, 5, ZoneType.MagmaCore, "beached_assets", texName);
            zoneTypes.Add("Icy", ModAssets.Colors.Zones.icy, 6, ZoneType.MagmaCore, "beached_assets", texName);
            zoneTypes.Add("Pearly", ModAssets.Colors.Zones.pearly, 5, ZoneType.BoggyMarsh, "beached_assets", texName);

            zoneTypes.OnZonetypesRegistered += () =>
            {
                basaltShore = zoneTypes.Get("BasaltShore");
                beach = zoneTypes.Get("Beach");
                bamboo = zoneTypes.Get("Bamboo");
                depths = zoneTypes.Get("Depths");
                sea = zoneTypes.Get("Sea");
                coralReef = zoneTypes.Get("CoralReef");
                icy = zoneTypes.Get("Icy");
                pearly = zoneTypes.Get("Pearly");
            };
        }
    }
}
