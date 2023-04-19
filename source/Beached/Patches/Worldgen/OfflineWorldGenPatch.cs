using Beached.ModDevTools;
using HarmonyLib;
namespace Beached.Patches.Worldgen
{
    public class OfflineWorldGenPatch
    {
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
