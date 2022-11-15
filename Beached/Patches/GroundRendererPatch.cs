using Beached.Content.Scripts;
using Beached.Utils;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
    public class GroundRendererPatch
    {
        [HarmonyPatch(typeof(GroundRenderer), "OnShadersReloaded")]
        public class GroundRenderer_OnShadersReloaded_Patch
        {
            public static void Postfix(Dictionary<SimHashes, MockStructs.Materials> ___elementMaterials)
            {
                IridescenceEffect.groundRendererMaterials = ___elementMaterials;
            }
        }
    }
}
