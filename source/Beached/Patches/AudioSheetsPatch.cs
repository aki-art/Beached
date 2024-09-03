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
				Log.Debug("initializing autidosheets");
				foreach (var sheet in __instance.sheets)
				{
					Log.Debug($"defaultType: {sheet.defaultType} {sheet.soundInfos.Length}");
					foreach (var info in sheet.soundInfos)
					{
						if (info.File == "hatch_kanim")
						{
							Log.Debug(info.Anim);
							Log.Debug($"\t1 {info.Name1} {info.Frame1}");
							Log.Debug($"\t2 {info.Name2} {info.Frame2}");
							Log.Debug($"\t3 {info.Name3} {info.Frame3}");
							Log.Debug($"\t4 {info.Name4} {info.Frame4}");
							Log.Debug($"\t5 {info.Name5} {info.Frame5}");
							Log.Debug($"\t6 {info.Name6} {info.Frame6}");
							Log.Debug($"\t7 {info.Name7} {info.Frame7}");
							Log.Debug($"\t8 {info.Name8} {info.Frame8}");
							Log.Debug($"\t9 {info.Name9} {info.Frame9}");
							Log.Debug($"\t10 {info.Name10} {info.Frame10}");
							Log.Debug($"\t11 {info.Name11} {info.Frame11}");
						}
					}
				}

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
