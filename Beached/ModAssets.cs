using Beached.Content.BWorldGen;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Beached
{
    public class ModAssets
    {
        public static class Prefabs
        {
            public static Dictionary<string, GameObject> setpieces;
        }

        // static hardcoded indices for my zonetypes
        public static readonly Dictionary<int, ZoneType> biomeOverrideLookup = new()
        {
            { 1, ZoneTypes.coralReef }
        };

        public static readonly Direction[] cardinals = {
            Direction.Down,
            Direction.Up,
            Direction.Left,
            Direction.Right,
        };

        public static class Textures
        {
            public static Texture2D LUTDay;
            public static Texture2DArray germOverlays;
            public static Texture2DArray biomeBackgrounds;

            public static class Placeholders
            {
                public static Texture2D beachBg;
                public static Texture2D zeoliteBg;
            }
        }

        public static class Materials
        {
            public static Material germOverlayReplacer;
        }

        public static class Sprites
        {
            public const string MOD_MINERALOGIST = "beached_mod_mineralogist";
            public const string ERRAND_MINERALOGY = "beached_errand_mineralogy";
            public const string ARCHETYPE_MINERALOGY = "beached_archetype_mineralogy";
        }

        public static class Fx
        {
            public static SpawnFXHashes saltOff = (SpawnFXHashes)"Beached_SaltOff".GetHashCode();
            public static SpawnFXHashes grimcapPoff = (SpawnFXHashes)"Beached_GrimCapPoff".GetHashCode();
            public static SpawnFXHashes mossplosion = (SpawnFXHashes)"Beached_Mossplosion".GetHashCode();

            public static Material testMaterial;
            public static Material darkVeilPostFxMaterial;
            public static GameObject darkVeilOverlay;
            public static GameObject test;
        }

        public static class Colors
        {
            // elements
            public static Color
                amber = Util.ColorFromHex("f98f1e"),
                ammonia = Util.ColorFromHex("4d3b9b"),
                ash = Util.ColorFromHex("8a8e9d"),
                aquamarine = new Color32(74, 255, 231, 255),
                basalt = new Color32(30, 30, 50, 255),
                beryllium = Util.ColorFromHex("7da39a"),
                bismuth = new Color32(117, 166, 108, 255),
                bismuthGas = new Color32(117, 166, 108, 255),
                bismuthOre = new Color32(117, 166, 108, 255),
                bone = Util.ColorFromHex("d6cec2"),
                calcium = Color.white,
                gravel = new Color32(100, 100, 100, 255),
                iridium = Util.ColorFromHex("b6b2fb"),
                latex = Util.ColorFromHex("e08a65"),
                moltenBismuth = new Color32(117, 166, 108, 255),
                moss = Util.ColorFromHex("528b35"),
                mucus = new Color32(170, 205, 170, 255),
                mucusConduit = new Color32(170, 205, 170, 255),
                mucusUi = new Color32(170, 205, 170, 255),
                murkyBrine = new Color32(60, 61, 55, 255),
                mycelium = Util.ColorFromHex("c9bda6"),
                nitrogen = new(0.65f, 0.65f, 0.65f, 0.2f),
                nitrogenOpaque = new(0.8f, 0.8f, 0.8f),
                pearl = Util.ColorFromHex("c9bda6"),
                permaFrost = Util.ColorFromHex("68b9e2"),
                rot = Util.ColorFromHex("404930"),
                root = Util.ColorFromHex("3a3430"),
                saltyOxygen = new Color32(205, 170, 170, 120),
                selenite = Util.ColorFromHex("ffd1dc"),
                sulfurousWater = Util.ColorFromHex("d5ff2d"),
                zinc = new Color32(30, 170, 170, 255),
                zirconium = new Color32(205, 0, 0, 255),
                zeolite = Util.ColorFromHex("2aa945"),

                water = Util.ColorFromHex("39a0f7"),
                saltWater = Util.ColorFromHex("7fe4ff"),

                zirconSpecular = new(2f, 0, 0),
                zincSpecular = Util.ColorFromHex("02b976"),

            // germs
                plankton = new Color32(0, 0, 255, 255),
                limpetEggs = new Color32(255, 225, 185, 255),
                capSpores = Color.red;

            public class Zones
            {
                public static Color
                    bamboo = Util.ColorFromHex("809D17FF"),
                    basaltShores = new Color32(211, 186, 157, 0),
                    beach = Util.ColorFromHex("C06410FF"),
                    bladder = Util.ColorFromHex("4DAD22FF"), // B5BBDBF
                    depths = Util.ColorFromHex("1578FFFF"),
                    icy = Util.ColorFromHex("90BFDBFF"), // B5BBDBF
                    reefs = Util.ColorFromHex("B8697E"),
                    sea = Util.ColorFromHex("63D6DEFF"),
                    pearly = Util.ColorFromHex("0E0906FF");
            }

            public class UI
            {
                public static Color beachedTutorialBG = Util.ColorFromHex("60cd5227");
                public static Color beachedTutorialBGHover = Util.ColorFromHex("1adf0019");
            }

            public static string positiveColorHex = GameUtil.BreathableValues.positiveColor.ToHexString();
            public static string warningColorHex = GameUtil.BreathableValues.warningColor.ToHexString();
            public static string negativeColorHex = GameUtil.BreathableValues.negativeColor.ToHexString();
        }

        public static void LoadAssets()
        {
            var assets = Path.Combine(Mod.folder, "Assets");

            Textures.LUTDay = LoadTexture(Path.Combine(assets, "textures", "cc_day_bright_and_saturated.png"));
            Textures.Placeholders.beachBg = LoadTexture(Path.Combine(assets, "textures", "bgplaceholders", "beach.png"));
            Textures.Placeholders.zeoliteBg = LoadTexture(Path.Combine(assets, "textures", "bgplaceholders", "heulandite_geode.png"));

            Log.Debug("LOADING ASSETS");

            var bundle = LoadAssetBundle("beached_assets", platformSpecific: true);
            Materials.germOverlayReplacer = new Material(bundle.LoadAsset<Shader>("Assets/Beached/D_GermOverlay.shader"));
            Textures.germOverlays = bundle.LoadAsset<Texture2DArray>("Assets/Beached/Images/combined.png");
            foreach (var asset in bundle.GetAllAssetNames())
            {
                Log.Debug(asset);
            }
            
            Prefabs.setpieces = new();

            var testSetPiece = bundle.LoadAsset<GameObject>("Assets/Beached/fx/test_setpiece.prefab");

            foreach (var renderer in testSetPiece.GetComponents<SpriteRenderer>())
            {
                renderer.material.renderQueue = RenderQueues.Liquid;
            }

            var bg = testSetPiece.transform.Find("bg 1");
            var bgRenderer = bg.GetComponent<SpriteRenderer>();
            var sprite = bgRenderer.sprite;
            bgRenderer.material = new Material(Shader.Find("Sprites/Default"));
            bgRenderer.sprite = sprite;

            Prefabs.setpieces.Add("test", testSetPiece);

            Debug.Assert(Materials.germOverlayReplacer != null, "mat is null");
            Debug.Assert(Materials.germOverlayReplacer.shader != null, "shader is null");

        }

        public static bool TryLoadTexture(string path, out Texture2D texture)
        {
            texture = LoadTexture(path, Mod.debugMode);
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

        public static void SaveImage(Texture textureToWrite, string name)
        {
            var texture2D = new Texture2D(textureToWrite.width, textureToWrite.height, TextureFormat.RGBA32, false);

            var renderTexture = new RenderTexture(textureToWrite.width, textureToWrite.height, 32);
            Graphics.Blit(textureToWrite, renderTexture);

            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            var bytes = texture2D.EncodeToPNG();
            var dirPath = Mod.folder;

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllBytes(Path.Combine(dirPath, name) + System.DateTime.Now + ".png", bytes);

            Log.Debug("Saved to " + dirPath);
        }

        public static AssetBundle LoadAssetBundle(string assetBundleName, string path = null, bool platformSpecific = false)
        {
            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == assetBundleName)
                {
                    return bundle;
                }
            }

            if (path.IsNullOrWhiteSpace())
            {
                path = Path.Combine(Mod.folder, "assets");
            }

            if (platformSpecific)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                        path = Path.Combine(path, "windows");
                        break;
                    case RuntimePlatform.LinuxPlayer:
                        path = Path.Combine(path, "linux");
                        break;
                    case RuntimePlatform.OSXPlayer:
                        path = Path.Combine(path, "mac");
                        break;
                }
            }

            path = Path.Combine(path, assetBundleName);

            var assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null)
            {
                Log.Warning($"Failed to load AssetBundle from path {path}");
                return null;
            }

            return assetBundle;
        }
    }
}
