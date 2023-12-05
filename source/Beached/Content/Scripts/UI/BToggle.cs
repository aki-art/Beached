using FUtility.FUI;

namespace Beached.Content.Scripts.UI
{
	public class BToggle : FToggle
	{
		private LocText label;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			label = transform.Find("Label").GetComponent<LocText>();
		}

		public void SetLabel(string text)
		{
			if (label == null)
			{
				return;
			}

			label.text = text;
		}
	}
}
