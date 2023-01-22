using Beached.Content.BWorldGen;
using Beached.ModDevTools;
using HarmonyLib;
namespace Beached.Patches.Worldgen
{
    public class OfflineWorldGenPatch
    {
        [HarmonyPatch(typeof(OfflineWorldGen), "DoWorldGenInitialize")]
        public class OfflineWorldGen_DoWorldGenInitialize_Patch
        {
            public static bool Prefix(OfflineWorldGen __instance)
            {
                return __instance.gameObject.AddOrGet<BeachedOfflineWorldGen>().OnDoWorldGenInitialize();
            }
        }

        [HarmonyPatch(typeof(OfflineWorldGen), "CanLoadSave")]
        public class OfflineWorldGen_CanLoadSave_Patch
        {
            public static void Postfix(OfflineWorldGen __instance)
            {
                WorldGenDevTool.offlineWorldGen = __instance;
            }
        }
    }
}
