namespace Beached.Content.Scripts
{
	public interface ICodexEntry
	{
		void AddCodexEntries(CodexEntryGenerator_Elements.ElementEntryContext context, KPrefabID prefab);

		int CodexEntrySortOrder();
	}
}
