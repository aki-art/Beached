using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Beached.Utils;
using HarmonyLib;
using Klei.AI;
using UnityEngine;

namespace Beached.Patches
{
    public class MinionStartingStatsPatch
    {
        [HarmonyPatch(typeof(MinionStartingStats), "CreateBodyData")]
        public class MinionStartingStats_CreateBodyData_Patch
        {
            public static void Postfix(Personality p, ref KCompBuilder.BodyData __result)
            {
                BDuplicants.ModifyBodyData(p, ref __result);
            }
        }

        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public class MinionStartingStats_GenerateTraits_Patch
        {
            public static void Prefix(MinionStartingStats __instance)
            {
                BDuplicants.OnTraitRoll(__instance);
            }

            public static void Postfix(MinionStartingStats __instance)
            {
                if (Mod.settings.CrossWorld.LifeGoals || Beached_WorldLoader.Instance.IsBeachedContentActive)
                {
                    var trait = BTraits.GetGoalForPersonality(__instance.personality);

                    var ext = __instance.GetExtension();
                    ext.SetLifeGoal(trait);

                    // Always add 2 morale
                    ext.AddLifeGoalReward(Db.Get().Attributes.QualityOfLife.Id, CONSTS.DUPLICANTS.LIFEGOALS.MORALBONUS);

                    // Add 3-5 more of their already present aptitudes
                    foreach (var aptitude in __instance.skillAptitudes)
                    {
                        foreach (var skill in aptitude.Key.relevantAttributes)
                        {
                            ext.AddLifeGoalReward(skill.Id, Random.Range(3, 6));
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(MinionStartingStats), "Apply")]
        public class MinionStartingStats_Apply_Patch
        {
            public static void Postfix(GameObject go, MinionStartingStats __instance)
            {
                var goalTrait = __instance.GetLifeGoalTrait();

                if (goalTrait == null)
                {
                    return;
                }

                Log.Debug(go.GetProperName());
                if (go.TryGetComponent(out Traits traits))
                {
                    Log.Debug("Added traits ");
                    Debug.Assert(go.GetComponent<Beached_LifeGoalTracker>() != null, "go.GetComponent<BeachedMinionStorage>()");
                    Debug.Assert(__instance.GetLifeGoalAttributes() != null, "__instance.GetLifeGoalAttributes()");
                    go.GetComponent<Beached_LifeGoalTracker>().AddAttributes(__instance.GetLifeGoalAttributes());
                    traits.Add(goalTrait);
                }
                else
                {

                    Log.Debug("no traits ");
                }
                // add custom data
            }
        }
    }
}
