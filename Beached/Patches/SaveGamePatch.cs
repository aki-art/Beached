using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;
namespace Beached.Patches
{
    public class SaveGamePatch
    {
        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                __instance.gameObject.AddOrGet<IridescenceEffect>();
                __instance.gameObject.AddOrGet<ElementInteractions>();
                __instance.gameObject.AddOrGet<BeachedTutorials>();
            }
        }
    }
}
