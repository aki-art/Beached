using KSerialization;
using System.Collections.Generic;

namespace Beached.Content.Scripts.Buildings
{
	public class Beached_DoorOpenTracker : KMonoBehaviour
	{
		[Serialize] public bool isOpen;

		private static readonly HashSet<HashedString> openingAnimations =
			[
				"open",
				 // Airlock Door mod
				"open_left",
				"open_right"
			];

		private HashedString open_left = "open";

		public override void OnSpawn()
		{
			base.OnSpawn();
			// doors open and close 2-5 times every time a duplicant goes through, and is unreliable for tracking
			GetComponent<KBatchedAnimController>().onAnimComplete += OnAnimComplete;
		}

		private void OnAnimComplete(HashedString name)
		{
			if (openingAnimations.Contains(name))
			{
				if (!isOpen)
					Trigger(ModHashes.usedBuilding);

				isOpen = true;
			}
			else
			{
				isOpen = false;
			}
		}
	}
}
