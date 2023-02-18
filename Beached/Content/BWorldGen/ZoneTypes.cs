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

        public static ZoneType 
            basaltShore,
            beach,
            bamboo,
            bone,
            sulfur,
            depths,
            sea,
            coralReef,
            icy,
            pearly;

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
            Add("Bone", ModAssets.Colors.Zones.bone, 0, ZoneType.BoggyMarsh);
            Add("Sulfur", ModAssets.Colors.Zones.sulfur, 8, ZoneType.BoggyMarsh);

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
                bone = zones.Get("Bone");
                sulfur = zones.Get("Sulfur");

                values.Add(basaltShore);
                values.Add(beach);
                values.Add(bamboo);
                values.Add(depths);
                values.Add(sea);
                values.Add(coralReef);
                values.Add(icy);
                values.Add(pearly);
                values.Add(bone);
                values.Add(sulfur);
            };
        }

        private static void Add(string id, Color color, int textureIndex, ZoneType zoneType)
        {
            zones.Add(id, color, textureIndex, zoneType, "beached_assets", "biome_backgrounds");
        }
    }
}
