using Beached.Content.Scripts;
using HarmonyLib;
using Neutronium.PostProcessing.LUT;
using System.Collections.Generic;
using UnityEngine;
using static Beached.ModAssets;

namespace Beached.Patches
{
    public class CameraControllerPatch
    {

        [HarmonyPatch(typeof(CameraController), "OnSpawn")]
        public class CameraController_OnSpawn_Patch
        {
            public static void Postfix(CameraController __instance)
            {
                Beached_Mod.Instance.InitWaterCamera(__instance.baseCamera);
            }
        }

        // replaces the LUT with a more vibrant, colorful overlay
        [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
        public class CameraController_OnPrefabInit_Patch
        {
            [HarmonyPriority(Priority.High)]
            public static void Prefix(CameraController __instance)
            {
                var useCustomLUT = Beached_WorldLoader.Instance.IsBeachedContentActive || Mod.settings.CrossWorld.UseVibrantLUTEverywhere;

                if (useCustomLUT)
                {
                    Mod.lutAPI.RegisterLUT("Beached_VibrantDayLUT", LUTSlot.Day, 300, Textures.LUTDay);
                }
                else
                {
                    Mod.lutAPI.UnregisterLUT("Beached_VibrantDayLUT");
                }
            }
        }
    }
}
