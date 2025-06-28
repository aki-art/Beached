using Beached.Content.Scripts.Entities;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Beached.STRINGS.BEACHED.UI.MUFFIN_SIDESCREEN.CONTENTSTOP;

namespace Beached.Content.Scripts.UI
{
	public class Beached_MuffinSideScreen : SideScreenContent
	{
		private CollarDispenser target;
		private TMP_Dropdown dropdown;

		private Transform filterParent;
		private MuffinFilter filterPrefab;
		private FSlider ageSlider;
		private FInputField2 ageInput;
		private bool isUpdatingAge;
		private FToggle2 ignoreEggToggle;
		private FToggle2 ignoreNamedToggle;
		private BIntCounter defaultCount;
		private FButton addFilterButton;
		private FCycle defaultCycle;

		private List<MuffinFilter> filters;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out CollarDispenser _);

		private CollarDispenser GetTarget() => target;

		public override void SetTarget(GameObject target)
		{
			if (target == null)
				return;

			target.Subscribe(ModHashes.sidesSreenRefresh, Refresh);

			InitializeScreen();

			if (target.TryGetComponent(out CollarDispenser dispenser))
			{
				this.target = dispenser;
				Refresh(null);
			}
		}

		public override void ClearTarget()
		{
			if (target != null)
				target.Unsubscribe(ModHashes.sidesSreenRefresh, Refresh);

			base.ClearTarget();
		}

		private void InitializeScreen()
		{
			Log.Debug("initializing screen");

			if (filterParent != null)
				return;

			filterParent = transform.Find("Scroll View/Viewport/Contents/FilterCategory");
			filterPrefab = filterParent.Find("CritterFilterPrefab").gameObject.AddComponent<MuffinFilter>();
			filterPrefab.Initialize();
			filterPrefab.gameObject.SetActive(false);

			ageSlider = transform.Find("ContentsTop/AgeSlider/Slider").gameObject.AddComponent<FSlider>();
			ageSlider.OnChange += OnAgeSliderChange;
			ageInput = ageSlider.transform.parent.Find("InputField").gameObject.AddComponent<FInputField2>();
			ageInput.OnValueChanged.AddListener(OnAgeInputChange);

			ignoreEggToggle = transform.Find("ContentsTop/IgnoreEggsCheckbox").gameObject.AddComponent<FToggle2>();
			ignoreEggToggle.SetCheckmark("Background/Checkmark");
			ignoreEggToggle.OnChange += () =>
			{
				if (target != null) target.ignoreEggs = ignoreEggToggle.On;
			};

			Helper.AddSimpleToolTip(ignoreEggToggle.gameObject, IGNOREEGGSCHECKBOX.TOOLTIP);

			ignoreNamedToggle = transform.Find("ContentsTop/IgnoreNamedCheckbox").gameObject.AddComponent<FToggle2>();
			ignoreNamedToggle.SetCheckmark("Background/Checkmark");
			ignoreNamedToggle.OnChange += () =>
			{
				if (target != null) target.ignoreNamed = ignoreNamedToggle.On;
			};

			Helper.AddSimpleToolTip(ignoreNamedToggle.gameObject, IGNORENAMEDCHECKBOX.TOOLTIP);

			defaultCycle = transform.Find("ContentsTop/FilterlessCycle").gameObject.AddComponent<FCycle>();
			defaultCycle.Initialize(
				defaultCycle.transform.Find("Left1").gameObject.AddComponent<FButton>(),
				defaultCycle.transform.Find("Right1").gameObject.AddComponent<FButton>(),
				defaultCycle.transform.Find("Label").gameObject.GetComponent<LocText>(),
				defaultCycle.transform.Find("Description").gameObject.GetComponent<LocText>());

			defaultCycle.Options =
			[
				new FCycle.Option(
					CollarDispenser.DefaultBeheavior.Count.ToString(),
					FILTERLESSCYCLE.COUNT,
					FILTERLESSCYCLE.COUNT_DESC),

				new FCycle.Option(
					CollarDispenser.DefaultBeheavior.Hunt.ToString(),
					FILTERLESSCYCLE.HUNT,
					FILTERLESSCYCLE.HUNT_DESC),

				new FCycle.Option(
					CollarDispenser.DefaultBeheavior.Ignore.ToString(),
					FILTERLESSCYCLE.IGNORE,
					FILTERLESSCYCLE.IGNORE_DESC),
			];

			defaultCycle.OnChange += OnDefaultCycleChange;
			defaultCount = transform.Find("ContentsTop/Default/NumberInput").gameObject.AddComponent<BIntCounter>();
			defaultCount.minimum = 0;
			defaultCount.maximum = 999;

			addFilterButton = transform.Find("Contents/AddButton/Button").gameObject.AddComponent<FButton>();
			addFilterButton.OnClick += AddNewFilter;
		}

		private void AddNewFilter()
		{
			var tag = GetMostLikelyRelevantTag();
			AddFilter(tag, target.defaultCritterCount, true);
		}

		private void OnDefaultCycleChange()
		{
			defaultCount.transform.parent.gameObject.SetActive(defaultCycle.Value == "Count");
			if (target != null && Enum.TryParse(defaultCycle.Value, out CollarDispenser.DefaultBeheavior defaultBehavior))
				target.defaultBehavior = defaultBehavior;

			target.RefreshTargets();
		}

		private MuffinFilter AddFilter(Tag tag, int count, bool updateAllFilters)
		{
			Log.Debug($"adding tag: {tag} {count}");

			if (!tag.IsValid)
			{
				tag = target.GetNewFilterableTag();
				return null;
			}

			if (target.filteredCritters.ContainsKey(tag))
			{
				Log.Warning("trying to add a filter twice: " + tag);
				return null;
			}

			var filter = Util.KInstantiateUI<MuffinFilter>(filterPrefab.gameObject, filterParent.gameObject);
			filter.gameObject.SetActive(true);
			filter.Initialize();


			filter.SetBaseOptions(CollarDispenser.cullables);
			filter.FilterOptions(predicate: t =>
			{
				if (t == Tag.Invalid)
					return false;

				// allow already selected option
				if (t == tag)
					return true;

				var target = GetTarget();
				if (target == null)
					return false;

				return !target.filteredCritters.ContainsKey(t);
			});

			filter.Tag = tag;
			filter.Count = count;

			filter.onRemoved += () =>
			{
			};

			filter.onTagChanged += _ => UpdateTargetFilters();
			filter.onCounterChanged += count => target.SetFilter(filter.Tag, count);

			if (updateAllFilters)
				UpdateTargetFilters();

			return filter;
		}

		private void UpdateTargetFilters()
		{
			Log.Debug("UpdateTargetFilters");

			target.filteredCritters.Clear();

			foreach (var filter in filters)
				target.SetFilter(filter.Tag, filter.Count);

			foreach (var filter in filters)
				filter.RefreshFilters();
		}

		private Tag GetMostLikelyRelevantTag()
		{
			if (target == null)
				return Tag.Invalid;

			var room = Game.Instance.roomProber.GetRoomOfGameObject(target.gameObject);

			if (room == null || room.cavity == null || room.cavity.creatures == null)
				return Tag.Invalid;

			foreach (var critter in room.cavity.creatures)
			{
				if (target.IsAcceptedTag(critter.PrefabTag))
					return critter.PrefabTag;
			}

			return Tag.Invalid;
		}


		private void OnAgeInputChange(string str)
		{
			if (isUpdatingAge)
				return;

			isUpdatingAge = true;

			if (int.TryParse(str, out int num))
			{
				ageSlider.Value = num;
				if (target != null)
					target.minAge = num;
			}


			isUpdatingAge = false;
		}

		private void OnAgeSliderChange()
		{
			if (isUpdatingAge)
				return;

			isUpdatingAge = true;

			ageInput.Text = ageSlider.Value.ToString();
			if (target != null)
				target.minAge = ageSlider.Value;

			isUpdatingAge = false;
		}

		private void Refresh(object _)
		{
			if (target != null)
			{
				isUpdatingAge = true;

				var minAgeInt = Mathf.RoundToInt(target.minAge * 100f);
				ageSlider.Value = minAgeInt;
				ageInput.Text = minAgeInt.ToString();

				isUpdatingAge = false;

				ignoreEggToggle.On = target.ignoreEggs;
				ignoreNamedToggle.On = target.ignoreNamed;

				defaultCount.Value = target.defaultCritterCount;
				defaultCount.transform.parent.gameObject.SetActive(target.defaultBehavior == CollarDispenser.DefaultBeheavior.Count);
				defaultCycle.Value = target.defaultBehavior.ToString();

				filters = [];

				if (target.filteredCritters != null)
				{
					foreach (var filter in target.filteredCritters)
						AddFilter(filter.Key, filter.Value, false);

					UpdateTargetFilters();
				}
			}
		}
	}
}
