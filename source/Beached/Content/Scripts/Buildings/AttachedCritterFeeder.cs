using KSerialization;

namespace Beached.Content.Scripts.Buildings
{
	public class AttachedCritterFeeder : KMonoBehaviour
	{
		[Serialize] public Ref<CritterFeederStorageBlock> parent;

		public override void OnSpawn()
		{
			var target = parent.Get();

			if (target == null || !target.TryGetComponent(out CritterFeederStorageBlock filter) || filter.GetFeeder() != this)
			{
				Util.KDestroyGameObject(gameObject);
				Log.Debug("destroying AttachedCritterFeeder");
				return;
			}

			base.OnSpawn();
		}
	}
}
