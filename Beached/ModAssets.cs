using System;
using System.IO;
using UnityEngine;
using static STRINGS.CREATURES.STATS;

namespace Beached
{
    public class ModAssets
    {
        public static class Textures
        {
            public static Texture2D LUTDay;
        }

        public static class Fx
        {
            public static SpawnFXHashes saltOff = (SpawnFXHashes)"saltOff".GetHashCode();

            public static Material testMaterial;
            public static Material darkVeilPostFxMaterial;
            public static GameObject darkVeilOverlay;
            public static GameObject test;
        }

        public static class Colors
        {
            // elements
            public static Color amber = Util.ColorFromHex("f98f1e");
            public static Color ammonia = Util.ColorFromHex("4d3b9b");
            public static Color ash = Util.ColorFromHex("8a8e9d");
            public static Color aquamarine = new Color32(74, 255, 231, 255);
            public static Color basalt = new Color32(30, 30, 50, 255);
            public static Color beryllium = Util.ColorFromHex("7da39a");
            public static Color bismuth = new Color32(117, 166, 108, 255);
            public static Color bismuthGas = new Color32(117, 166, 108, 255);
            public static Color bismuthOre = new Color32(117, 166, 108, 255);
            public static Color calcium = Color.white;
            public static Color gravel = new Color32(100, 100, 100, 255);
            public static Color moltenBismuth = new Color32(117, 166, 108, 255);
            public static Color mucus = new Color32(170, 205, 170, 255);
            public static Color mucusConduit = new Color32(170, 205, 170, 255);
            public static Color mucusUi = new Color32(170, 205, 170, 255);
            public static Color murkyBrine = new Color32(60, 61, 55, 255);
            public static Color mycelium = Util.ColorFromHex("c9bda6");
            public static Color pearl = Util.ColorFromHex("c9bda6");
            public static Color root = Util.ColorFromHex("3a3430");
            public static Color saltyOxygen = new Color32(205, 170, 170, 120);
            public static Color selenite = Util.ColorFromHex("ffd1dc");
            public static Color sulfurousWater = Util.ColorFromHex("d5ff2d");
            public static Color zinc = new Color32(30, 170, 170, 255);
            public static Color zirconium = new Color32(205, 0, 0, 255);
            public static Color zeolite = Util.ColorFromHex("2aa945");

            public static Color water = Util.ColorFromHex("39a0f7");
            public static Color saltWater = Util.ColorFromHex("7fe4ff");

            // germs
            public static Color plankton = new Color32(0, 0, 255, 255);
            public static Color limpetEggs = new Color32(255, 225, 185, 255);

            public static Color zirconSpecular = new Color(2f, 0, 0);
            public static Color zincSpecular = new Color(0f, 1.2f, 1.7f);

            public class Zones
            {
                public static Color bamboo = Util.ColorFromHex("809D17FF");
                public static Color basaltShores = new Color32(211, 186, 157, 0);
                public static Color beach = Util.ColorFromHex("C06410FF");
                public static Color bladder = Util.ColorFromHex("4DAD22FF"); // B5BBDBFF
                public static Color depths = Util.ColorFromHex("1578FFFF");
                public static Color icy = Util.ColorFromHex("90BFDBFF"); // B5BBDBFF
                public static Color reefs = Util.ColorFromHex("AB5DB0FF");
                public static Color sea = Util.ColorFromHex("63D6DEFF");
                public static Color pearly = Util.ColorFromHex("0E0906FF");
            }

            public static string positiveColorHex = GameUtil.BreathableValues.positiveColor.ToHexString();
            public static string warningColorHex = GameUtil.BreathableValues.warningColor.ToHexString();
            public static string negativeColorHex = GameUtil.BreathableValues.negativeColor.ToHexString();
        }

        public static void LoadAssets()
        {
            var assets = Path.Combine(Mod.folder, "Assets");

            Textures.LUTDay = LoadTexture(Path.Combine(assets, "cc_day_bright_and_saturated.png"));
        }

        public static bool TryLoadTexture(string path, out Texture2D texture)
        {
            texture = LoadTexture(path, Mod.DebugMode);
            return texture != null;
        }

        public static Texture2D LoadTexture(string path, bool warnIfFailed = true)
        {
            Texture2D texture = null;

            if (File.Exists(path))
            {
                var data = TryReadFile(path);
                texture = new Texture2D(1, 1);
                texture.LoadImage(data);
            }
            else if (warnIfFailed)
            {
                Log.Warning($"Could not load texture at path {path}.");
            }

            return texture;
        }

        public static byte[] TryReadFile(string texFile)
        {
            try
            {
                return File.ReadAllBytes(texFile);
            }
            catch (Exception e)
            {
                Log.Warning("Could not read file: " + e);
                return null;
            }
        }
    }
}
