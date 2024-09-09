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
				List<GameObject> brains = Assets.GetPrefabsWithComponent<CreatureBrain>();

				var snails = CodexEntryGenerator_Creatures.GenerateCritterEntry(BTags.Species.snail, STRINGS.CREATURES.FAMILY.BEACHEDSLICKSHELL, brains);
				snails.parentId = "CREATURES";

				var entries = new HashSet<string>();

				// need to remove the _-s and move the same entries under the fixed ID
				// https://forums.kleientertainment.com/klei-bug-tracker/oni/codex-critter-entry-generators-ignore-_-s-but-sidescreen-links-dont-r46250/
				foreach (var item in snails.subEntries)
				{
					entries.Add(item.id);
					var correctId = CodexCache.FormatLinkID(item.id);
					CodexCache.AddSubEntry(correctId, CodexCache.FindSubEntry(item.id));
					item.id = correctId;
				}

				foreach (var item in entries)
				{
					CodexCache.subEntries.Remove(item);
				}

				CodexCache.AddEntry(BTags.Species.snail.ToString(), snails);
			}
		}
	}
}
