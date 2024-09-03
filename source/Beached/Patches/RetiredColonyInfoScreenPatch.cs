using Beached.Content.ModDb;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class RetiredColonyInfoScreenPatch
	{
		[HarmonyPatch(typeof(RetiredColonyInfoScreen), "PopulateAchievements")]
		public class RetiredColonyInfoScreen_PopulateAchievements_Patch
		{
			public static void Postfix(RetiredColonyInfoScreen __instance)
			{
				foreach (var entry in __instance.achievementEntries)
				{
					if (BAchievements.beachedAchievements.Contains(entry.Key))
					{
						if (entry.Value.TryGetComponent(out HierarchyReferences hierarchyReferences))
						{
							var dlcOverlay = hierarchyReferences.GetReference<KImage>("dlc_overlay");

							dlcOverlay.gameObject.SetActive(true);
							dlcOverlay.sprite = Assets.GetSprite("beached_dlc_banner_large");
							dlcOverlay.color = Color.white;
						}
					}
				}
			}
		}
	}
}
