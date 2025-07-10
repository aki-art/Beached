using Beached.Content.Overlays;
using HarmonyLib;
using UnityEngine;

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
					infoUnits =
					[
						new OverlayLegend.OverlayInfoUnit(
							Assets.GetSprite("unknown"),
							"STRINGS.UI.OVERLAYS.PIPPLANTING.DESCRIPTION",
							Color.white,
							Color.white)
					],
					isProgrammaticallyPopulated = true,
					mode = ElementInteractionsOverlayMode.ID,
					name = "STRINGS.UI.OVERLAYS.PIPPLANTING.NAME",
				});
			}
		}
	}
}
