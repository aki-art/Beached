using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace Beached.Patches
{
	public class MinionStartingStatsPatch
	{
		[HarmonyPatch(typeof(MinionStartingStats), nameof(MinionStartingStats.CreateBodyData))]
		public class MinionStartingStats_CreateBodyData_Patch
		{
			public static void Postfix(Personality p, ref KCompBuilder.BodyData __result)
			{
				BDuplicants.ModifyBodyData(p, ref __result);
			}
		}

		[HarmonyPatch(typeof(MinionStartingStats), nameof(MinionStartingStats.GenerateTraits))]
		public class MinionStartingStats_GenerateTraits_Patch
		{
			public static void Prefix(MinionStartingStats __instance)
			{
				Log.Debug("ROLLING TRAITS");
				foreach (var trait in DUPLICANTSTATS.GOODTRAITS)
				{
					Log.Debug($"{trait.id} {trait.rarity}");
				}
				BDuplicants.OnTraitRoll(__instance);
			}

			public static void Postfix(MinionStartingStats __instance)
			{
				ModAPI.ApplyLifeGoalFromPersonality(__instance, false);
			}
		}

		[HarmonyPatch(typeof(MinionStartingStats), nameof(MinionStartingStats.Apply))]
		public class MinionStartingStats_Apply_Patch
		{
			public static void Postfix(GameObject go, MinionStartingStats __instance)
			{
				var goalTrait = __instance.GetLifeGoalTrait();

				if (goalTrait == null)
					return;

				if (go.TryGetComponent(out Traits traits))
				{
					go.GetComponent<Beached_LifeGoalTracker>().AddAttributes(__instance.GetLifeGoalAttributes());
					traits.Add(goalTrait);
				}

				// TODO: add custom data
			}
		}
	}
}
