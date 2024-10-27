using Beached.Content.Codex.Guides;
using System.Collections.Generic;

namespace Beached.Content.Codex
{
	public class BeachedCodexEntries
	{
		public const string BEACHED_GUIDES_CATEGORY = "BEACHEDGUIDES";
		public static void Generate()
		{
			MushroomTutorial.GenerateEntry();
			CreateGuidesEntry(new CodexEntry(BEACHED_GUIDES_CATEGORY, [], "title"));
		}

		public static Dictionary<string, CodexEntry> CreateGuidesEntry(CodexEntry entry)
		{
			var results = new Dictionary<string, CodexEntry>()
			{
				{ MushroomTutorial.ID, entry }
			};

			CreateModsCategory(BEACHED_GUIDES_CATEGORY, results);

			return results;
		}

		private static void CreateModsCategory(string ID, Dictionary<string, CodexEntry> entries)
		{
			var categoryEntry = new CodexEntry(ID, [], STRINGS.UI.CODEX.GUIDES)
			{
				id = ID,
				category = ""
			};

			if (!CodexCache.entries.ContainsKey(ID))
				CodexCache.AddEntry(ID, categoryEntry, null);
			else
				CodexCache.MergeEntry(ID, categoryEntry);
		}
	}
}
