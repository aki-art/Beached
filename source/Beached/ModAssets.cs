using Beached.Content.BWorldGen;
using Beached.Content.Scripts;
using FUtility.FUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProcGen.SubWorld;
using Object = UnityEngine.Object;

namespace Beached
{
	public class ModAssets
	{
		public const string BASE_FOLDER = "assets/textures";
		public const string NEW_ASSETBUNDLE = "beached_assets2";

		public const string SHIRT2_SNAP = "beached_snapto_shirt2";

		public static class Prefabs
		{
			public static Dictionary<string, GameObject> setpieces;
			public static GameObject universalSidescreen; // prefab with commonly used controls ready to go
			public static GameObject cometTrailFx;
			public static GameObject testQuad;
			public static GameObject forceFieldDome;
			public static GameObject asteroidBelt;
			public static GameObject critterIdentitySidescreen;
			public static GameObject muffinSideScreen;
		}

		// static hardcoded indices for my zonetypes
		public static readonly Dictionary<int, ZoneType> biomeOverrideLookup = new()
		{
			{ 1, ZoneTypes.coralReef }
		};

		public static readonly Direction[] cardinals = [
			Direction.Down,
			Direction.Up,
			Direction.Left,
			Direction.Right,
		];

		public static class Textures
		{
			public static Texture2D LUTDay;
			public static Texture2DArray germOverlays;
			public static Texture2DArray biomeBackgrounds;
			public static Texture2D forceFieldGrid;
			public static Texture2D forceFieldBlurMap;
			public static Texture2D dirtLigher;
			public static Texture2D forceField;
			public static Texture dirtOriginal;

			public static class Placeholders
			{
				public static Texture2D zeoliteBg;
			}
		}

		public static class Sounds
		{
			public const string
				SHELL_CHIME_LOUD = "Beached_Chimes_Loud",
				MUSSEL_SPROUT_HARVEST = "Beached_Mussel_Sprout_Harvet";
		}

		public static class Materials
		{
			public static Material germOverlayReplacer;
			public static Material forceField;
			public static Material liquidRefractionMat;
			public static Material forceField2;
			public static Material darkVeil;
			public static Material zoneTypeMaskMaterial;
		}

		public static class Sprites
		{
			public const string
				MOD_MINERALOGIST = "beached_mod_mineralogist",
				ERRAND_MINERALOGY = "beached_errand_mineralogy",
				ARCHETYPE_MINERALOGY = "beached_archetype_mineralogy",
				BUILDCATEGORY_POIS = "beached_buildcategory_pois",
				STATUSITEM_DRIEDOUT = "beached_statusitem_driedout";
		}

		public static class CONTEXTS
		{
			public static readonly HashedString
				HARVEST_ORANGE_SQUISH = "beached_harvest_orangesquish",
				SAND = "beached_sand",
				THAWING = "beached_thawing";
		}

		public static class Fx
		{
			public static SpawnFXHashes
				saltOff = (SpawnFXHashes)"Beached_SaltOff".GetHashCode(),
				grimcapPoff = (SpawnFXHashes)"Beached_GrimCapPoff".GetHashCode(),
				ammoniaBubbles = (SpawnFXHashes)"Beached_AmmoniaBubbles".GetHashCode(),
				mossplosion = (SpawnFXHashes)"Beached_Mossplosion".GetHashCode(),
				mossplosionRed = (SpawnFXHashes)"Beached_MossplosionRed".GetHashCode();

			public static Material testMaterial;
			public static Material darkVeilPostFxMaterial;
			public static GameObject darkVeilOverlay;
			public static GameObject electricOverlay;

			public struct ParticleFxSet
			{
				public static ParticleSystemPlayer ammoniaBubblesUp;
				public static ParticleSystemPlayer ammoniaBubblesDown;
				public static ParticleSystemPlayer ammoniaBubblesSide;
			}

			public static class Lasers
			{
				public const string
					SQUISH = "Beached_Laser_Squish",
					SAND = "Beached_Laser_Sand",
					FLAMETHROWER = "Beached_Laser_FlameThrower";

				public static void AddLaserEffects(GameObject minionPrefab)
				{
					var laserEffects = minionPrefab.transform.Find("LaserEffect").gameObject;
					var kbatchedAnimEventToggler = laserEffects.GetComponent<KBatchedAnimEventToggler>();
					var kbac = minionPrefab.GetComponent<KBatchedAnimController>();

					AddLaserEffect(SQUISH, CONTEXTS.HARVEST_ORANGE_SQUISH, kbatchedAnimEventToggler, kbac, "beached_squish_harvest_beam_kanim", "loop");
					AddLaserEffect(SAND, CONTEXTS.SAND, kbatchedAnimEventToggler, kbac, "beached_sand_beam_kanim", "loop");
					AddLaserEffect(FLAMETHROWER, CONTEXTS.THAWING, kbatchedAnimEventToggler, kbac, "beached_flamethrower_beam_kanim", "loop");
				}

				private static void AddLaserEffect(string ID, HashedString context, KBatchedAnimEventToggler kbatchedAnimEventToggler, KBatchedAnimController kbac, string animFile, string defaultAnimation = "loop")
				{
					var laserEffect = new BaseMinionConfig.LaserEffect
					{
						id = ID,
						animFile = animFile,
						anim = defaultAnimation,
						context = context
					};

					var laserGo = new GameObject(laserEffect.id);
					Log.Debug("setting laser parebt");
					laserGo.transform.parent = kbatchedAnimEventToggler.transform;
					laserGo.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);

					var tracker = laserGo.AddOrGet<KBatchedAnimTracker>();
					tracker.controller = kbac;
					tracker.symbol = new HashedString("snapTo_rgtHand");
					tracker.offset = new Vector3(195f, -35f, 0f);
					tracker.useTargetPoint = true;

					var kbatchedAnimController = laserGo.AddOrGet<KBatchedAnimController>();
					kbatchedAnimController.AnimFiles =
					[
						Assets.GetAnim(laserEffect.animFile)
					];

					var item = new KBatchedAnimEventToggler.Entry
					{
						anim = laserEffect.anim,
						context = laserEffect.context,
						controller = kbatchedAnimController
					};

					kbatchedAnimEventToggler.entries.Add(item);

					laserGo.AddOrGet<LoopingSounds>();
				}
			}
		}

		public static class Colors
		{
			public static Color
				beached = Util.ColorFromHex("e99d7f"),

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
				sourBrine = Util.ColorFromHex("8c5075"),
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
				capSpores = Util.ColorFromHex("3db1ff"),
				poffSpores = Color.white,
				iceWrath = Util.ColorFromHex("80b5ff"),
				fur = new Color32(210, 40, 180, 255),

				electricBlue = Util.ColorFromHex("0090ff"),
				liquidOverlayBlue = Util.ColorFromHex("3dc8ff"),
				gasOverlayPurple = Util.ColorFromHex("bc85ff");

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
			var sw = new Stopwatch();
			sw.Start();

			var assets = Path.Combine(Mod.folder, "assets");
			Log.Debug("initiating asset bundles");
			var bundle = LoadAssetBundle("beached_assets", platformSpecific: true);
			var sharedAssetsBundle = LoadAssetBundle("beached_shared_assets", platformSpecific: false);
			var bundle2 = LoadAssetBundle(NEW_ASSETBUNDLE, platformSpecific: true);
			var shadersBundle = LoadAssetBundle("beached_shaders", platformSpecific: true);

			/*	var up = bundle2.LoadAsset<GameObject>("Assets/Prefabs/AmmoniaBubbles/AmmoniaBubbles.prefab");
				Fx.ParticleFxSet.ammoniaBubblesUp = up.AddComponent<ParticleSystemPlayer>();
				Fx.ParticleFxSet.ammoniaBubblesUp.duration = 1f;
	*/
			Log.Debug("loading sounds");
			LoadSounds(Path.Combine(assets, "sounds"));

			Log.Debug("loading textures");
			Textures.Placeholders.zeoliteBg = LoadTexture(Path.Combine(assets, "textures", "bgplaceholders", "heulandite_geode.png"));
			Textures.LUTDay = LoadTexture(Path.Combine(assets, "textures", "cc_day_bright_and_saturated.png"));
			Textures.dirtLigher = LoadTexture(Path.Combine(assets, "textures", "dirt_lighter.png"));

			Log.Debug("loading set pieces");
			LoadSetpieces(bundle2, sharedAssetsBundle);
			Prefabs.forceFieldDome = bundle2.LoadAsset<GameObject>("Assets/Prefabs/Smaller Wider Dome.prefab");

			foreach (var asset in bundle.GetAllAssetNames())
			{
				Log.Debug(asset);
			}

			Log.Debug("loading UI");
			Prefabs.universalSidescreen = bundle.LoadAsset<GameObject>("Assets/Beached/UI/UniversalSidescreen_tmpconverted.prefab");
			Prefabs.critterIdentitySidescreen = bundle2.LoadAsset<GameObject>("Assets/UI/CritterIdentityScreen.prefab");
			Prefabs.critterIdentitySidescreen.gameObject.SetActive(false);
			Prefabs.critterIdentitySidescreen.AddOrGet<RectTransform>();
			Prefabs.muffinSideScreen = bundle2.LoadAsset<GameObject>("Assets/UI/MuffinSideScreen_tmpconverted.prefab");

			TMPConverter.ReplaceAllText(Prefabs.universalSidescreen);
			TMPConverter.ReplaceAllText(Prefabs.critterIdentitySidescreen);
			TMPConverter.ReplaceAllText(Prefabs.muffinSideScreen);

			// very important to do this as early as possible, if these references are not found Unity will CTD (native crash), with no log.
			var dropDownGo = Prefabs.muffinSideScreen.transform.Find("Scroll View/Viewport/Contents/FilterCategory/CritterFilterPrefab/Dropdown").gameObject;
			dropDownGo.SetActive(true);

			var dropdown = dropDownGo.AddComponent<TMP_Dropdown>();

			// when converting to LocText these references were lost, so they need to be rebound
			var item = dropdown.transform.Find("Template/Viewport/Content/Item");
			dropdown.itemText = item.Find("Item Label").GetComponent<LocText>();
			dropdown.itemImage = item.transform.Find("UIImage").GetComponent<Image>();
			dropdown.captionText = dropdown.transform.Find("Label").GetComponent<LocText>();
			dropdown.captionImage = dropdown.transform.Find("UIImage").GetComponent<Image>();
			dropdown.template = dropdown.transform.Find("Template").GetComponent<RectTransform>();
			dropdown.options = [];

			Object.DontDestroyOnLoad(dropDownGo);
			/*            Materials.germOverlayReplacer = new Material(bundle.LoadAsset<Shader>("Assets/Beached/D_GermOverlay.shader"));
                        Materials.forceField = new(bundle.LoadAsset<Shader>("Assets/Beached/ForceField.shader"))
                        {
                            renderQueue = RenderQueues.Liquid
                        };*/

			Materials.forceField = new(Shader.Find("Klei/BloomedParticleShader"));

			//Textures.germOverlays = bundle.LoadAsset<Texture2DArray>("Assets/Beached/Images/combined.png");

			Textures.forceFieldGrid = bundle.LoadAsset<Texture2D>("Assets/Beached/Images/grid_b.png");
			Textures.forceFieldBlurMap = bundle.LoadAsset<Texture2D>("Assets/Beached/Images/blurmap.png");
			// LoadSetpieces(bundle);

			Log.Debug("loading particle systems");
			Prefabs.cometTrailFx = bundle.LoadAsset<GameObject>("Assets/Beached/fx/CometSparkles.prefab");
			Log.AssertNotNull(Prefabs.cometTrailFx, "cometTrailFx");
			var renderer = Prefabs.cometTrailFx.GetComponent<ParticleSystemRenderer>();
			var texture = renderer.material.mainTexture;
			renderer.material = new Material(Shader.Find("Klei/BloomedParticleShader"))
			{
				renderQueue = RenderQueues.Liquid,
				mainTexture = texture
			};

			LoadAsteroidBelt(bundle2, sharedAssetsBundle);

			Materials.zoneTypeMaskMaterial = shadersBundle.LoadAsset<Material>("Assets/Shaders/BiomeMaskMaterial.mat");

			if (Materials.zoneTypeMaskMaterial == null)
				Log.Warning("zone type mat null");

			//Materials.darkVeil = shadersBundle.LoadAsset<Material>("Assets/Materials/Shader Graphs_DarkVeilShaderv2.mat");
			Fx.darkVeilOverlay = bundle2.LoadAsset<GameObject>("Assets/Prefabs/DarkVeilQuad.prefab");
			Fx.electricOverlay = bundle2.LoadAsset<GameObject>("Assets/Prefabs/ElectricityQuad.prefab");

			sw.Stop();
			Log.Info($"Finished loading assets. It took {sw.ElapsedMilliseconds} ms");
		}

		private static void LoadAsteroidBelt(AssetBundle bundle2, AssetBundle sharedBundle)
		{
			Prefabs.asteroidBelt = bundle2.LoadAsset<GameObject>("Assets/Prefabs/BeltContainer.prefab");

			var particles = Prefabs.asteroidBelt.transform.Find("AsteroidBelt").GetComponent<ParticleSystem>();

			var refs = Prefabs.asteroidBelt.AddComponent<HierarchyReferences>();
			refs.references = refs.references = [new ElementReference()
			{
				Name = "Particles",
				behaviour = particles
			}];

			var main = particles.main;
			main.startLifetime = 100_000;
			main.maxParticles = 300;
			main.playOnAwake = false;

			var renderer = particles.GetComponent<ParticleSystemRenderer>();
			renderer.material = new Material(Shader.Find("UI/Default"));
			renderer.material.SetTexture("_MainTex", sharedBundle.LoadAsset<Texture2D>("Assets/Textures/asteroids.png"));

			Prefabs.asteroidBelt.SetActive(false);
		}

		private static void LoadSetpieces(AssetBundle bundle, AssetBundle sharedAssetsBundle)
		{
			Prefabs.setpieces = [];
			var defaultShader = Shader.Find("Sprites/Default");
			var genericSingleSpriteVista = bundle.LoadAsset<GameObject>("Assets/Prefabs/GenericVista.prefab");
			var genericSpriteRenderer = genericSingleSpriteVista.transform.Find("Bg").GetComponent<SpriteRenderer>();

			genericSpriteRenderer.material = new Material(defaultShader);
			genericSpriteRenderer.sprite = Assets.GetSprite("unknown");

			Prefabs.setpieces.Add("generic", genericSingleSpriteVista);

			var beachSetPiece = bundle.LoadAsset<GameObject>("Assets/Prefabs/BeachVista.prefab");
			//beachSetPiece.GetComponent<Transform>().localScale *= 2f;
			//var setPieceMaterial = new Material(bundle.LoadAsset<Shader>("Assets/Beached/fx/parallax/BeachedParallax.shader"));

			/*			foreach (var renderer in beachSetPiece.GetComponents<SpriteRenderer>())
						{
							// reference to built in shaders are lost so reassigning it here
							if (renderer.gameObject.name == "bg 1")
							{
								var texture = renderer.material.mainTexture;
								renderer.material.shader = defaultShader;
							}

							renderer.material.renderQueue = RenderQueues.Liquid;
						}
			*/

			//beachSetPiece.transform.Find("bg 1").GetComponent<SpriteRenderer>().sprite = Assets.GetSprite("beached_beach_cave_outline");
			Prefabs.setpieces.Add("beach", beachSetPiece);
		}

		private static void LoadSounds(string path)
		{
			AudioUtil.LoadSound(Sounds.SHELL_CHIME_LOUD, Path.Combine(path, "511636__aslipasli__chimes.wav"));
			AudioUtil.LoadSound(Sounds.MUSSEL_SPROUT_HARVEST, Path.Combine(path, "250133__fngersounds__egg-cracking.wav"));
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

		public static void LoadKAnims()
		{
			Log.Debug("Loading KAnims");
			Stopwatch stopWatch = new();
			stopWatch.Start();


			var bundle = LoadAssetBundle("beached_kanim_assets", platformSpecific: false);



			stopWatch.Stop();
			Log.Info($"Finished loading Beached animations. It took {stopWatch.ElapsedMilliseconds} ms");
		}


		// just load all sprites from the folder
		// TODO: move to asset bundle
		public static void LoadSprites(Assets assets)
		{
			LoadSprites(assets, Path.Combine(Mod.folder, "assets", "sprites"));

			var driedOutSprite = LoadSprite("status_item_dryedout", ModAssets.Sprites.STATUSITEM_DRIEDOUT);
			assets.SpriteAssets.Add(driedOutSprite);
		}

		private static void LoadSprites(Assets __instance, string path)
		{
			if (!Directory.Exists(path))
			{
				Log.Warning("Sprites directory not found.");
				return;
			}

			Log.Info("Loading Sprite Assets...");

			foreach (var file in Directory.GetFiles(path, "*.png"))
			{
				var name = Path.GetFileNameWithoutExtension(file);
				var sprite = LoadSprite(file, name);
				__instance.SpriteAssets.Add(sprite);

				var metaPath = Path.Combine(Path.GetDirectoryName(path), name + ".meta.json");
				if (File.Exists(metaPath))
				{
					var json = File.ReadAllText(metaPath);
					if (json != null)
					{
						var data = JsonConvert.DeserializeObject<AssetMetaData>(json);
						if (data.TintedSprite)
						{
							__instance.TintedSpriteAssets.Add(new TintedSprite()
							{
								sprite = sprite,
								name = name,
								color = Util.ColorFromHex(data.ColorHex)
							});
						}
					}
				}
			}
		}

		public class AssetMetaData
		{
			public bool TintedSprite { get; set; }

			public string ColorHex { get; set; } = "FFFFFF";
		}

		private static Sprite LoadSprite(string path, string spriteName)
		{
			var texture = ModAssets.LoadTexture(path, true);

			if (texture == null)
			{
				return null;
			}

			var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector3.zero);
			sprite.name = spriteName;

			return sprite;
		}
	}
}
