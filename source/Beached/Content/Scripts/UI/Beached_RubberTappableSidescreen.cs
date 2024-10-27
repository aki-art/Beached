using Beached.Content.Scripts.Entities.Plant;
using Epic.OnlineServices;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class Beached_RubberTappableSidescreen : SideScreenContent
	{
		private RubberTappable target;
		public Transform container;

		private BToggle checkbox;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out RubberTappable _);

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			if (checkbox == null)
				InitializeScreen();

			if (target.TryGetComponent(out RubberTappable tappable))
			{
				this.target = tappable;
				UpdateToggle();
			}
		}

		private void UpdateToggle()
		{
			if (target == null)
				return;

			checkbox.SetLabel(target.SidescreenButtonText);
		}

		public override int GetSideScreenSortOrder() => 0;

		private void InitializeScreen()
		{
			transform.Find("Scroll View").gameObject.SetActive(true);
			container = transform.Find("Scroll View/Viewport/Contents");

			transform.Find("Scroll View").gameObject.SetActive(true);
			var checkboxPrefab = transform.Find("Contents/CheckBoxPrefab").gameObject.AddOrGet<BToggle>();

			checkbox = Instantiate(checkboxPrefab);

			checkbox.transform.SetParent(container);
			checkbox.gameObject.SetActive(true);
			checkbox.SetLabel(STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.TAP);
			container.gameObject.SetActive(true);

			FUtility.FUI.Helper.AddSimpleToolTip(checkbox.gameObject, STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.TOOLTIP);

			checkbox.OnClick += UpdateTapOrder;
		}

		private void UpdateTapOrder()
		{
			if (target == null)
				return;

			target.OnSidescreenButtonPressed();
		}
	}
}
