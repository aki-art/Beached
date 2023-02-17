using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
    public class DevToolMenuNodeListPatch
    {
        // nesting doesnt work in base game, because they use Path methods, which are system dependent
        // so when on windows they try to aplit a path by /, nothing happens, because the path is separated by \-s.
        //[HarmonyPatch(typeof(DevToolMenuNodeList), "AddOrGetParentFor")]
        public class DevToolMenuNodeList_AddOrGetParentFor_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var index = codes.FindIndex(ci => ci.opcode == OpCodes.Stloc_2);

                if (index == -1)
                {
                    return codes;
                }

                var m_ReplacementMethod = AccessTools.Method(typeof(DevToolMenuNodeList_AddOrGetParentFor_Patch), "ReplacementMethod", new[]
                {
                    typeof(string[]),
                    typeof(string)
                });

                codes.InsertRange(index, new[]
                {
                    // string[] is on stack
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, m_ReplacementMethod)
                });

                return codes;
            }

            private static string[] ReplacementMethod(string[] pathComponents, string childPath)
            {
                Log.Debug("looking at path: " + childPath);

                if (childPath.IsNullOrWhiteSpace())
                {
                    return pathComponents;
                }

                if (childPath.StartsWith("Mods/Beached/"))
                {
                    var newSplit = childPath.Split('/');
                    var newPathComponents = new string[newSplit.Length - 1];
                    for (int i = 0; i < newPathComponents.Length; i++)
                    {
                        Log.Debug("-  split: " + newSplit[i]);
                        newPathComponents[i] = newSplit[i];
                    }

                    return newPathComponents;
                }

                return pathComponents;
            }
        }
    }
}
