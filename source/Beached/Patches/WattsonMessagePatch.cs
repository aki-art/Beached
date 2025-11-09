using Beached.Content.Scripts.UI;
using HarmonyLib;

namespace Beached.Patches
{
	internal class WattsonMessagePatch
	{
		// add links to the welcome message
		[HarmonyPatch(typeof(WattsonMessage), "OnPrefabInit")]
		public class WattsonMessage_OnPrefabInit_Patch
		{
			public static void Postfix(WattsonMessage __instance)
			{
				__instance.message.gameObject.AddOrGet<Beached_WelcomeMessageLinkParser>();
			}
		}
	}
}
