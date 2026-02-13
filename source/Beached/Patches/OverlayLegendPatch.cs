using Beached.Content.Overlays;
using HarmonyLib;

namespace Beached.Patches
{
	public class OverlayLegendPatch
	{
		[HarmonyPatch(typeof(OverlayLegend), "OnSpawn")]
		public class OverlayLegend_OnSpawn_Patch
		{
			public static void Prefix(OverlayLegend __instance)
			{
				__instance.overlayInfoList.Add(new OverlayLegend.OverlayInfo()
				{
					infoUnits = [],
					isProgrammaticallyPopulated = true,
					mode = Beached_FlowOverlayMode.ID,
					name = "Beached.STRINGS.UI.OVERLAY.BEACHED_FLOWOVERLAY.NAME",
				});
			}
		}
	}
}
