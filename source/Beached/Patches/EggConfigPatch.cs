using Beached.Content.Scripts.Items;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class EggConfigPatch
	{
		[HarmonyPatch(typeof(EggConfig), nameof(EggConfig.CreateEgg))]
		public class EggConfig_CreateEgg_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<Beached_GeneticallyModifiableEgg>();
			}
		}
	}
}
