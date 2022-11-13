using Beached.Content.Scripts;
using HarmonyLib;
namespace Beached.Patches
{
    public class GlobalPatch
    {
        [HarmonyPatch(typeof(Global), "Awake")]
        public class Global_Awake_Patch
        {
            public static void Postfix(Global __instance)
            {
                __instance.gameObject.AddOrGet<WorldManager>();
            }
        }
    }
}
