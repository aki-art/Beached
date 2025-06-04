using Beached.Content;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class CodexEntryGenerator_ElementsPatch
	{
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

				AddPoffEntries(__result);
				AddLubricatableEntries(__result);
			}

			private static void AddLubricatableEntries(CodexEntryGenerator_Elements.ElementEntryContext __result)
			{
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

			private static void AddPoffEntries(CodexEntryGenerator_Elements.ElementEntryContext __result)
			{
				var poffShroom = Assets.GetPrefab(PoffShroomConfig.ID);

				foreach (var poffEntry in PoffConfig.configs)
				{
					var elementTag = poffEntry.elementID.CreateTag();
					var entry = __result.usedMap.map.GetOrAdd(elementTag, () => []);
					var use = new ElementUsage(elementTag, PoffShroomConfig.GAS_CONSUMPTION_PER_SECOND_WILD, true)
					{
						customFormating = (tag, amount, continous) => GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerCycle)
					};

					var output = new ElementUsage(poffEntry.rawID, 1, true)
					{
						customFormating = (tag, amount, continous) => GameUtil.GetFormattedCaloriesForItem(poffEntry.rawID, 1f, GameUtil.TimeSlice.PerCycle)
					};

					var poff = Assets.GetPrefab(poffEntry.rawID);

					entry.Add(new CodexEntryGenerator_Elements.ConversionEntry
					{
						title = poffShroom.GetProperName(),
						prefab = poffShroom,
						inSet = [use],
						outSet = [output]
					});
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

	}
}
