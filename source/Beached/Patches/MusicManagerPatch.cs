using HarmonyLib;

namespace Beached.Patches
{
	public class MusicManagerPatch
	{
		[HarmonyPatch(typeof(MusicManager), "ConfigureSongs")]
		public class MusicManager_ConfigureSongs_Patch
		{
			public static void Postfix(MusicManager __instance)
			{
				Log.Debug("SONGS QUEUED:");
				foreach (var song in __instance.songMap)
				{
					Log.Debug(song.Key);
				}
			}
		}
	}
}
