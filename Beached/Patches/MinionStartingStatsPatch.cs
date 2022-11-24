using Beached.Content.ModDb;
using HarmonyLib;
namespace Beached.Patches
{
    public class MinionStartingStatsPatch
    {

        //[HarmonyPatch(typeof(MinionStartingStats), "GenerateAttributes")]
        public class MinionStartingStats_GenerateAttributes_Patch
        {
            public static void Prefix(MinionStartingStats __instance)
            {
                if (!__instance.StartingLevels.ContainsKey(BAttributes.PRECISION_ID))
                {
                    __instance.StartingLevels[BAttributes.PRECISION_ID] = 0;
                }
            }
        }
    }
}
