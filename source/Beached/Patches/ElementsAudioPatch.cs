using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class ElementsAudioPatch
	{
#if ELEMENTS
		[HarmonyPatch(typeof(ElementsAudio), nameof(ElementsAudio.LoadData))]
		public class ElementsAudio_LoadData_Patch
		{
			public static void Postfix(ElementsAudio __instance)
			{
				Elements.CreateAudioConfigs(__instance);
			}
		}
#endif
	}
}
