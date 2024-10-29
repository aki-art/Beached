using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class Beached_CollarDispenserSidescreen : SideScreenContent
	{
		public Transform container;
		public BIntCounter counter;
		public BToggle checkbox;
		private CollarDispenser target;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out CollarDispenser _);

		public override void SetTarget(GameObject target)
		{
			Log.Debug("set target collar");
			base.SetTarget(target);

			if (counter == null)
				InitializeScreen();

			if (target.TryGetComponent(out CollarDispenser dispenser))
			{
				this.target = dispenser;
			}
		}

		public override int GetSideScreenSortOrder() => 0;

		private void InitializeScreen()
		{
			Log.Debug("initializing screen COLLAR");

			transform.Find("Scroll View").gameObject.SetActive(true);
			container = transform.Find("Scroll View/Viewport/Contents");
			var checkboxPrefab = transform.Find("Contents/CheckBoxPrefab").gameObject.AddOrGet<BToggle>();
			var counterPrefab = transform.Find("Contents/CounterPrefab").gameObject.AddOrGet<BIntCounter>();

			checkbox = Instantiate(checkboxPrefab);

			checkbox.transform.SetParent(container);
			checkbox.gameObject.SetActive(true);
			checkbox.SetLabel("Per critter type");


			counter = Instantiate(counterPrefab);
			counter.transform.SetParent(container);
			container.gameObject.SetActive(true);
		}

	}
}
