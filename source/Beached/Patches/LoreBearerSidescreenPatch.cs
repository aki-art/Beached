using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	internal class LoreBearerSidescreenPatch
	{

		[HarmonyPatch(typeof(LoreBearerSideScreen), "IsValidForTarget")]
		public class LoreBearerSideScreen_IsValidForTarget_Patch
		{
			public static void Postfix(GameObject target, ref bool __result)
			{
				if (__result && target.TryGetComponent(out WorldgenOnlyLoreBearer single))
				{
					__result = single.hasLore;
				}
			}
		}
	}
}
