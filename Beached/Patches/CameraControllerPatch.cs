using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
    public class CameraControllerPatch
    {
        [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
        public class CameraController_OnPrefabInit_Patch
        {
            public static void Postfix(CameraController __instance)
            {
                // TODO: check for LUT not incuded
                // TODO: only in beached worlds
                if (BeachedWorldManager.Instance.IsBeachedContentActive)
                {
                    __instance.dayColourCube = ModAssets.Textures.LUTDay;
                }
            }
        }
    }
}
