using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class TreeFilterableSideScreenPatch
	{
		[HarmonyPatch(typeof(TreeFilterableSideScreen), nameof(TreeFilterableSideScreen.IsValidForTarget))]
		public class TreeFilterableSideScreen_IsValidForTarget_Patch
		{
			public static void Postfix(GameObject target, ref bool __result)
			{
				__result &= !target.TryGetComponent(out SimpleFlatFilterable _);
			}
		}
	}
}
