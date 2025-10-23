using FMODUnity;
using HarmonyLib;
using System.Linq;

namespace Beached.Content.ModDb
{
	public class BSongs
	{
		public static void OnConfigureSongsPre(MusicManager manager)
		{
			var oceanPalace = RuntimeManager.PathToEventReference("event:/beached/Music/ocean_palace");

			var result = RuntimeManager.StudioSystem.getEventByID(oceanPalace.Guid, out var ev2);

			Log.Debug($"{result} {ev2.isValid()}");

			if (manager.stingers.Any(stinger => !stinger.fmodEvent.IsNull && stinger.fmodEvent.Guid == oceanPalace.Guid))
				return;

			manager.stingers = manager.stingers.AddToArray(new MusicManager.Stinger()
			{
				fmodEvent = oceanPalace,
				requiredDlcId = DlcManager.VANILLA_ID
			});

			Log.Debug($"past");

		}
	}
}
