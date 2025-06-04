using Beached.Content.Codex;
using HarmonyLib;

namespace Beached.Patches
{
	public class CodexCachePatch
	{
		public const string MODS = "MODS";

		[HarmonyPatch(typeof(CodexCache), nameof(CodexCache.CodexCacheInit))]
		public class CodexCache_CodexCacheInit_Patch
		{
			public static void Postfix()
			{
				BeachedCodexEntries.Generate();
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix()
			{
				//FoodGraphGenerator.Generate(Path.Combine(FUtility.Utils.ModPath, "food_graphs.txt"));
			}
		}
	}
}
