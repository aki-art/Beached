using Beached.Content.Scripts.Entities;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class CrabConfigPatch
	{
		[HarmonyPatch(typeof(CrabConfig), nameof(CrabConfig.CreatePrefab))]
		public class CrabConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				__result.AddComponent<LimpetHost>();
			}
		}
	}
}
