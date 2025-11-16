using Beached.Content;
using Beached.Content.Scripts;
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


		[HarmonyPatch(typeof(WattsonMessage), "OnActivate")]
		public class WattsonMessage_OnActivate_Patch
		{
			public static void Postfix()
			{
				Beached_Mod.Instance.Trigger(ModHashes.gameStartedForFirstTime);
			}
		}
	}
}
