using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class ResearchEntryPatch
	{
		[HarmonyPatch(typeof(ResearchEntry), nameof(ResearchEntry.SetTech))]
		public class ResearchEntry_SetTech_Patch
		{
			public static void Postfix(ResearchEntry __instance)
			{
				var index = 1; //child 0 is the icon prefab

				var container = __instance.iconPanel.transform;

				foreach (var unlockedItem in __instance.targetTech.unlockedItems)
				{
					if (!Game.IsCorrectDlcActiveForCurrentSave(unlockedItem))
						continue;

					var child = container.GetChild(index++);

					if (child == null)
						continue;

					if (!unlockedItem.Id.StartsWith("Beached_"))
						continue;

					if (!child.TryGetComponent<HierarchyReferences>(out var references))
						continue;

					var dlcOverlay = references.GetReference<KImage>("DLCOverlay");
					dlcOverlay.gameObject.SetActive(true);
					dlcOverlay.sprite = Assets.GetSprite("beached_tech_banner");
					dlcOverlay.color = Color.white;

					if (child.TryGetComponent<ToolTip>(out var toolTip))
						// TODO STRING
						toolTip.toolTip = $"{unlockedItem.Name}\n{unlockedItem.description}\n\n{STRINGS.UI.BEACHED_MISC.BEACHED_CONTENT}";
				}
			}
		}
	}
}
