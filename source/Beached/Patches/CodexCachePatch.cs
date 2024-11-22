using Beached.Content;
using Beached.Content.Codex;
using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class CodexCachePatch
	{
		public const string MODS = "MODS";


		[HarmonyPatch(typeof(CodexEntryGenerator_Elements), "GetElementEntryContext")]
		public class CodexEntryGenerator_Elements_GetElementEntryContext_Patch
		{
			public static void Prefix(ref bool __state)
			{
				__state = CodexEntryGenerator_Elements.contextInstance == null;
			}

			public static void Postfix(ref CodexEntryGenerator_Elements.ElementEntryContext __result, ref bool __state)
			{
				if (!__state)
					return;

				foreach (var lubricant in Elements.lubricantStrengths.Keys)
				{
					var entry = __result.usedMap.map.GetOrAdd(lubricant.CreateTag(), () => []);

					foreach (var building in Assets.GetPrefabsWithComponent<Lubricatable>())
					{
						var applyTo = new HashSet<ElementUsage>();
						var lubricatable = building.GetComponent<Lubricatable>();
						var mucusUse = new ElementUsage(lubricant.CreateTag(), lubricatable.massPerUseOrPerSecond, true)
						{
							customFormating = (tag, amount, continous) => GetMucusFormatting(tag, amount, lubricatable)
						};

						applyTo.Add(mucusUse);

						entry.Add(new CodexEntryGenerator_Elements.ConversionEntry
						{
							title = building.GetProperName(),
							prefab = Assets.GetPrefab(building.PrefabID()),
							inSet = applyTo,
							outSet = []
						});
					}
				}
			}

			private static string GetMucusFormatting(Tag tag, float amount, Lubricatable lubricatable)
			{
				var mass = lubricatable.isTimedUse
					? GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerSecond)
					: GameUtil.GetFormattedMass(amount);

				return lubricatable.isTimedUse ? mass : $"{mass}/use";
			}
		}

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

			/*		private static string FormatUse(Tag tag, float amount, bool continous)
					{
						return $""
					}*/
		}
	}
}