using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
    public class PropertyTexturesPatch
    {
        // Makes the Salty Oxygen texture the lighter texture Oxygen uses
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
                return (Grid.Element[cell].id == Elements.SaltyOxygen) ? (byte)0 : (byte)existingValue;
            }
        }
    }
}
