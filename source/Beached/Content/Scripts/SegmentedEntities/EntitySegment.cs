namespace Beached.Content.Scripts.SegmentedEntities
{
	public class EntitySegment : KMonoBehaviour
	{
		public bool notifyRootOnDestroy;
		private SegmentedEntityRoot root;

		public EntitySegment()
		{
			notifyRootOnDestroy = true;
		}

		public void SetRoot(SegmentedEntityRoot root)
		{
			//if (root != null)
			//	Log.Warning($"Trying to set root of {name}, but it already has one!");

			this.root = root;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			if (!notifyRootOnDestroy)
				return;

			if (Game.IsQuitting() || App.IsExiting)
				return;

			root.OnSegmentCleanup(this);
		}
	}
}
