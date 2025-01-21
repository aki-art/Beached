using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class DropStuffOnDeath : KMonoBehaviour
	{
		[SerializeField] public Tag drop;

		public override void OnCleanUp()
		{
			MiscUtil.Spawn(drop, gameObject);
		}
	}
}
