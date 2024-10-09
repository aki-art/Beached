using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class HydrogenGeneratorConfigPatch
	{
		[HarmonyPatch(typeof(HydrogenGeneratorConfig), "DoPostConfigureComplete")]
		public class HydrogenGeneratorConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(GameObject go)
			{
				var upTime = CONSTS.CYCLE_LENGTH;
				ModAPI.ExtendPrefabToLubricatable(go, 10f, 10f / upTime, true);
			}
		}
	}
}
