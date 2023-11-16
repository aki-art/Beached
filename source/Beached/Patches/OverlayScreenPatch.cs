using Beached.Content.Overlays;
using HarmonyLib;

namespace Beached.Patches
{
	public class OverlayScreenPatch
	{
		[HarmonyPatch(typeof(OverlayScreen), nameof(OverlayScreen.RegisterModes))]
		public static class OverlayScreen_RegisterModes_Patch
		{
			public static void Postfix()
			{
				OverlayScreen.Instance.RegisterMode(new ConductionOverlayMode());
				OverlayScreen.Instance.RegisterMode(new ElementInteractionsOverlayMode());
			}
		}
	}
}
