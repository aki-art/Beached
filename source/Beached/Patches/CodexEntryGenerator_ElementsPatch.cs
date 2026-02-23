using Beached.Content;
using Beached.Content.Codex;
using Beached.Content.Defs.Entities.Critters.Mites;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities.AI;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using static Beached.Content.ModDb.LootTables;

namespace Beached.Patches
{
	public class CodexEntryGenerator_ElementsPatch
	{
		//[HarmonyPatch(typeof(CodexEntryGenerator_Elements), "GenerateMadeAndUsedContainers")]
		public class CodexEntryGenerator_Elements_GenerateMadeAndUsedContainers_Patch
		{
			private static HashSet<string> processedRecipes;

			public static void Postfix(Tag tag, List<ContentContainer> containers)
			{
				if (!Beached_Mod.Instance.treasury.chances.TryGetSource(tag, out var source))
					return;

				var contents = new ContentContainer([Beached_ArcheologyCodexWidget.Generate(tag, source)], ContentContainer.ContentLayout.Vertical);

				containers.Add(new ContentContainer(
				[
					new CodexSpacer(),
					new CodexCollapsibleHeader(STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME, contents)
				], ContentContainer.ContentLayout.Vertical));

				containers.Add(contents);

				processedRecipes = [];

				foreach (var container in containers)
				{
					if (container.content == null || container.content.Count < 2)
						continue;

					var inputs = container.content[0];

					var recipePanels = new List<CodexRecipePanel>();

					if (inputs is ContentContainer ingredientsContainer)
					{
						foreach (var content in ingredientsContainer.content)
						{
							if (content is CodexRecipePanel recipePanel)
							{
								if (recipePanel.complexRecipe != null)
								{
									if (recipePanel.complexRecipe.ingredients != null)
									{
										foreach (var ingredient in recipePanel.complexRecipe.ingredients)
										{
											if (ingredient.possibleMaterials != null && ingredient.possibleMaterials.Length > 1)
											{
												Log.Debug($"RECIPE WITH MULTI INPUT: {recipePanel.complexRecipe.id}");
											}
										}
									}
								}
							}
						}
					}
				}
			}

			// TEMPORARY
			public static bool Prefix(Tag tag, List<ContentContainer> containers)
			{
				var recipeCache = new HashSet<string>();

				var content1 = new List<ICodexWidget>();
				var content2 = new List<ICodexWidget>();

				foreach (var recipe in ComplexRecipeManager.Get().recipes)
				{
					if (recipeCache.Contains(recipe.results.Join(r => r.material.ToString())))
						continue;

					if (Game.IsCorrectDlcActiveForCurrentSave(recipe) && !recipe.IsAnyProductDeprecated())
					{
						if ((recipe.ingredients).Any(i => i.material == tag))
							content1.Add(new CodexRecipePanel(recipe));

						if ((recipe.results).Any((i => i.material == tag)))
							content2.Add(new CodexRecipePanel(recipe, true));

						recipeCache.Add(recipe.results.Join(r => r.material.ToString()));

						Log.Debug("added cache: " + recipe.results.Join(r => r.material.ToString()));
					}
				}





				// -------------------
				List<CodexEntryGenerator_Elements.ConversionEntry> conversionEntryList1;
				if (CodexEntryGenerator_Elements.GetElementEntryContext().usedMap.map.TryGetValue(tag, out conversionEntryList1))
				{
					foreach (var conversionEntry in conversionEntryList1)
						content1.Add(new CodexConversionPanel(conversionEntry.title, conversionEntry.inSet.ToArray<ElementUsage>(), conversionEntry.outSet.ToArray<ElementUsage>(), conversionEntry.prefab, conversionEntry.aidIcon1));
				}

				List<CodexEntryGenerator_Elements.ConversionEntry> conversionEntryList2;
				if (CodexEntryGenerator_Elements.GetElementEntryContext().madeMap.map.TryGetValue(tag, out conversionEntryList2))
				{
					foreach (var conversionEntry in conversionEntryList2)
						content2.Add(new CodexConversionPanel(conversionEntry.title, conversionEntry.inSet.ToArray<ElementUsage>(), conversionEntry.outSet.ToArray<ElementUsage>(), conversionEntry.prefab, conversionEntry.aidIcon1));
				}

				var contents1 = new ContentContainer(content1, ContentContainer.ContentLayout.Vertical);
				var contents2 = new ContentContainer(content2, ContentContainer.ContentLayout.Vertical);
				if (content1.Count > 0)
				{
					containers.Add(new ContentContainer(new List<ICodexWidget>()
				  {
					 new CodexSpacer(),
					 new CodexCollapsibleHeader((string) global::STRINGS.CODEX.HEADERS.ELEMENTCONSUMEDBY, contents1)
				  }, ContentContainer.ContentLayout.Vertical));
					containers.Add(contents1);
				}

				if (content2.Count > 0)
				{
					containers.Add(new ContentContainer(new List<ICodexWidget>()
					{
					   new CodexSpacer(),
					   new CodexCollapsibleHeader((string)  global::STRINGS.CODEX.HEADERS.ELEMENTPRODUCEDBY, contents2)
					}, ContentContainer.ContentLayout.Vertical));
					containers.Add(contents2);
				}

				return false;
			}
			/*	public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
				{
					var codes = orig.ToList();

					var f_recipes = AccessTools.DeclaredField(typeof(ComplexRecipeManager), nameof(ComplexRecipeManager.recipes));

					var index = codes.FindIndex(ci => ci.LoadsField(f_recipes));

					if (index == -1)
					{
						Log.Warning("Could not patch CodexEntryGenerator_Elements_GenerateMadeAndUsedContainers");
						return codes;
					}

					var m_AddNewLayers = AccessTools.DeclaredMethod(typeof(AmbienceManager_Quadrant_Ctor_Patch), nameof(AddNewLayers));

					codes.InsertRange(index + 1,
						[
							new CodeInstruction(OpCodes.Ldarg_0),
							new CodeInstruction(OpCodes.Call, m_AddNewLayers)
						]);

					return codes;
				}*/

		}

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
				AddSlagmiteMiningResults(__result, SlagmiteConfig.ID);
				AddSlagmiteMiningResults(__result, GleamiteConfig.ID);
				Molt(__result);
				AdditionalEntries(__result);
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix(ref CodexEntryGenerator_Elements.ElementEntryContext __result)
			{
				FixDeathEntries(__result);
			}

			// changes codex header from critter name to "Drops", making it clearer where the items come from.
			// transpiler was highly impractical as it is in a nested local function inside another giant method, so instead a best guess is made to which entry needs to be modified
			private static void FixDeathEntries(CodexEntryGenerator_Elements.ElementEntryContext context)
			{
				foreach (var butcherable in Assets.GetPrefabsWithComponentAsListOfComponents<Butcherable>())
				{
					if (context.usedMap.map.TryGetValue(butcherable.PrefabID(), out var entries))
					{
						var drops = butcherable.drops.Keys.ToHashSet();
						var prefabId = butcherable.PrefabID();

						foreach (var entry in entries)
						{
							if (!entry.prefab.IsPrefabID(prefabId))
								continue;

							if (entry.title != butcherable.GetProperName())
								continue;

							var hasNoInputs = entry.inSet == null || entry.inSet.Count == 0;
							if (hasNoInputs)
							{
								if (drops.SetEquals(entry.outSet.Select(e => e.tag.ToString()).ToHashSet()))
									entry.title = $"Dropped by {prefabId.ProperNameStripLink()}"; // TODO str
							}
						}
					}
				}
			}

			private static void Molt(CodexEntryGenerator_Elements.ElementEntryContext context)
			{
				foreach (var prefab in Assets.Prefabs)
				{
					var def = prefab.GetDef<MoltDropperMonitor.Def>();
					if (def != null)
					{
						var conversionEntry = new CodexEntryGenerator_Elements.ConversionEntry()
						{
							title = $"Molted by {prefab.GetProperName()}",  // TODO str
							prefab = prefab.gameObject,
							inSet = []
						};

						context.usedMap.Add(prefab.PrefabID(), conversionEntry);

						var applyTo = new HashSet<ElementUsage>();
						var use = new ElementUsage(def.onGrowDropID, def.massToDrop / CONSTS.CYCLE_LENGTH, true)
						{
							customFormating = (tag, amount, continous) =>
							{
								return GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerCycle) + "\n<size=66%>while happy</size>";
							}
						};

						conversionEntry.outSet.Add(use);
						context.madeMap.Add(def.onGrowDropID, conversionEntry);
					}
				}
			}

			private static void AdditionalEntries(CodexEntryGenerator_Elements.ElementEntryContext context)
			{
				List<(int order, System.Action fn)> entries = [];

				foreach (var prefab in Assets.Prefabs)
				{
					foreach (var entry in prefab.GetComponents<ICodexEntry>())
						entries.Add((entry.CodexEntrySortOrder(), () => entry.AddCodexEntries(context, prefab)));

					foreach (var entry in prefab.GetDefs<ICodexEntry>())
						entries.Add((entry.CodexEntrySortOrder(), () => entry.AddCodexEntries(context, prefab)));
				}

				entries
					.OrderBy(e => e.order)
					.Do(e => e.fn?.Invoke());
			}

			private static void AddSlagmiteMiningResults(CodexEntryGenerator_Elements.ElementEntryContext context, string critterId)
			{
				var prefab = Assets.GetPrefab(critterId);
				var shellMonitor = prefab.GetDef<ShellGrowthMonitor.Def>();

				if (shellMonitor == null)
					return;

				var autoMiner = Assets.GetPrefab(AutoMinerConfig.ID);

				var dropsTable = BDb.lootTables.Get(shellMonitor.lootTableId) as LootTable<MaterialReward>;
				var totalWeight = dropsTable.options.Sum(o => o.weight);

				foreach (var loot in dropsTable.options)
				{
					var conversionEntry = new CodexEntryGenerator_Elements.ConversionEntry()
					{
						title = autoMiner.GetProperName(),
						prefab = autoMiner,
						inSet = []
					};

					conversionEntry.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, false));
					context.usedMap.Add(prefab.PrefabID(), conversionEntry);

					var percentChance = loot.weight / totalWeight;
					var applyTo = new HashSet<ElementUsage>();
					var use = new ElementUsage(loot.item.tag, loot.item.mass, false)
					{
						customFormating = (tag, amount, continous) =>
						{
							var percentChanceStr = $"{STRINGS.CREATURES.SPECIES.BEACHED_SLAGMITE.ODDS} {GameUtil.GetStandardPercentageFloat(percentChance * 100f)}{(global::STRINGS.UI.UNITSUFFIXES.PERCENT)}";
							return $"{percentChanceStr}\n{GameUtil.GetFormattedMass(amount)}";
						}
					};

					conversionEntry.outSet.Add(use);
					context.madeMap.Add(loot.item.tag, conversionEntry);
				}
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
