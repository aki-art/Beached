using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	public class FurSource : KMonoBehaviour
	{
		private FurSourceReactable reactable;

		public static HashSet<string> furries =
		[
			SquirrelConfig.ID,
			SquirrelHugConfig.ID
		];

		public bool CanTriggerReaction() => GetComponent<Traits>().HasTrait(BCritterTraits.HYPOALLERGENIC);

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (CanTriggerReaction())
				reactable = new FurSourceReactable(gameObject);
		}

		public void RemoveReactable()
		{
			reactable?.Cleanup();
			reactable = null;
		}
	}
}
