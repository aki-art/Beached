using Beached.Content.ModDb;
using HarmonyLib;
using Klei.CustomSettings;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public class StoryContentPanelPatch
	{
		public static StoryContentPanel.StoryState userChosenState;


		[HarmonyPatch(typeof(StoryContentPanel), "GetTraitsString")]
		public class StoryContentPanel_GetTraitsString_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				// find injection point
				var index = codes.FindIndex(ci => ci.LoadsConstant(5));

				if (index == -1)
					return codes;

				var m_GetMaxTraitCount = AccessTools.DeclaredMethod(typeof(StoryContentPanel_GetTraitsString_Patch), "GetMaxTraitCount");

				// inject right after the found index
				codes.InsertRange(index + 1, [

					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_GetMaxTraitCount)
				]);

				return codes;
			}

			private static int GetMaxTraitCount(int maxTraits, StoryContentPanel instance)
			{
				var clusterId = instance.mainScreen.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
				return clusterId == CONSTS.BEACHED_CLUSTER_SETTING_ID ? maxTraits + 1 : maxTraits;
			}
		}

		[HarmonyPatch(typeof(StoryContentPanel), "IncrementStorySetting")]
		public class StoryContentPanel_IncrementStorySetting_Patch
		{
			public static bool Prefix(StoryContentPanel __instance, string storyId, ref bool __state)
			{
				// we skip because we want nothing to happen, including side effects from mods, effectively disabling the checkbox effect
				if (storyId == BStories.Glaciers.Id)
				{
					var clusterId = __instance.mainScreen.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
					if (clusterId == CONSTS.BEACHED_CLUSTER_SETTING_ID)
					{
						__state = true;
						return false;
					}
				}

				return true;
			}

			public static void Postfix(StoryContentPanel __instance, ref bool __state)
			{
				var glacierOnBeached = __state;

				// save state only when we didn't set it by force
				if (!glacierOnBeached)
				{
					userChosenState = __instance.GetActiveStories().Contains(BStories.Glaciers.Id)
						? StoryContentPanel.StoryState.Guaranteed
						: StoryContentPanel.StoryState.Forbidden;
				}
			}
		}

		[HarmonyPatch(typeof(StoryContentPanel), "SelectRandomStories")]
		public class StoryContentPanel_SelectRandomStories_Patch
		{
			public static void Postfix(StoryContentPanel __instance, bool useBias)
			{
				var clusterId = __instance.mainScreen.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);

				if (clusterId == CONSTS.BEACHED_CLUSTER_SETTING_ID)
				{
					if (__instance.storyStates[BStories.Glaciers.Id] == StoryContentPanel.StoryState.Guaranteed)
						return;

					var disableThis = __instance.storyStates
						.Where(pair => pair.Value == StoryContentPanel.StoryState.Guaranteed)
						.Select(pair => pair.Key)
						.GetRandom();

					__instance.SetStoryState(BStories.Glaciers.Id, StoryContentPanel.StoryState.Guaranteed);
					__instance.SetStoryState(disableThis, StoryContentPanel.StoryState.Forbidden);
				}
			}
		}
	}
}
