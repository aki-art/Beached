using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
    public class EdiblePatch
    {
        [HarmonyPatch(typeof(Edible), "AddOnConsumeEffects")]
        public class Edible_AddOnConsumeEffects_Patch
        {
            public static void Postfix(Edible __instance, Worker worker)
            {
                if(__instance.HasTag(BTags.palateCleanserFood))
                {
                    if(worker != null && worker.TryGetComponent(out Beached_MinionStorage minionStorage))
                    {
                        minionStorage.OnPalateCleansed();
                    } 
                }
            }
        }
    }
}
