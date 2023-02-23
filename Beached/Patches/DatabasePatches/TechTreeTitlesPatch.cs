using Beached.Content.ModDb;
using Database;
using HarmonyLib;

namespace Beached.Patches.DatabasePatches
{
    public class TechTreeTitlesPatch
    {
        [HarmonyPatch(typeof(TechTreeTitles), "Load")]
        public class TechTreeTitles_Load_Patch
        {
            public static void Postfix(TechTreeTitles __instance)
            {
                BTechTreeTitles.Register(__instance);
            }
        }
    }
}
