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
        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public class MinionStartingStats_GenerateTraits_Patch
        {
            public static void Postfix(MinionStartingStats __instance)
            {
                if (Mod.settings.CrossWorld.LifeGoals || BeachedWorldLoader.Instance.IsBeachedContentActive)
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
                    Debug.Assert(go.GetComponent<BeachedLifeGoalTracker>() != null, "go.GetComponent<BeachedMinionStorage>()");
                    Debug.Assert(__instance.GetLifeGoalAttributes() != null, "__instance.GetLifeGoalAttributes()");
                    go.GetComponent<BeachedLifeGoalTracker>().AddAttributes(__instance.GetLifeGoalAttributes());
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
