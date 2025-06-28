using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	[HarmonyPatch(typeof(CreatureCalorieMonitor.Def), "GetDescriptors")]
	public class CreatureCalorieMonitor_Def_GetDescriptors_Patch
	{
		public static void Postfix(GameObject obj, ref List<Descriptor> __result)
		{
			if (obj.TryGetComponent(out AdditionalPoopTags tags))
				tags.ModifyCalorieDescriptors(ref __result);
		}
	}

	[HarmonyPatch(typeof(CreatureCalorieMonitor.Stomach), "Poop")]
	public static class CreatureCalorieMonitor_Stomach_Poop_Patch
	{
		public static void Prefix(CreatureCalorieMonitor.Stomach __instance)
		{
			__instance.owner.Trigger(ModHashes.prePoop);
		}
	}
}
