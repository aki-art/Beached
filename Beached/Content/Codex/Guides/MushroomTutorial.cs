using System.Collections.Generic;

namespace Beached.Content.Codex.Guides
{
    public class MushroomTutorial
    {
        public static string ID = "BEACHEDMUSHROOMS";

        public static CodexEntry GenerateEntry()
        {
            var categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(ID, STRINGS.UI.CODEX.SPORES.TITLE, new Dictionary<string, CodexEntry>());
            categoryEntry.category = BeachedCodexEntries.BEACHED_GUIDES_CATEGORY;
            PopulateEntries(categoryEntry);

            return categoryEntry;
        }

        public static CategoryEntry GenerateCategoryEntry(string id, string name)
        {
            var containers = new List<ContentContainer>
            {
                new ContentContainer(new List<ICodexWidget>
                {
                    new CodexText(name, CodexTextStyle.Title, null),
                    new CodexDividerLine()
                }, ContentContainer.ContentLayout.Vertical)
            };

            var categoryEntry = new CategoryEntry("Root", containers, name, new List<CodexEntry>(), true, true)
            {
                icon = null
            };

            CodexCache.AddEntry(id, categoryEntry);

            return categoryEntry;
        }

        private static void PopulateEntries(CategoryEntry categoryEntry)
        {
            var guidePart1 = new List<ICodexWidget>
            {
                new CodexText(STRINGS.UI.CODEX.SPORES.CONTENT, CodexTextStyle.Body),
            };

            var containers = new List<ContentContainer>
            {
                new ContentContainer(guidePart1, ContentContainer.ContentLayout.Vertical)
                {
                    lockID = "Beached_Lock_CritterHappiness"
                }
            };

            categoryEntry.contentContainers.AddRange(containers);
        }
    }
}
