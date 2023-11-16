using Beached.Content.Overlays;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class StatusItemPatch
	{
		[HarmonyPatch(typeof(StatusItem), "GetStatusItemOverlayBySimViewMode")]
		public static class StatusItem_GetStatusItemOverlayBySimViewMode_Patch
		{
			public static void Prefix(Dictionary<HashedString, StatusItem.StatusItemOverlays> ___overlayBitfieldMap)
			{
				if (!___overlayBitfieldMap.ContainsKey(ConductionOverlayMode.ID))
					___overlayBitfieldMap.Add(ConductionOverlayMode.ID, StatusItem.StatusItemOverlays.None);

				if (!___overlayBitfieldMap.ContainsKey(ElementInteractionsOverlayMode.ID))
					___overlayBitfieldMap.Add(ElementInteractionsOverlayMode.ID, StatusItem.StatusItemOverlays.None);
			}
		}
	}
}
