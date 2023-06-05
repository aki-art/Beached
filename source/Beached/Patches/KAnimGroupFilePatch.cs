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

				var swaps = new HashSet<HashedString>()
				{
					"beached_poffmouth_mouth_kanim",
					"beached_rubberboots_kanim",
					"beached_zeolite_necklace_kanim",
					"minnow_head_kanim"
				};

				var swapAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(CONSTS.BATCH_TAGS.SWAPS));

				// remove the wrong group
				groups.RemoveAll(g => swaps.Contains(g.animNames[0]));

				foreach (var swap in swaps)
				{
					// readd to correct group
					var anim = Assets.GetAnim(swap);

					swapAnimsGroup.animFiles.Add(anim);
					swapAnimsGroup.animNames.Add(anim.name);
				}
			}
		}
	}
}
