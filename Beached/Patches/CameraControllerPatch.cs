using Beached.Content.Scripts;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
    public class CameraControllerPatch
    {
        // replaces the LUT with a more vibrant, colorful overlay
        [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
        public class CameraController_OnPrefabInit_Patch
        {
            private static Texture2D defaultDayColorCube;

            [HarmonyPriority(Priority.High)]
            public static void Prefix(CameraController __instance)
            {
                var useCustomLUT = BeachedWorldLoader.Instance.IsBeachedContentActive || Mod.settings.CrossWorld.UseVibrantLUTEverywhere;

                if (Global.Instance.GetComponent("RomenHRegistry") is IDictionary<string, object> RomenHRegistry)
                {
                    if (useCustomLUT)
                    {
                        RomenHRegistry["somekeyidk"] = ModAssets.Textures.LUTDay;
                    }
                    else
                    {
                        RomenHRegistry.Remove("somekeyidk");
                    }

                    return;
                }

                defaultDayColorCube ??= __instance.dayColourCube;

                __instance.dayColourCube = useCustomLUT
                    ? ModAssets.Textures.LUTDay
                    : defaultDayColorCube;
            }
        }
    }
}
