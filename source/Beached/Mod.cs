global using Beached.Utils;
using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Scripts;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
using Neutronium.PostProcessing.LUT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Beached
{
	public class Mod : UserMod2
	{
		public static Components.Cmps<Beached_PlushiePlaceable> plushiePlaceables = new();

		public const string STATIC_ID = "Beached";
#if DEBUG
		public static bool debugMode = true;
#else
		public static bool debugMode = false;
#endif

		public static Config settings = new();

		public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

		public static bool isFastTrackHere;
		public static bool isCritterTraitsRebornHere;
		public static bool isTwitchHere;

		public static LUT_API lutAPI;

		public static Harmony harmonyInstance;

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);

#if !ELEMENTS
            Log.Warning("UNSTABLE BUILD: This build was compiled with no ELEMENTS. All elements disabled, attempted worldgen will fail.");
#endif
#if !TRANSPILERS
			Log.Warning("UNSTABLE BUILD: This build was compiled with no TRANSPILERS");
#endif

			BTags.OnModLoad();

			BeachedDevTools.Initialize();
			BWorldGenTags.Initialize();

			lutAPI = LUT_API.Setup(harmony, true);
			harmonyInstance = harmony;

			ProcessAttributes(harmony);
		}

		private static void ProcessAttributes(Harmony harmony)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			var types = Assembly.GetExecutingAssembly().GetTypes();

			foreach (var type in types)
			{
				foreach (var methodInfo in type.GetMethods())
				{
					foreach (Attribute attr in Attribute.GetCustomAttributes(methodInfo))
					{
						var attributeType = attr.GetType();

						if (attributeType == typeof(OnModLoadedAttribute))
						{
							methodInfo.Invoke(null, null);
							continue;
						}

						if (attributeType == typeof(DbEntryAttribute))
						{
							var parameters = methodInfo.GetParameters();

							if (parameters == null || parameters.Length == 0)
								Log.Warning($"Cannot insert Db entries to {type.Name}.{methodInfo.Name}, no parameter given");

							var targetType = methodInfo.GetParameters()[0].ParameterType;
							var targetConstructor = AccessTools.Constructor(targetType, new Type[] { typeof(ResourceSet) })
								?? AccessTools.Constructor(targetType, new Type[] { });

							if (targetConstructor == null)
							{
								Log.Warning("Constructor not found for " + targetType.FullName);
								continue;
							}

							harmony.Patch(targetConstructor, postfix: new HarmonyMethod(methodInfo));
						}
					}
				}
			}

			stopWatch.Stop();
			Log.Debug($"Processed attributes in {stopWatch.ElapsedMilliseconds} ms");
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);

			foreach (var modEntry in mods)
			{
				if (modEntry.IsEnabledForActiveDlc())
				{
					switch (modEntry.staticID)
					{
						case "TrueTiles":
							Integration.TrueTiles.OnAllModsLoaded();
							break;
						case "PeterHan.FastTrack":
							isFastTrackHere = true;
							break;
						case "DecorPackA":
							Integration.DecorPackI.RegisterTiles();
							break;
						case "CritterTraitsReborn":
							isCritterTraitsRebornHere = true;
							Integration.CritterTraitsReborn.Initialize();
							break;
					}
				}
			}

			if (!isCritterTraitsRebornHere)
				Patches.SimpleInfoScreenPatch.Patch(harmony);

			Integration.CritterShedding.Initialize();
		}
	}
}
