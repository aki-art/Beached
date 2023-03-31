using Beached.Content.ModDb;
using Klei.AI;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
    public class Beached_CritterTraitsScreen : KMonoBehaviour
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
                traitsDrawer = new DetailsPanelDrawer(attributesLabelTemplate, traitsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
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
            {
                traitsPanel.gameObject.SetActive(false);
            }
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

            if (target.TryGetComponent(out Traits traits) && target.TryGetComponent(out CreatureBrain _))
            {
                var group = Db.Get().traitGroups.TryGet(BCritterTraits.GMO_GROUP);

                traitsPanel.gameObject.SetActive(true);
                traitsPanel.HeaderLabel.text = (string)global::STRINGS.UI.DETAILTABS.STATS.GROUPNAME_TRAITS;

                traitsDrawer.BeginDrawing();

                foreach (var trait in traits.TraitList)
                {
                    if (!string.IsNullOrEmpty(trait.Name) && group.modifiers.Contains(trait))
                    {
                        traitsDrawer.NewLabel(trait.Name).Tooltip(trait.GetTooltip());
                        hasAnyTraits = true;
                    }
                }

                traitsDrawer.EndDrawing();
            }

            if (!hasAnyTraits)
            {
                traitsPanel.gameObject.SetActive(false);
            }
        }
    }
}
