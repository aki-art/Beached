using Beached.Content.Codex;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace Beached.Patches
{
    internal class CodexCachePatch
    {
        public const string MODS = "MODS";

        [HarmonyPatch(typeof(CodexCache), "CodexCacheInit")]
        public class CodexCache_CodexCacheInit_Patch
        {
            public static void Postfix()
            {
                BeachedCodexEntries.Generate();
            }
        }

        [HarmonyPatch(typeof(CodexCache), "CollectEntries")]
        public static class CodexCache_CollectEntries_Patch
        {
            public static void Postfix(string folder, List<CodexEntry> __result)
            {
                if (folder == "")
                {
                    var extraEntries = CodexCache.CollectEntries(Path.Combine(Mod.folder, "codex", "Creatures"));
                    __result.AddRange(extraEntries);
                }
            }
        }
    }
}
