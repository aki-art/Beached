using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class ManualGeneratorConfigPatch
	{
		[HarmonyPatch(typeof(ManualGeneratorConfig), "DoPostConfigureComplete")]
		public class ManualGeneratorConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(GameObject go)
			{
				var upTime = CONSTS.CYCLE_LENGTH;
				ModAPI.ExtendPrefabToLubricatable(go, 10f, 10f / upTime, true);
			}
		}
	}
}
