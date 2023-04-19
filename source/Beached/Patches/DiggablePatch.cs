using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
    public class DiggablePatch
    {
        [HarmonyPatch(typeof(Diggable), "OnSpawn")]
        public class Diggable_OnSpawn_Patch
        {
            public static void Prefix(Diggable __instance)
            {
                __instance.gameObject.AddOrGet<TreasureHolder>();
            }
        }
    }
}
