using Database;
using HarmonyLib;
using Klei.CustomSettings;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class BStories
	{
		public static Story Glaciers;

		[DbEntry]
		public static void Register(Stories __instance)
		{
			Glaciers = __instance.Add(new Story(
				"Beached_Glaciers",
				"storytraits/Beached_Glaciers",
				7,
				7,
				51,
				"beached/storytraits/muffinglacerb").SetKeepsake("keepsake_megabrain"));

			Log.Debug("LOADED STORY TRAITS");
			foreach (var resource in __instance.resources)
			{
				Log.Debug(resource.Id);
			}
			__instance.resources.Sort();
		}


		[HarmonyPatch(typeof(CustomGameSettings), "VerifySettingsDictionary", typeof(Dictionary<string, SettingConfig>))]
		public class CustomGameSettings_VerifySettingsDictionary_Patch
		{
			public static void Prefix(Dictionary<string, SettingConfig> configs)
			{
				if (configs == null)
					return;

				foreach (var config in configs)
				{
					Log.Debug(config.Key);
					Log.Debug($"\t{config.Value.coordinate_dimension}");
					Log.Debug($"\t{config.Value.coordinate_dimension_width}");

					List<SettingLevel> levels = config.Value.GetLevels();
					if (levels == null)
						continue;

					foreach (var level in levels)
					{
						Log.Debug($"\t\t{level.id} {level.coordinate_offset}");
					}
				}
			}
		}
	}
}
