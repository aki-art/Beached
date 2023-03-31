using Beached.Content.Scripts.Buildings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Content.Scripts.UI
{
    internal class Beached_SampleSelectorSidesScreen : SideScreenContent
    {
        [SerializeField] private RectTransform consumptionSettingToggleContainer;
        [SerializeField]  private GameObject consumptionSettingTogglePrefab;
        [SerializeField] private RectTransform settingRequirementRowsContainer;
        [SerializeField] private RectTransform settingEffectRowsContainer;
        [SerializeField] private LocText selectedOptionNameLabel;
        [SerializeField]  private GameObject settingDescriptorPrefab;

        private DNAInjector target;
        private LocText descriptor = null;
        private Dictionary<Tag, HierarchyReferences> settingToggles = new();
        private List<GameObject> requirementRows = new List<GameObject>();

        public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out DNAInjector _);

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);

            if (consumptionSettingToggleContainer == null)
            {
                InitializeObjects();
            }

            if (!target.TryGetComponent(out DNAInjector newTarget))
            {
                return;
            }

            this.target = newTarget;
            PopulateOptions();
        }

        private void PopulateOptions()
        {
            if (settingToggles.Count == 0)
            {
                foreach (var item in DNAInjector.options)
                {
                    var toggle = Util.KInstantiateUI(consumptionSettingTogglePrefab, consumptionSettingToggleContainer.gameObject, true);
                    if (toggle.TryGetComponent(out HierarchyReferences hierarchyReferences))
                    {
                        settingToggles.Add(item.Key, hierarchyReferences);
                        hierarchyReferences.GetReference<LocText>("Label").text = item.Value.name;
                        hierarchyReferences.GetReference<Image>("Image").sprite = item.Value.GetIcon();
                        hierarchyReferences.GetReference<MultiToggle>("Toggle").onClick += () => SelectOption(item.Key);
                    }
                }
            }

            RefreshToggles();
            RefreshDetails();
        }

        private void SelectOption(Tag option)
        {
            target.SetSelectedSample(option);
            RefreshToggles();
            RefreshDetails();
        }

        private void RefreshToggles()
        {
            foreach (var toggle in settingToggles)
            {
                var reference = toggle.Value.GetReference<MultiToggle>("Toggle");
                reference.ChangeState(toggle.Key == target.GetSelectedOption() ? 1 : 0);
                reference.gameObject.SetActive(true);
            }
        }

        private void RefreshDetails()
        {
            descriptor ??= Util.KInstantiateUI(settingDescriptorPrefab, settingEffectRowsContainer.gameObject, true).GetComponent<LocText>();

            var selectedOption = target.GetSelectedOption();

            if (selectedOption != null && DNAInjector.options.TryGetValue(selectedOption, out var option))
            {
                descriptor.text = option.detailedDescription;
                selectedOptionNameLabel.text = $"<b>{option.name}</b>";
                descriptor.gameObject.SetActive(true);
            }
        }

        private void InitializeObjects()
        {
            consumptionSettingToggleContainer = transform.Find("Contents/ScrollViewport/SettingOptionContentContainer/").GetComponent<RectTransform>();
            consumptionSettingTogglePrefab = consumptionSettingToggleContainer.transform.Find("SettingOptionTogglePrefab").gameObject;
            var descriptors = transform.Find("Contents/DetailsScrollView/Viewport/Content/Descriptors");
            settingRequirementRowsContainer = descriptors.Find("RequirementDescriptors").GetComponent<RectTransform>();
            settingEffectRowsContainer = descriptors.Find("EffectDescriptors").GetComponent<RectTransform>();
            selectedOptionNameLabel = transform.Find("Contents/DetailsScrollView/Viewport/Content/SettingTitleLabel").GetComponent<LocText>();
            settingDescriptorPrefab = descriptors.Find("DescriptorLabelPrefab").gameObject;
        }
    }
}
