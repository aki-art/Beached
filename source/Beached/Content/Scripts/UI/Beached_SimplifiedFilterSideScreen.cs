using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	// just a flat list of selectables that notify the treefilterable to fetch somethings.
	// very similar to FlatTagFilterableSidescreen, but this one has a scrollbar

	// of course a few weeks after i coded this Klei added a similar UI to the water cooler... oh well
	public class Beached_SimplifiedFilterSideScreen : SideScreenContent
	{
		private SimpleFlatFilterable target;
		private Dictionary<Tag, BIconToggle> toggles;

		private BIconToggle togglePrefab;
		private Transform toggleContainer;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out SimpleFlatFilterable _);

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			if (togglePrefab == null)
			{
				InitializeScreen();
			}

			if (target.TryGetComponent(out SimpleFlatFilterable filterable))
			{
				this.target = filterable;
				UpdateToggles(filterable.tagOptions);
			}
		}

		private void InitializeScreen()
		{
			transform.Find("Scroll View").gameObject.SetActive(true);
			toggleContainer = transform.Find("Scroll View/Viewport/Contents");
			togglePrefab = transform.Find("Contents/CheckBoxIconPrefab").gameObject.AddOrGet<BIconToggle>();
		}

		private void UpdateToggles(HashSet<Tag> tags)
		{
			toggles ??= new Dictionary<Tag, BIconToggle>();

			foreach (var toggle in toggles.Values)
			{
				toggle.gameObject.SetActive(false);
			}

			if (tags == null)
			{
				return;
			}

			foreach (var tag in tags)
			{
				if (!DiscoveredResources.Instance.IsDiscovered(tag) && !DebugHandler.InstantBuildMode)
				{
					continue;
				}

				if (toggles.TryGetValue(tag, out BIconToggle toggle))
				{
					// update
					toggle.gameObject.SetActive(true);
				}
				else
				{
					// add
					var newToggle = Util.KInstantiateUI<BIconToggle>(togglePrefab.gameObject, toggleContainer.gameObject, true);
					toggles[tag] = newToggle;

					var prefab = Assets.GetPrefab(tag);
					if (prefab != null)
					{
						newToggle.SetIcon(Def.GetUISprite(prefab).first);
						newToggle.SetLabel(prefab.PrefabID().ProperNameStripLink());
					}

					newToggle.toggle.onValueChanged.AddListener(val => OnToggled(tag, val));
				}

				toggles[tag].toggle.SetIsOnWithoutNotify(target.selectedTags.Contains(tag));
			}
		}

		private void OnToggled(Tag tag, bool value)
		{
			Log.Debug("TOGGLED TAG: " + tag);
			target.SelectTag(tag, value);
		}

		public override int GetSideScreenSortOrder() => -1;
	}
}
