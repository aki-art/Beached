using System.Collections.Generic;
using UnityEngine;
using ZoneTypeAPI;
using static ProcGen.SubWorld;

namespace Beached.Content.BWorldGen
{
    public class ZoneTypes
    {
        public static ZoneTypes2 zones;
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
            zones = new ZoneTypes2();

            // texture loading is very temporary, ZoneType API is far from done
            Add("BasaltShore", ModAssets.Colors.Zones.basaltShores, 7, ZoneType.FrozenWastes);
            Add("Bamboo", ModAssets.Colors.Zones.bamboo, 1, ZoneType.Sandstone);
            Add("Beach", ModAssets.Colors.Zones.beach, 7, ZoneType.FrozenWastes);
            Add("Depths", ModAssets.Colors.Zones.depths, 4, ZoneType.MagmaCore);
            Add("Sea", ModAssets.Colors.Zones.sea, 2, ZoneType.MagmaCore);
            Add("CoralReef", ModAssets.Colors.Zones.reefs, 5, ZoneType.MagmaCore);
            Add("Icy", ModAssets.Colors.Zones.icy, 6, ZoneType.MagmaCore);
            Add("Pearly", ModAssets.Colors.Zones.pearly, 5, ZoneType.BoggyMarsh);

            zones.OnZonetypesRegistered += () =>
            {
                basaltShore = zones.Get("BasaltShore");
                beach = zones.Get("Beach");
                bamboo = zones.Get("Bamboo");
                depths = zones.Get("Depths");
                sea = zones.Get("Sea");
                coralReef = zones.Get("CoralReef");
                icy = zones.Get("Icy");
                pearly = zones.Get("Pearly");

                values.Add(basaltShore);
                values.Add(beach);
                values.Add(bamboo);
                values.Add(depths);
                values.Add(sea);
                values.Add(coralReef);
                values.Add(icy);
                values.Add(pearly);
            };
        }

        private static void Add(string id, Color color, int textureIndex, ZoneType zoneType)
        {
            zones.Add(id, color, textureIndex, zoneType, "beached_assets", "biome_backgrounds");
        }
    }
}
