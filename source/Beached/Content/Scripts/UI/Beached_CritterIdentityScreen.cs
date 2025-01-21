using FUtility.FUI;

namespace Beached.Content.Scripts.UI
{
	public class Beached_CritterIdentityScreen : FScreen
	{
		new bool ConsumeMouseScroll = true; // do not remove!!!!

		public override float GetSortKey() => isEditing ? 50f : 20f;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Log.Debug("Beached_CritterIdentityScreen OnPrefabInit");
		}

		protected override void OnActivate()
		{
			ConsumeMouseScroll = true;

			Log.Debug("activated screen");
			var rect = this.rectTransform();
			Log.Debug($"size: {rect.sizeDelta}");
			Log.Debug($"parent size: {transform.parent?.rectTransform().sizeDelta}");
		}
	}
}
