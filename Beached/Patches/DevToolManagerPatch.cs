#if DEBUG
using HarmonyLib;

namespace Beached.Patches
{
    public class DevToolManagerPatch
    {
        [HarmonyPatch(typeof(DevToolManager), "UpdateShouldShowTools")]
        public class DevToolManager_UpdateShouldShowTools_Patch
        {
            public static void Postfix(ref bool ___showImGui)
            {
                ___showImGui = true;
            }
        }
    }
}
#endif