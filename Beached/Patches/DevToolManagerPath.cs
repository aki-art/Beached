using HarmonyLib;

namespace Beached.Patches
{
    public class DevToolManagerPath
    {
        [HarmonyPatch(typeof(DevToolManager), MethodType.Constructor)]
        public class DevToolManager_Ctor_Patch
        {
            public static void Postfix(DevToolManager __instance)
            {
            }
        }
    }
}
