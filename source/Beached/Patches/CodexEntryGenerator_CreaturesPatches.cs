using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	public class CodexEntryGenerator_CreaturesPatches
	{
		[HarmonyPatch(typeof(CodexEntryGenerator_Creatures), "GenerateCreatureDescriptionContainers")]
		public class CodexEntryGenerator_Creatures_GenerateCreatureDescriptionContainers_Patch
		{
			public static void Postfix(GameObject creature, List<ContentContainer> containers)
			{
				AdditionalPoops(creature, containers);
			}

			private static void AdditionalPoops(GameObject creature, List<ContentContainer> containers)
			{
				if (creature.TryGetComponent(out AdditionalPoopTags additionalPoopTags))
				{
					if (!GetDietEntriesContainer(containers, out var dietEntriesContainer, out var contents))
						return;

					foreach (var content in contents.content)
					{
						if (content is CodexConversionPanel dietPanel)
						{
							foreach (var entry in additionalPoopTags.entries)
							{
								if (dietPanel.outs == null || dietPanel.outs.Length == 0)
									continue;

								var originalOut = dietPanel.outs[0];
								dietPanel.outs = dietPanel.outs.AddToArray(new ElementUsage(entry.tag, entry.ratioToOutput * originalOut.amount, originalOut.continuous, originalOut.customFormating));
							}
						}
					}
				}
			}

			private static bool GetDietEntriesContainer(List<ContentContainer> containers, out ContentContainer dietEntriesContainer, out ContentContainer contents)
			{
				contents = null;
				dietEntriesContainer = containers.Find(container => container.content != null
						&& container.content.Count >= 2
						&& container.content[1] is CodexCollapsibleHeader header
						&& header.label == (string)global::STRINGS.CODEX.HEADERS.DIET);

				if (dietEntriesContainer == null)
					return false;

				contents = (dietEntriesContainer.content[1] as CodexCollapsibleHeader).contents;

				return contents != null;
			}
		}

		[HarmonyPatch(typeof(CodexEntryGenerator_Creatures), "GenerateEntries")]
		public class CodexEntryGenerator_Creatures_GenerateEntries_Patch
		{
			public static void Postfix()
			{
				var brains = Assets.GetPrefabsWithComponent<CreatureBrain>();

				AddCreature(BTags.Species.snail, STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES, brains);
				AddCreature(BTags.Species.muffin, STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDMUFFINSPECIES, brains);
				AddCreature(BTags.Species.rotMonger, STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDROTMONGERSPECIES, brains);
				AddCreature(BTags.Species.karacoo, STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDKARACOOSPECIES, brains);
				AddCreature(BTags.Species.mite, STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDMITESPECIES, brains);
				AddCreature(BTags.Species.jellyfish, STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDJELLYFISHSPECIES, brains);
			}

			private static void AddCreature(Tag species, string name, List<GameObject> brains)
			{
				var critters = CodexEntryGenerator_Creatures.GenerateCritterEntry(species, name, brains);
				critters.parentId = "CREATURES";

				var entries = new HashSet<string>();

				foreach (var item in critters.subEntries)
				{
					var oldId = item.id;
					entries.Add(item.id);

					// need to remove the _-s and move the same entries under the fixed ID
					// https://forums.kleientertainment.com/klei-bug-tracker/oni/codex-critter-entry-generators-ignore-_-s-but-sidescreen-links-dont-r46250/
					var correctId = CodexCache.FormatLinkID(item.id);
					CodexCache.AddSubEntry(correctId, item);
					item.id = correctId;
					CodexCache.subEntries.Remove(oldId);
				}

				CodexCache.AddEntry(species.ToString(), critters);
			}
		}
	}
}
