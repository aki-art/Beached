using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class KAnimGroupFilePatch
	{
		[HarmonyPatch(typeof(KAnimGroupFile), nameof(KAnimGroupFile.Load))]
		public class KAnimGroupFile_Load_Patch
		{
			public static void Prefix(KAnimGroupFile __instance)
			{
				var groups = __instance.GetData();

				AddGroup(groups, CONSTS.BATCH_TAGS.SWAPS,
				[
					"beached_poffmouth_mouth_kanim",
					"beached_rubberboots_kanim",
/*					"beached_zeolite_necklace_kanim",
					"beached_maxixe_necklace_kanim",
					"beached_hematite_necklace_kanim",*/
					"minnow_head_kanim"
				]);

				AddGroup(groups, CONSTS.BATCH_TAGS.INTERACTS,
				[
					"beached_spinner_interact_kanim",
					"beached_glaciermuffinsunlock_kanim",
					//"beached_skeletonunlock_kanim"
				]);
			}

			private static void AddGroup(List<KAnimGroupFile.Group> groups, int hash, HashSet<HashedString> items)
			{
				var swapAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(hash));

				// remove the wrong group
				groups.RemoveAll(g => items.Contains(g.animNames[0]));

				foreach (var item in items)
				{
					// readd to correct group
					var anim = Assets.GetAnim(item);

					swapAnimsGroup.animFiles.Add(anim);
					swapAnimsGroup.animNames.Add(anim.name);
				}
			}
		}
	}
}
