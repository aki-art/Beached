using Beached.Content;
using Beached.Content.Scripts.Items;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class EggConfigPatch
	{
		[HarmonyPatch(typeof(EggConfig), nameof(EggConfig.CreateEgg),
		[
			typeof(string),		// id
			typeof(string),		// name
			typeof(string),		// desc
			typeof(Tag),		// creature_id
			typeof(string),		// anim
			typeof(float),		// mass
			typeof(int),		// egg_sort_order
			typeof(float),		// base_incubation_rate
			typeof(string[]),	// requiredDlcIds
			typeof(string[]),	// forbiddenDlcIds
			typeof(bool),		// preventEggDrops
			typeof(float),		// eggMassToDrop
		])]
		public class EggConfig_CreateEgg_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				// karacoos want to sit on these
				__result.AddTag(BTags.karacooSittable);

				// for karacoos. Pickupable is already IApproachable but allows too much interaction range
				__result.AddOrGet<Approachable>();

				// allow genetic modification
				__result.AddOrGet<Beached_GeneticallyModifiableEgg>();
			}
		}
	}
}
