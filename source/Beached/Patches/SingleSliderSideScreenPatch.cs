using Beached.Content.Defs.Buildings;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class SingleSliderSideScreenPatch
	{
		[HarmonyPatch(typeof(SingleSliderSideScreen), nameof(SingleSliderSideScreen.IsValidForTarget))]
		public class SingleSliderSideScreen_IsValidForTarget_Patch
		{
			public static void Postfix(GameObject target, ref bool __result)
			{
				__result &= !target.HasTag(AmmoniaGeneratorConfig.ID);
			}
		}
	}
}
