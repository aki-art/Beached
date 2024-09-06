using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class Beached_AdditionalEntitiesTraitsScreen : KMonoBehaviour
	{
		private CollapsibleDetailContentPanel traitsPanel;
		private DetailsPanelDrawer traitsDrawer;
		private GameObject attributesLabelTemplate;
		private GameObject target;

		private bool initialized;

		public void Initialize(SimpleInfoScreen instance)
		{
			if (!initialized)
			{
				attributesLabelTemplate = instance.attributesLabelTemplate;
				traitsPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, instance.gameObject);
				traitsDrawer = new DetailsPanelDrawer(attributesLabelTemplate, traitsPanel.Content.gameObject);
				traitsPanel.HeaderLabel.text = global::STRINGS.UI.CHARACTERCONTAINER_TRAITS_TITLE;

				initialized = true;
			}
		}

		public void SetTarget(GameObject target)
		{
			this.target = target;
			Refresh();
		}

		public void Hide()
		{
			if (traitsPanel != null)
				traitsPanel.gameObject.SetActive(false);
		}

		private void Refresh()
		{
			if (!gameObject.activeSelf || target == null)
			{
				return;
			}

			RefreshTraits();
		}

		private void RefreshTraits()
		{
			var hasAnyTraits = false;

			if (target.TryGetComponent(out Traits traits))
			{
				var GMO = Db.Get().traitGroups.TryGet(BCritterTraits.GMO_GROUP);
				var Geysers = Db.Get().traitGroups.TryGet(BGeyserTraits.GEYSER_GROUP);

				hasAnyTraits |= DrawTraits(traits, GMO) > 0;
				hasAnyTraits |= DrawTraits(traits, Geysers) > 0;
			}

			if (!hasAnyTraits)
				traitsPanel.gameObject.SetActive(false);
		}

		private int DrawTraits(Traits traits, TraitGroup group)
		{
			var count = 0;
			traitsPanel.gameObject.SetActive(true);
			traitsPanel.HeaderLabel.text = (string)global::STRINGS.UI.DETAILTABS.STATS.GROUPNAME_TRAITS;

			traitsDrawer.BeginDrawing();

			foreach (var trait in traits.TraitList)
			{
				if (!string.IsNullOrEmpty(trait.Name) && group.modifiers.Contains(trait))
				{
					traitsPanel.SetLabel("traitLabel_" + trait.Id, trait.Name, trait.GetTooltip());
					count++;
				}
			}

			traitsDrawer.EndDrawing();
			return count;
		}
	}
}
