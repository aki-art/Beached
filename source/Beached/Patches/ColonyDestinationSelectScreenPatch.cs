using Beached.Content.ModDb;
using Beached.ModDevTools;
using HarmonyLib;
using Klei.CustomSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Patches
{
	public class ColonyDestinationSelectScreenPatch
	{
		[HarmonyPatch(typeof(ColonyDestinationSelectScreen), "QualitySettingChanged")]
		public class ColonyDestinationSelectScreen_QualitySettingChanged_Patch
		{
			public static void Postfix(ColonyDestinationSelectScreen __instance, SettingConfig config, SettingLevel level)
			{
#if TRANSPILERS
				if (config.id == CustomGameSettingConfigs.ClusterLayout.id)
				{
					if (WorldgenUtil.IsBeachedWorld(level.id))
					{
						__instance.storyContentPanel.SetStoryState(BStories.Glaciers.Id, StoryContentPanel.StoryState.Guaranteed);
						SetDim(__instance.storyContentPanel.storyRows[BStories.Glaciers.Id], true);
					}
					else
					{
						__instance.storyContentPanel.SetStoryState(BStories.Glaciers.Id, StoryContentPanelPatch.userChosenState);
						SetDim(__instance.storyContentPanel.storyRows[BStories.Glaciers.Id], false);
					}
				}

				if (config.id == BStories.Glaciers.Id)
				{
					var clusterId = __instance.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout.id);

					if (!WorldgenUtil.IsBeachedWorld(clusterId))
						StoryContentPanelPatch.userChosenState = level.id == "Guaranteed"
							? StoryContentPanel.StoryState.Guaranteed
							: StoryContentPanel.StoryState.Forbidden;
				}
#endif
			}

			private static void SetDim(GameObject gameObject, bool dim)
			{
				Log.Debug("set dim pre");
				if (gameObject.TryGetComponent(out HierarchyReferences hierarchyReferences))
				{
					var icon = hierarchyReferences.GetReference<Image>("Icon");

					hierarchyReferences.GetReference<MultiToggle>("checkbox").gameObject.SetActive(!dim);

					Log.Debug("beached_lockicon");
					if (!hierarchyReferences.TryGetReference<Image>("beached_lockicon", out var lockIcon) && dim)
					{
						var go = Object.Instantiate(icon);
						go.name = "Beached_LockIcon";
						go.sprite = Assets.GetSprite("beached_storytrait_locked_overlay");
						go.GetComponent<LayoutElement>().ignoreLayout = true;
						var rect = go.GetComponent<RectTransform>();

						rect.pivot = new Vector2(0.5f, 0.5f);

						Log.Debug("setting parent");
						go.transform.SetParent(icon.transform.parent, false);
						Log.Debug("set parent");
#if DEVTOOLS

						TempDevTool.lockIcon = go.transform;
#endif
						go.transform.localPosition = new Vector3(179f, 0, 0);
						go.transform.localScale = 1.35f * Vector3.one;

						lockIcon = go.GetComponent<Image>();

						hierarchyReferences.references = hierarchyReferences.references.AddToArray(new ElementReference()
						{
							Name = "beached_lockicon",
							behaviour = lockIcon
						});
					}

					if (lockIcon != null)
						lockIcon.gameObject.SetActive(dim);
				}
			}
		}
	}
}
