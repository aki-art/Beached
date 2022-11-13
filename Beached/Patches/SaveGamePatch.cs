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
                Log.Debug("SaveGame OnPrefabInit");
                __instance.gameObject.AddOrGet<BeachedWorldManager>();
            }
        }
    }
}
