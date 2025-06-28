using Beached.Content.Scripts;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class ImmigrationPatch
	{
		[HarmonyPatch(typeof(Immigration), "ConfigureCarePackages")]
		public class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(Immigration __instance)
			{
				DisallowCoalUntilSteelAge(__instance);
			}

			private static void DisallowCoalUntilSteelAge(Immigration __instance)
			{
				HashSet<string> coalPackages =
					[
						SimHashes.Carbon.ToString(),
						HatchConfig.ID,
						BabyHatchConfig.ID,
						HatchConfig.EGG_ID,
					];

				foreach (var package in __instance.carePackages)
				{
					if (coalPackages.Contains(package.id))
						AddRequirement(package, CoalOnAstropalegosCondition);
				}
			}

			private static void AddRequirement(CarePackageInfo package, Func<bool> condition)
			{
				var fieldInfo = typeof(CarePackageInfo).GetField("requirement");
				var existingRequirement = fieldInfo.GetValue(package) as Func<bool>;

				fieldInfo.SetValue(package, existingRequirement + condition);
			}

			private static bool CoalOnAstropalegosCondition()
			{
				return !Beached_WorldLoader.Instance.IsBeachedContentActive
					|| DiscoveredResources.Instance.IsDiscovered(SimHashes.Steel.CreateTag());
			}
		}
	}
}
