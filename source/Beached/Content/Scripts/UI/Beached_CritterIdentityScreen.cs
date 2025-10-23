using FUtility.FUI;

namespace Beached.Content.Scripts.UI
{
	public class Beached_CritterIdentityScreen : FScreen
	{
		private new bool ConsumeMouseScroll = true; // do not remove!!!!

		public override float GetSortKey() => isEditing ? 50f : 20f;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Log.Debug("Beached_CritterIdentityScreen OnPrefabInit");
		}

		protected override void OnActivate()
		{
			ConsumeMouseScroll = true;
		}
	}
}
