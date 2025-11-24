using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Patches
{
	public class MinionBrowserScreenPatch
	{
		[HarmonyPatch(typeof(MinionBrowserScreen), "PopulateGallery")]
		public class MinionBrowserScreen_PopulateGallery_Patch
		{
			public static void Postfix(MinionBrowserScreen __instance)
			{
				foreach (Transform gridItem in __instance.galleryGridContent)
				{
					if (gridItem.TryGetComponent(out HierarchyReferences refs))
					{
						if (refs.TryGetReference<Image>("Icon", out var icon)
							&& icon != null
							&& icon.sprite?.name == "dreamIcon_BEACHED_MINNOW")
						{
							var dlcIcon = gridItem.Find("DlcBanner").GetComponent<Image>();
							if (dlcIcon != null)
							{
								dlcIcon.gameObject.SetActive(true);

								dlcIcon.sprite = Assets.GetSprite("beached_tech_banner");
								dlcIcon.color = Color.white;

								if (gridItem.TryGetComponent(out ToolTip tooltip))
									tooltip.SetSimpleTooltip(STRINGS.UI.BEACHED_MISC.DUPE_BEACHED);
							}
						}
					}
				}
			}
		}
	}
}
