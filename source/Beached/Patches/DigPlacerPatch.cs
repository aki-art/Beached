using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class DigPlacerPatch
	{
		[HarmonyPatch(typeof(DigPlacerConfig), "CreatePrefab")]
		public class DigPlacerConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<TreasureHolder>();
				__result.AddOrGet<Beached_DiggableSkillMod>().requiredSkillPerk = BSkillPerks.CAN_FIND_TREASURES_ID;
			}
		}
	}
}
