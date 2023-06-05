global using Beached.Utils;
using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
using Neutronium.PostProcessing.LUT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Beached
{
	public class Mod : UserMod2
	{
		public const string STATIC_ID = "Beached";

		public static bool debugMode = true;

		public static Config settings = new();

		public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

		public static bool isFastTrackHere;
		public static bool isCritterTraitsRebornHere;
		public static bool isTwitchHere;

		public static LUT_API lutAPI;

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);

#if !ELEMENTS
            Log.Warning("UNSTABLE BUILD: This build was compiled with no ELEMENTS. All elements disabled, attempted worldgen will fail.");
#endif

			BTags.OnModLoad();

			ZoneTypes.Initialize();
			BeachedDevTools.Initialize();
			BWorldGenTags.Initialize();

			lutAPI = LUT_API.Setup(harmony, true);

			var types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (var type in types)
			{
				foreach (var methodInfo in type.GetMethods())
				{
					foreach (Attribute attr in Attribute.GetCustomAttributes(methodInfo))
					{
						if (attr.GetType() == typeof(OnModLoadedAttribute))
						{
							methodInfo.Invoke(null, null);
						}
					}
				}
			}
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
			{
				Patches.SimpleInfoScreenPatch.Patch(harmony);
			}
		}
	}
}
