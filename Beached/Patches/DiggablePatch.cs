using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
    public class DiggablePatch
    {
        [HarmonyPatch(typeof(Diggable), "OnStopWork")]
        public class Diggable_OnStopWork_Patch
        {
            public static void Prefix(Diggable __instance, bool ___isDigComplete, Element ___originalDigElement)
            {
                Log.Debug("dig finish");
                if (___isDigComplete && __instance.worker is Worker worker)
                {
                    Log.Debug("worker");
                    var cell = Grid.PosToCell(__instance);

                    if (TreasureChances.treasureChances.TryGetValue(___originalDigElement.id, out var treasureTable))
                    {
                        Log.Debug("treasure " + ___originalDigElement.id + " " + treasureTable.treasures.Count);
                        var resume = worker.GetComponent<MinionResume>();
                        if (resume != null && resume.HasPerk(BSkillPerks.CanFindTreasures))
                        {
                            Log.Debug("CanFindTreasures");
                            treasureTable.OnExcavation(__instance, cell, ___originalDigElement, resume);
                        }
                    }
                }
            }
        }
    }
}
