using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class MusicManagerPatch
	{
		[HarmonyPatch(typeof(MusicManager), "ConfigureSongs")]
		public class MusicManager_ConfigureSongs_Patch
		{
			public static void Prefix(MusicManager __instance)
			{
				BSongs.OnConfigureSongsPre(__instance);
			}
		}

	}
}
