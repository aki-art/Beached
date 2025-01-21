using Beached.Content.Scripts.Entities.Plant;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class Beached_RubberTappableSidescreen : SideScreenContent
	{
		private RubberTappable target;
		public Transform container;

		private BButton button;

		public override int GetSideScreenSortOrder() => -1;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			InitializeScreen();
		}

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out RubberTappable _);

		public override void SetTarget(GameObject target)
		{
			if (target == null)
				return;

			target.Subscribe(ModHashes.sidesSreenRefresh, Refresh);

			InitializeScreen();

			if (target.TryGetComponent(out RubberTappable tappable))
			{
				this.target = tappable;
				UpdateToggle();
			}
		}

		public override void ClearTarget()
		{
			if (target != null)
				target.Unsubscribe(ModHashes.sidesSreenRefresh, Refresh);

			base.ClearTarget();
		}

		private void Refresh(object _)
		{
			UpdateToggle();
		}

		private void UpdateToggle()
		{
			if (target == null)
				return;

			button.SetLabel(target.SidescreenButtonText);
		}


		public override string GetTitle() => "Rubber Tap";

		private void InitializeScreen()
		{
			if (button != null)
				return;

			transform.Find("Scroll View").gameObject.SetActive(true);
			container = transform.Find("Scroll View/Viewport/Contents");

			var buttonGroupPrefab = transform.Find("Contents/ButtonGroupPrefab").transform;
			var buttonGroup = Instantiate(buttonGroupPrefab);
			buttonGroup.transform.SetParent(container);

			var buttonPrefab = buttonGroup.Find("ButtonPrefab").gameObject.AddOrGet<BButton>();


			button = Instantiate(buttonPrefab);
			button.transform.SetParent(buttonGroup);
			button.SetLabel(STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.TAP);
			button.name = "ToggleTapButton";

			FUtility.FUI.Helper.AddSimpleToolTip(button.gameObject, STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.TOOLTIP);

			container.gameObject.SetActive(true);
			buttonGroup.gameObject.SetActive(true);
			button.gameObject.SetActive(true);
			buttonPrefab.gameObject.SetActive(false);

			button.OnClick += UpdateTapOrder;
		}

		private void UpdateTapOrder()
		{
			if (target != null)
				target.OnSidescreenButtonPressed();
		}
	}
}
