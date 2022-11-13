using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
    public class TESTPATCHES
    {
        [HarmonyPatch(typeof(GroundRenderer), "ConfigureMaterialShine")]
        public class GroundRenderer_ConfigureMaterialShine_Patch
        {
            public static bool Prefix(Material material)
            {
                if(!material.HasProperty("_ShineMask"))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
