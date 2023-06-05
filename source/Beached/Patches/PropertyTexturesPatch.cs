using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
    public class PropertyTexturesPatch
    {

        [HarmonyPatch(typeof(PropertyTextures), "OnReset")]
        public class PropertyTextures_OnReset_Patch
        {
            public static void Postfix(PropertyTextures __instance)
            {
                ModAssets.Materials.liquidRefractionMat.SetTexture(
                    "_LiquidTex", 
                    __instance.externallyUpdatedTextures[(int)PropertyTextures.Property.Liquid]);
            }
        }

#if ELEMENTS
        // Makes the Salty Oxygen texture the lighter texture Oxygen uses
        // TODO transpiler
        [HarmonyPatch(typeof(PropertyTextures), "UpdateDanger")]
        public static class PropertyTextures_UpdateDanger_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var index = codes.FindIndex(ci => ci.LoadsConstant((int)SimHashes.Oxygen)); /// 34 ldc.i4 SimHashes.Oxygen

                if (index == -1)
                {
                    return codes;
                }

                var GetDangerForElement = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch), "GetDangerForElement", new[] { typeof(int), typeof(int) });

                codes.InsertRange(index + 3, new[]
                {
                    // byte.maxValue is loaded to the stack
                    new CodeInstruction(OpCodes.Ldloc_2), // load num to the stack
                    new CodeInstruction(OpCodes.Call, GetDangerForElement)
                });

                return codes;
            }

            // Calling with existing value so there is a possibility for other mods to also add their own values
            private static byte GetDangerForElement(int existingValue, int cell)
            {
                return (Grid.Element[cell].id == Elements.saltyOxygen) ? (byte)0 : (byte)existingValue;
            }
        }
#endif
    }
}
