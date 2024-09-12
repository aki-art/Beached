using HarmonyLib;

namespace Beached.Patches
{
	public class AudioSheetsPatch
	{
		[HarmonyPatch(typeof(AudioSheets), "Initialize")]
		public class AudioSheets_Initialize_Patch
		{
			public static void Prefix(AudioSheets __instance)
			{
				__instance.sheets.Add(new AudioSheet()
				{
					defaultType = "SoundEvent",
					soundInfos =
					[
						Single("beached_mussel_sprout_kanim", "harvest", "Beached_MusselSprout_Crack", 7)
					]
				});

				__instance.sheets.Add(new AudioSheet()
				{
					defaultType = "LoopingSoundEvent",
					soundInfos =
					[
						Loop("beached_chime_kanim", "mild", "Beached_Chime_Loop", 1716),
						Loop("beached_chime_kanim", "medium", "Beached_Chime_Loop", 1716),
						Loop("beached_chime_kanim", "hard", "Beached_Chime_Loop", 1716)
					]
				});
			}

			private static AudioSheet.SoundInfo Single(string file, string anim, string eventName, int frame)
			{
				return new AudioSheet.SoundInfo()
				{
					File = file,
					Anim = anim,
					Frame0 = frame,
					Name0 = eventName,
					RequiredDlcId = DlcManager.VANILLA_ID
				};
			}


			private static AudioSheet.SoundInfo Loop(string file, string anim, string eventName, float minInterval, int frame = 0)
			{
				return new AudioSheet.SoundInfo()
				{
					File = file,
					Anim = anim,
					MinInterval = minInterval,
					Frame0 = frame,
					Name0 = eventName,
					RequiredDlcId = DlcManager.VANILLA_ID
				};
			}
		}
	}
}
