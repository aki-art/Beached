using Beached.Content.ModDb;
using Beached.Content.Overlays;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class OverlayMenuPatch
	{
		[HarmonyPatch(typeof(OverlayMenu), nameof(OverlayMenu.InitializeToggles))]
		public static class OverlayMenu_InitializeToggles_Patch
		{
			public static void Postfix(List<KIconToggleMenu.ToggleInfo> ___overlayToggleInfos)
			{
				var menu = new OverlayMenu.OverlayToggleInfo(
					STRINGS.UI.OVERLAY.BEACHED_CONDUCTIONOVERLAY.NAME,
					"status_item_interference",
					ConductionOverlayMode.ID,
					"",
					Action.NumActions,
					STRINGS.UI.OVERLAY.BEACHED_CONDUCTIONOVERLAY.TOOLTIP
				);

				___overlayToggleInfos.Add(menu);

				var debugMenu = new OverlayMenu.OverlayToggleInfo(
					$"<b>{STRINGS.UI.OVERLAY.BEACHED_FLOWOVERLAY.BUTTON}</b>",
					"overlay_beached_flow",
					Beached_FlowOverlayMode.ID,
					BTechItems.FLOW_OVERLAY_TECH,
					Action.NumActions,
					STRINGS.UI.OVERLAY.BEACHED_FLOWOVERLAY.TOOLTIP
				);

				___overlayToggleInfos.Add(debugMenu);
			}
		}
	}
}
