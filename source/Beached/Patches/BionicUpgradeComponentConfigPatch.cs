using Beached.Content.Defs;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	public class BionicUpgradeComponentConfigPatch
	{
		[HarmonyPatch(typeof(BionicUpgradeComponentConfig), "CreatePrefabs")]
		public class BionicUpgradeComponentConfig_CreatePrefabs_Patch
		{
			public static void Postfix(BionicUpgradeComponentConfig __instance, ref List<GameObject> __result)
			{
				ExtraBionicUpgradeComponentConfig.AddPrefabs(__instance, __result);
			}
		}
	}
}
