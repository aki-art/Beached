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

				Log.Debug($"drecko_kanim {new HashedString("drecko_kanim").hash:D}");
				Log.Debug($"drecko {new HashedString("drecko").hash:D}");
				Log.Debug($"Assets/anim/drecko {new HashedString("Assets/anim/drecko").hash:D}");

				var drecko = Assets.GetAnim("drecko_kanim");
				var dreckoGroup = KAnimGroupFile.GetGroup("drecko_kanim");

				foreach (var group in groups)
				{
					Log.Debug($"group {group.id.hash:D} {group.animNames.Join(a => HashCache.Get().Get(a))}");
				}

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

				/*				AddGroup(groups, -2012462048,
								[
									"beached_test_morph_kanim"
								]);*/
			}

			private static int GetBatchTagOf(string animName)
			{
				var anim = Assets.GetAnim(animName);

				if (anim == null)
				{
					Log.Warning($"NO ANIM WITH NAME {animName}");
					return -1;
				}

				return anim.batchTag.HashValue;
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
