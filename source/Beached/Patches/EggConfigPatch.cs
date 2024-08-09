using Beached.Content.Scripts.Items;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class EggConfigPatch
	{
		[HarmonyPatch(typeof(EggConfig), nameof(EggConfig.CreateEgg),
		[
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(Tag),
			typeof(string),
			typeof(float),
			typeof(int),
			typeof(float),
			typeof(string[]),
		])]
		public class EggConfig_CreateEgg_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<Beached_GeneticallyModifiableEgg>();
			}
		}
	}
}
