using KSerialization;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	public class SimpleFlatFilterable : KMonoBehaviour
	{
		[Serialize] public HashSet<Tag> selectedTags = new();

		[MyCmpReq] private TreeFilterable treeFilterable;

		public HashSet<Tag> tagOptions = new();
		public string headerText;

		public override void OnSpawn()
		{
			base.OnSpawn();
			treeFilterable.filterByStorageCategoriesOnSpawn = false;
			Refresh();
		}

		public void Refresh()
		{
			treeFilterable.UpdateFilters(selectedTags);
		}

		public void SelectTag(Tag tag, bool state)
		{
			if (!tagOptions.Contains(tag))
			{
				Log.Warning($"SimpleFlatFilterable: The tag {tag.Name} is not valid for this filterable - it must be added to tagOptions");
				return;
			}

			if (state)
				selectedTags.Add(tag);
			else
				selectedTags.Remove(tag);

			treeFilterable.UpdateFilters(selectedTags);
		}

		public void ToggleTag(Tag tag) => SelectTag(tag, !selectedTags.Contains(tag));

		public string GetHeaderText() => headerText;
	}
}
