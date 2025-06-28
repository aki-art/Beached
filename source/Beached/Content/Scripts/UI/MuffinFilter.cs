using Beached.Content.Scripts.Entities;
using FUtility.FUI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class MuffinFilter : KMonoBehaviour
	{
		private BIntCounter counter;
		private TMP_Dropdown dropDown;
		private FButton deleteButton;
		private FToggle2 activeToggle;
		private List<FilterOption> originalOptions;
		// private List<Tag> originalTagOptions;
		//private List<FilterOption> filteredOptions;
		//private List<Tag> filteredTagOptions;
		// private Tag selectedTag;

		private Func<Tag, bool> filterPredicate;

		public int Count
		{
			get => counter.Value;
			set => counter.Value = value;
		}

		public Action<Tag> onTagChanged;
		public System.Action onRemoved;
		public Action<int> onCounterChanged;

		public Tag Tag
		{
			get
			{
				if (dropDown.value == -1 || dropDown.value > dropDown.options.Count)
				{
					Log.Warning("Invalid tag selected. " + dropDown.value);
					return Tag.Invalid;
				}

				return (dropDown.options[dropDown.value] as FilterOption).id;
			}

			set
			{
				var index = dropDown.options.FindIndex(t => (t as FilterOption).id == value);
				if (index != -1)
				{
					dropDown.value = index;
					//selectedTag = value;
				}
				else
					Log.Warning($"not a filterable tag!! {value} -> {dropDown.options.Join(t => (t as FilterOption).id)}");

				dropDown.RefreshShownValue();
			}
		}

		public void Initialize()
		{
			if (dropDown != null)
				return;

			counter = transform.Find("NumberInput").gameObject.AddComponent<BIntCounter>();
			counter.minimum = 0;
			counter.maximum = 999;

			counter.onValueChanged += num => onCounterChanged?.Invoke(num);

			dropDown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
			dropDown.onValueChanged.AddListener(OnTypeChanged);

			deleteButton = transform.Find("DeleteButton").gameObject.AddComponent<FButton>();
			deleteButton.OnClick += () => onRemoved?.Invoke();
		}

		private void OnTypeChanged(int index)
		{
			//selectedTag = (critterType.options[index] as FilterOption).id;
			onTagChanged?.Invoke(Tag);

			RefreshFilters();
		}

		public void SetBaseOptions(IEnumerable<Tag> options)
		{
			originalOptions = [];

			foreach (var option in options)
			{
				var ui = Def.GetUISprite(Assets.GetPrefab(option));
				originalOptions.Add(new FilterOption(option.name, option.ProperName(), ui.first));
			}
		}

		public void RefreshFilters()
		{
			if (originalOptions == null)
				SetBaseOptions(CollarDispenser.cullables);

			var filteredOptions = new List<TMP_Dropdown.OptionData>();
			//filteredTagOptions = [];

			for (int i = 0; i < originalOptions.Count; i++)
			{
				if (filterPredicate == null || filterPredicate(originalOptions[i].id))
				{
					//filteredTagOptions.Add(originalTagOptions[i]);
					filteredOptions.Add(originalOptions[i]);
				}
			}

			/*if (filteredTagOptions.Count != filteredOptions.Count)
				Log.Warning("Mismatched Options length");*/

			dropDown.options.Clear();
			dropDown.AddOptions(filteredOptions);

			var tag = Tag;
			var index = dropDown.options.FindIndex(o => (o as FilterOption).id == tag);

			if (index == -1)
				Log.Warning($"Invalid filter wanted: {tag}");

			dropDown.value = index == -1 ? 0 : index;

			dropDown.RefreshShownValue();
		}

		public void FilterOptions(Func<Tag, bool> predicate)
		{
			Log.AssertNotNull(dropDown, "critterType");
			filterPredicate = predicate;
			RefreshFilters();
		}

		public class FilterOption(string id, string name, Sprite sprite) : TMP_Dropdown.OptionData(name, sprite)
		{
			[SerializeField] public string id = id;
		}
	}
}
