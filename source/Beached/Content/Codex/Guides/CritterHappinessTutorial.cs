using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Codex.Guides
{
	public class CritterHappinessTutorial
	{
		public static string ID = "BEACHEDCRITTERHAPPINESS";

		public static CodexEntry GenerateEntry()
		{
			var categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(ID, STRINGS.UI.CODEX.CRITTER_HAPPINESS.TITLE, []);
			categoryEntry.category = BeachedCodexEntries.BEACHED_GUIDES_CATEGORY;
			PopulateEntries(categoryEntry);

			return categoryEntry;
		}

		public static CategoryEntry GenerateCategoryEntry(string id, string name)
		{
			var containers = new List<ContentContainer>
			{
				new(
				[
					new CodexText(name, CodexTextStyle.Title, null),
					new CodexDividerLine()
				], ContentContainer.ContentLayout.Vertical)
			};

			var categoryEntry = new CategoryEntry("Root", containers, name, [], true, true)
			{
				icon = null
			};

			CodexCache.AddEntry(id, categoryEntry);

			return categoryEntry;
		}

		private static void PopulateEntries(CategoryEntry categoryEntry)
		{
			var header = new List<ICodexWidget>
			{
				new CodexText("Critter Happiness", CodexTextStyle.Title),
				new CodexLabelWithIcon("Critter Happiness", CodexTextStyle.Body, new Tuple<Sprite, Color>(Assets.GetSprite("research_type_alpha_icon"), Color.white)),
				new CodexDividerLine()
			};

			var guidePart1 = new List<ICodexWidget>
			{
				new CodexText("text here", CodexTextStyle.Body),
				new CodexSpacer(),
				new CodexText("more text here", CodexTextStyle.Body)
			};

			var containers = new List<ContentContainer>
			{
				new(header, ContentContainer.ContentLayout.Vertical),
				new(guidePart1, ContentContainer.ContentLayout.Vertical)
				{
					lockID = "Beached_Lock_CritterHappiness"
				}
			};

			categoryEntry.contentContainers.AddRange(containers);
		}
	}
}
