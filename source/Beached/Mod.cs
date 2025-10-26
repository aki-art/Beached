﻿global using Beached.Utils;
using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Defs.Entities;
using Beached.Content.Scripts;
using Beached.Integration;
using Beached.ModDevTools;
using Beached.Settings;
using Beached.Utils.GlobalEvents;
using HarmonyLib;
using KMod;
using Neutronium.PostProcessing.LUT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static Beached.Patches.DebugPatches.DebugPatches;

namespace Beached
{
	public class Mod : UserMod2
	{
		public const string STATIC_ID = "Beached";
		public static Integrations integrations = new();

#if DEBUG
		public static bool debugMode = true;
		public static bool speedStuffUp = true; // for debugging
#else
		public static bool debugMode = false;
		public static bool speedStuffUp = false; // for debugging
#endif

		public static Config settings = new();

		public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

		public static LUT_API lutAPI;
		public static Harmony harmonyInstance;
		internal static bool drawDebugGuides;

		public override void OnLoad(Harmony harmony)
		{

			Mod.harmony = harmony;
			//Log.Debug($"loading assembly from {Path.Combine(Mod.folder, "lib", "BeachedUnityBridge.dll")}");
			//Assembly.LoadFrom(Path.Combine(folder, "lib", "BeachedUnityBridge.dll"));

			base.OnLoad(harmony);

			BTags.OnModLoad();
#if DEVTOOLS
			BeachedDevTools.Initialize();
#endif
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
					foreach (var attr in Attribute.GetCustomAttributes(methodInfo))
					{
						var attributeType = attr.GetType();

						if (attributeType == typeof(SubscribeAttribute))
						{
							var parameters = methodInfo.GetParameters();
							if (parameters.Length != 1 || parameters[0].ParameterType != typeof(bool))
							{
								Log.Warning($"Subscribe attribute expects a single bool parameter! ({methodInfo.Name})");
								continue;
							}

							if (methodInfo.ReturnType != typeof(void))
							{
								Log.Warning($"Subscribe attribute does not expect a return value! ({methodInfo.Name})");
								continue;
							}

							var action = (Action<bool>)Delegate.CreateDelegate(typeof(Action<bool>), methodInfo);
							Beached_WorldLoader.onWorldReloaded += action;

							continue;
						}

						if (attributeType == typeof(DbEntryAttribute))
						{
							var parameters = methodInfo.GetParameters();

							if (parameters == null || parameters.Length == 0)
								Log.Warning($"Cannot insert Db entries to {type.Name}.{methodInfo.Name}, no parameter given");

							var targetType = methodInfo.GetParameters()[0].ParameterType;
							var targetConstructor = AccessTools.Constructor(targetType, [typeof(ResourceSet)])
								?? AccessTools.Constructor(targetType, []);

							if (targetConstructor == null)
							{
								Log.Warning("Constructor not found for " + targetType.FullName);
								continue;
							}

							harmony.Patch(targetConstructor, postfix: new HarmonyMethod(methodInfo));
						}

						if (attributeType == typeof(OverrideAttribute))
						{
							var patch = new HarmonyMethod(methodInfo);
							var overrideAttr = (OverrideAttribute)attr;

							var targetMethod = overrideAttr.parameters == null
								? type.BaseType.GetMethod(methodInfo.Name)
								: type.BaseType.GetMethod(methodInfo.Name, ((OverrideAttribute)attr).parameters);

							if (overrideAttr.postfix)
								harmony.Patch(targetMethod, postfix: patch);
							else
								harmony.Patch(targetMethod, prefix: patch);
						}
					}
				}
			}

			stopWatch.Stop();
			Log.Debug($"Processed attributes in {stopWatch.ElapsedMilliseconds} ms");
		}

		public static bool beginLogging = false;
		private static Harmony harmony;

		private static void PrefixLog()
		{
			try
			{
				var stackTrace = new StackTrace();
				Log.Debug(stackTrace.GetFrame(1).GetMethod().FullDescription());
			}
			catch (Exception e)
			{
				Log.Debug($"Could not log {e.StackTrace} {e.Message}");
			}
		}


		//[HarmonyPatch(typeof(MainMenu), "OnPrefabInit")]
		public class MinionStartingStats_GenerateStats_Patch
		{
			public static void Prefix()
			{
				if (beginLogging)
					return;

				beginLogging = true;

				var types2 = Assembly.GetExecutingAssembly().GetTypes();
				var prefix = new HarmonyMethod(AccessTools.DeclaredMethod(typeof(Mod), nameof(PrefixLog)));

				foreach (var type in types2)
				{

					var methods = type.GetMethods();
					foreach (var method in methods)
					{
						try
						{
							if (method.Name != "OnLoad" && method.Name != "PrefixLog" && method.Name != "Debug" && method.DeclaringType != typeof(Debug_Assert_Patch))
								harmony.Patch(method, prefix);
						}
						catch { }
					}
				}

			}
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			base.OnAllModsLoaded(harmony, mods);
			BEntities.ConfigureCrops();
			integrations.OnAllModsLoaded(harmony, mods);

			if (!integrations.IsModPresent(Integrations.MOONLET))
				throw new MoonletMissingException();
		}
	}
}
