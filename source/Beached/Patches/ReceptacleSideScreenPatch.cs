using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class ReceptacleSideScreenPatch
	{
		[HarmonyPatch(typeof(ReceptacleSideScreen), "IsValidForTarget")]
		public class ReceptacleSideScreen_IsValidForTarget_Patch
		{
			public static void Postfix(GameObject target, ref bool __result)
			{
				if (__result && target.TryGetComponent(out CrystalGrower _))
					__result = false;
			}
		}
	}
}
