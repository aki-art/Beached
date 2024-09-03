using Beached.Content.BWorldGen;
using Beached.Content.Defs.Duplicants;
using HarmonyLib;
using Klei.CustomSettings;
using ProcGen;
using System.Collections;

namespace Beached.Patches
{
	public class MinionSelectScreenPatch
	{
		[HarmonyPatch(typeof(MinionSelectScreen), nameof(MinionSelectScreen.OnPrefabInit))]
		public class MinionSelectScreen_OnPrefabInit_Patch
		{
			private static MinionSelectScreen minionSelectScreen;

			public static void Postfix(MinionSelectScreen __instance)
			{
				minionSelectScreen = __instance;
				__instance.StartCoroutine(SetDefaultMinionsRoutine());
			}

			private static IEnumerator SetDefaultMinionsRoutine()
			{
				yield return SequenceUtil.WaitForNextFrame;

				var currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);

				if (SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id).clusterTags.Contains(BWorldGenTags.BeachedCluster.ToString()))
				{
					((CharacterContainer)minionSelectScreen.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get(MinnowConfig.ID)));
					((CharacterContainer)minionSelectScreen.containers[1]).GenerateCharacter(true);
					((CharacterContainer)minionSelectScreen.containers[0]).GenerateCharacter(true);
				}
			}
		}
	}
}
