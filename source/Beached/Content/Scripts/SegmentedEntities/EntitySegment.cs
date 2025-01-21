using KSerialization;

namespace Beached.Content.Scripts.SegmentedEntities
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class EntitySegment : KMonoBehaviour
	{
		[Serialize] public bool notifyRootOnDestroy;
		private SegmentedEntityRoot root;
		[Serialize] public HashedString animation;

		public EntitySegment()
		{
			notifyRootOnDestroy = true;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (animation.IsValid)
				GetComponent<KBatchedAnimController>().Play(animation);
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
