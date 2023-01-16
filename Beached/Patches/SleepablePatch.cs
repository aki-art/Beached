using Beached.Content.Scripts;
using HarmonyLib;
namespace Beached.Patches
{
    public class SleepablePatch
    {
        [HarmonyPatch(typeof(Sleepable), "OnPrefabInit")]
        public class Sleepable_OnPrefabInit_Patch
        {
            public static void Postfix(Sleepable __instance)
            {
                if (__instance.GetComponent<Assignable>() != null)
                {
                    __instance.gameObject.AddComponent<TargetOfGoalTracker>().targetTag = MechanicalSurfboardConfig.ID;
                    __instance.gameObject.AddComponent<TargetOfGoalTracker>().targetTag = ItemPedestalConfig.ID;
                }
            }
        }
    }
}
