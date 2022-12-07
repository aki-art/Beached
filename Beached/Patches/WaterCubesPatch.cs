using HarmonyLib;

namespace Beached.Patches
{
    public class WaterCubesPatch
    {
        // make the liquids a little more see-through
        [HarmonyPatch(typeof(WaterCubes), "Init")]
        public class WaterCubes_Init_Patch
        {
            public static void Postfix(WaterCubes __instance)
            {
                __instance.material.SetFloat("_BlendScreen", 0.67f);
            }
        }
    }
}
