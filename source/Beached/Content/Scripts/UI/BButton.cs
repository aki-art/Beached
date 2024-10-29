using FUtility.FUI;

namespace Beached.Content.Scripts.UI
{
	public class BButton : FButton
	{
		private LocText label;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			label = transform.Find("Text").GetComponent<LocText>();
		}

		public void SetLabel(string text)
		{
			label ??= transform.Find("Text").GetComponent<LocText>();
			if (label != null)
				label.text = text;
			else
				Log.Warning("This button has no label.");
		}
	}
}
