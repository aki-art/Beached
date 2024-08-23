using FMODUnity;
using HarmonyLib;
using System.Linq;

namespace Beached.Content.ModDb
{
	internal class BSongs
	{

		[HarmonyPatch(typeof(Game), "OnSpawn")]
		public class Game_OnSpawn_Patch
		{
			public static void Prefix()
			{
				var manager = MusicManager.instance;
				EventReference oceanPalace = RuntimeManager.PathToEventReference("event:/beached/Music/ocean_palace");

				if (manager.stingers.Any(stinger => !stinger.fmodEvent.IsNull && stinger.fmodEvent.Guid == oceanPalace.Guid))
					return;

				manager.stingers = manager.stingers.AddToArray(new MusicManager.Stinger()
				{
					fmodEvent = oceanPalace,
					requiredDlcId = DlcManager.VANILLA_ID
				});
			}
		}

		public static void OnConfigureSongsPre(MusicManager manager)
		{
		}
	}
}
