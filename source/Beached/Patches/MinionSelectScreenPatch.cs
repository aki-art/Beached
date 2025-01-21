
#if !NO_MINNOW
using Beached.Content.Defs.Duplicants;
using HarmonyLib;
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

				if (WorldgenUtil.IsCurrentBeachedWorld())
				{
					((CharacterContainer)minionSelectScreen.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get(MinnowConfig.ID)));
					((CharacterContainer)minionSelectScreen.containers[1]).GenerateCharacter(true);
					((CharacterContainer)minionSelectScreen.containers[0]).GenerateCharacter(true);
				}
			}
		}
	}
}
#endif