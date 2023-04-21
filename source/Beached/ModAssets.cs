using Beached.Content.BWorldGen;
using Beached.Content.Scripts;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TemplateClasses;
using UnityEngine;
using static ProcGen.SubWorld;
using Object = UnityEngine.Object;

namespace Beached
{
    public class ModAssets
    {
        public const string BASE_FOLDER = "assets/textures";
        public static class Prefabs
        {
            public static Dictionary<string, GameObject> setpieces;
            public static GameObject universalSidescreen; // prefab with commonly used controls ready to go
            public static GameObject cometTrailFx;
            public static GameObject testQuad;
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
            public static Texture2D forceFieldGrid;
            public static Texture2D forceFieldBlurMap;

            public static class Placeholders
            {
                public static Texture2D zeoliteBg;
            }
        }

        public static class Sounds
        {
            public const string SHELL_CHIME_LOUD = "Beached_Chimes_Loud";
        }

        public static class Materials
        {
            public static Material germOverlayReplacer;
            public static Material forceField;
            public static Material liquidRefractionMat;
        }

        public static class Sprites
        {
            public const string MOD_MINERALOGIST = "beached_mod_mineralogist";
            public const string ERRAND_MINERALOGY = "beached_errand_mineralogy";
            public const string ARCHETYPE_MINERALOGY = "beached_archetype_mineralogy";
            public const string BUILDCATEGORY_POIS = "beached_buildcategory_pois";
            public const string STATUSITEM_DRIEDOUT = "beached_statusitem_driedout";
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
            public static Color
                // elements
                amber = Util.ColorFromHex("f98f1e"),
                ammonia = Util.ColorFromHex("4d3b9b"),
                ash = Util.ColorFromHex("8a8e9d"),
                aquamarine = new(0.05f, 1.2f, 2.51f),
                basalt = new Color32(30, 30, 50, 255),
                beryllium = Util.ColorFromHex("7da39a"),
                bismuth = new Color32(117, 166, 108, 255),
                bismuthGas = new Color32(117, 166, 108, 255),
                bismuthOre = new Color32(117, 166, 108, 255),
                bone = Util.ColorFromHex("d6cec2"),
                calcium = Color.white,
                coquina = Util.ColorFromHex("b49b8a"),
                gravel = new Color32(100, 100, 100, 255),
                iridium = Util.ColorFromHex("b6b2fb") * 2f,
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
                capSpores = Color.red,
                poffSpores = Color.white;

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
                    //sea = Util.ColorFromHex("63D6DEFF"),
                    sea = Util.ColorFromHex("C7CDDEFF"),
                    pearly = Util.ColorFromHex("0E0906FF"),
                    bone = Util.ColorFromHex("D3BCD0"),
                    sulfur = Util.ColorFromHex("ffff00");
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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Log.Debug("Loading Assets...");

            var assets = Path.Combine(Mod.folder, "assets");
            LoadSounds(Path.Combine(assets, "sounds"));

            Textures.Placeholders.zeoliteBg = LoadTexture(Path.Combine(assets, "textures", "bgplaceholders", "heulandite_geode.png"));
            Textures.LUTDay = LoadTexture(Path.Combine(assets, "textures", "cc_day_bright_and_saturated.png"));

            var bundle = LoadAssetBundle("beached_assets", platformSpecific: true);
            var shadersBundle = LoadAssetBundle("beached_shaders", platformSpecific: true);

            foreach (var asset in bundle.GetAllAssetNames())
            {
                Log.Debug(asset);
            }

            Prefabs.universalSidescreen = bundle.LoadAsset<GameObject>("Assets/generated assets/tmp converted ui/UniversalSidescreen_tmpconverted.prefab");

            var tmpConverter = new TMPConverter();
            tmpConverter.ReplaceAllText(Prefabs.universalSidescreen);

            /*            Materials.germOverlayReplacer = new Material(bundle.LoadAsset<Shader>("Assets/Beached/D_GermOverlay.shader"));
                        Materials.forceField = new(bundle.LoadAsset<Shader>("Assets/Beached/ForceField.shader"))
                        {
                            renderQueue = RenderQueues.Liquid
                        };*/

            Materials.forceField = new(Shader.Find("Klei/BloomedParticleShader"));

            //Textures.germOverlays = bundle.LoadAsset<Texture2DArray>("Assets/Beached/Images/combined.png");

            //Textures.forceFieldGrid = bundle.LoadAsset<Texture2D>("Assets/Beached/Images/grid_b.png");
            // Textures.forceFieldBlurMap = bundle.LoadAsset<Texture2D>("Assets/Beached/Images/blurmap.png");
            // LoadSetpieces(bundle);
/*
            Prefabs.cometTrailFx = bundle.LoadAsset<GameObject>("Assets/Beached/fx/CometSparkles.prefab");
            var renderer = Prefabs.cometTrailFx.GetComponent<ParticleSystemRenderer>();
            var texture = renderer.material.mainTexture;
            renderer.material = new Material(Shader.Find("Klei/BloomedParticleShader"))
            {
                renderQueue = RenderQueues.Liquid,
                mainTexture = texture
            };*/

            Materials.liquidRefractionMat = shadersBundle.LoadAsset<Material>("Assets/Materials/Beached_LiquidRefraction.mat");

            sw.Stop();
            Log.Info($"Finished loading assets. It took {sw.ElapsedMilliseconds} ms");
        }

        private static void LoadSetpieces(AssetBundle bundle)
        {
            Prefabs.setpieces = new();

            var testSetPiece = bundle.LoadAsset<GameObject>("Assets/Beached/fx/test_setpiece.prefab");

            SetupSetPiece(testSetPiece);
            Prefabs.setpieces.Add("test", testSetPiece);

            var beachSetPiece = bundle.LoadAsset<GameObject>("Assets/Beached/fx/parallax/Beach/beach_setpiece.prefab");
            beachSetPiece.GetComponent<Transform>().localScale *= 2f;
            var setPieceMaterial = new Material(bundle.LoadAsset<Shader>("Assets/Beached/fx/parallax/BeachedParallax.shader"));
            foreach (var renderer in beachSetPiece.GetComponents<SpriteRenderer>())
            {
                renderer.material = setPieceMaterial;
                renderer.material.renderQueue = RenderQueues.Liquid;
            }

            beachSetPiece.AddComponent<ParallaxBg>().DeserializeFromJson();

            Prefabs.setpieces.Add("beach", beachSetPiece);
        }

        /*
                private static void ProcessTMP(GameObject gameObject)
                {
                    var textComponents = gameObject.GetComponentsInChildren(typeof(TextMeshProUGUI), true);

                    foreach (var text in textComponents.Cast<TextMeshProUGUI>())
                    {
                        text.font = text.font.name.Contains("GRAYSTROKE") ? GrayStroke : NotoSans;
                        var newText = TMPFixer.ConvertToLocText(text);
                        //f_m_isAlignmentEnumConverted.SetValue(text, true);

                        Log.Debug("alignment: " + text.alignment);

                        //newText.text = STRINGS.FormatAsLink("读写汉字 - 学中文", "DECOR");
                    }
                }
        */
        private static void SetupSetPiece(GameObject testSetPiece)
        {
            foreach (var renderer in testSetPiece.GetComponents<SpriteRenderer>())
            {
                renderer.material.renderQueue = RenderQueues.Liquid;
            }

            var bg = testSetPiece.transform.Find("bg 1");
            var bgRenderer = bg.GetComponent<SpriteRenderer>();
            var sprite = bgRenderer.sprite;
            bgRenderer.material = new Material(Shader.Find("Sprites/Default"));
            bgRenderer.sprite = sprite;
        }

        private static void LoadSounds(string path)
        {
            Log.Debug(Path.Combine(path, "511636__aslipasli__chimes.wav"));
            AudioUtil.LoadSound(Sounds.SHELL_CHIME_LOUD, Path.Combine(path, "511636__aslipasli__chimes.wav"));
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

            texture2D.ReadPixels(new(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            var bytes = texture2D.EncodeToPNG();
            var dirPath = Mod.folder;

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllBytes(Path.Combine(dirPath, name) + System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".png", bytes);

            Log.Debug("Saved to " + dirPath);
        }

        public static AssetBundle LoadAssetBundle(string assetBundleName, string path = null, bool platformSpecific = false)
        {
            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == assetBundleName)
                {
                    Log.Warning("trying to load duplicate asset bundle " + bundle.name);
                    return bundle;
                }
            }

            path ??= Path.Combine(Mod.folder, "assetbundles");

            if (platformSpecific)
            {
                path = Application.platform switch
                {
                    RuntimePlatform.WindowsPlayer => Path.Combine(path, "windows"),
                    RuntimePlatform.LinuxPlayer => Path.Combine(path, "linux"),
                    RuntimePlatform.OSXPlayer => Path.Combine(path, "mac"),
                    _ => path
                };
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

        public static TextureAtlas GetCustomAtlas(string fileName, string folder, TextureAtlas tileAtlas)
        {
            var path = Mod.folder;

            if (folder != null)
            {
                path = Path.Combine(path, folder);
            }

            var tex = LoadTexture(Path.Combine(path, fileName + ".png"));

            if (tex == null)
            {
                return null;
            }

            var atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = tex;
            atlas.scaleFactor = tileAtlas.scaleFactor;
            atlas.items = tileAtlas.items;

            return atlas;
        }

        public static void AddCustomTileAtlas(BuildingDef def, string textureName, bool shiny = false, string referenceAtlas = "tiles_metal")
        {
            var reference = Assets.GetTextureAtlas(referenceAtlas);
            def.BlockTileAtlas = GetCustomAtlas($"{textureName}_main", BASE_FOLDER, reference);
            def.BlockTilePlaceAtlas = GetCustomAtlas($"{textureName}_place", BASE_FOLDER, reference);

            if (shiny)
            {
                def.BlockTileShineAtlas = GetCustomAtlas($"{textureName}_spec", BASE_FOLDER, reference);
            }
        }

        public static void AddCustomTileTops(BuildingDef def, string name, bool shiny = false, string decorInfo = "tiles_glass_tops_decor_info", string existingPlaceID = null, string existingSpecID = null)
        {
            var info = Object.Instantiate(Assets.GetBlockTileDecorInfo(decorInfo));

            if (info != null)
            {
                info.atlas = GetCustomAtlas($"{name}_tops", BASE_FOLDER, info.atlas);
                def.DecorBlockTileInfo = info;

                if (shiny)
                {
                    var id = existingSpecID.IsNullOrWhiteSpace() ? $"{name}_tops_spec" : existingSpecID;
                    info.atlasSpec = GetCustomAtlas(id, BASE_FOLDER, info.atlasSpec);
                }
            }

            if (existingPlaceID.IsNullOrWhiteSpace())
            {
                var placeInfo = Object.Instantiate(Assets.GetBlockTileDecorInfo(decorInfo));
                placeInfo.atlas = GetCustomAtlas($"{name}_tops_place", BASE_FOLDER, placeInfo.atlas);
                def.DecorPlaceBlockTileInfo = placeInfo;
            }
            else
            {
                def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo(existingPlaceID);
            }
        }

        internal static void LoadKAnims()
        {
            Log.Debug("Loading KAnims");
            Stopwatch stopWatch = new();
            stopWatch.Start();


            var bundle = LoadAssetBundle("beached_kanim_assets", platformSpecific: false);



            stopWatch.Stop();
            Log.Info($"Finished loading Beached animations. It took {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
