using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	internal class CodexEntryGenerator_CreaturesPatches
	{
		[HarmonyPatch(typeof(CodexEntryGenerator_Creatures), "GenerateEntries")]
		public class CodexEntryGenerator_Creatures_GenerateEntries_Patch
		{
			public static void Postfix()
			{
				var brains = Assets.GetPrefabsWithComponent<CreatureBrain>();

				AddCreature(BTags.Species.snail, STRINGS.CREATURES.FAMILY.BEACHEDSLICKSHELL, brains);
				AddCreature(BTags.Species.muffin, STRINGS.CREATURES.FAMILY.BEACHEDMUFFIN, brains);
				AddCreature(BTags.Species.karacoo, STRINGS.CREATURES.FAMILY.BEACHEDKARACOO, brains);
				AddCreature(BTags.Species.mite, STRINGS.CREATURES.FAMILY.BEACHEDMITE, brains);
				//AddCreature(BTags.Species.jellyfish, STRINGS.CREATURES.FAMILY.BEACHEDJELLYFISH, brains);
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
