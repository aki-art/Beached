using FMODUnity;
using HarmonyLib;

namespace Beached.Patches
{
	public class GlobalAssetsPatch
	{
		[HarmonyPatch(typeof(GlobalAssets), "OnPrefabInit")]
		public class GlobalAssets_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				if (RuntimeManager.StudioSystem.getBank("bank:/Beached", out var beachedBank) == FMOD.RESULT.OK)
				{
					beachedBank.getEventList(out var events);
					foreach (var @event in events)
					{
						@event.getPath(out var path);
						// the game nomally cuts off the unique pathing but it is a nonsensical
						// design that causes only pain and no gain so we are not doing that
						//GlobalAssets.SoundTable[path] = path;
					}

					RuntimeManager.StudioSystem.getBankList(out var banks);
					Log.Debug("BANKS:");
					foreach (var bnk in banks)
					{
						bnk.getPath(out var path);
						Log.Debug(path);
					}
					Log.Info($"Registered {events.Length} sound effects");
				}
				else
				{
					Log.Warning("Could not load beached FMOD bank! :(");
				}
			}
		}
	}
}
