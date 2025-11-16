using Beached.Content.Defs.Duplicants;
using KSerialization;

namespace Beached.Content.Scripts
{
	public class Beached_Grave : KMonoBehaviour
	{
		[Serialize] public HashedString originalPersonalityId;

		public override void OnSpawn()
		{
			if (originalPersonalityId != HashedString.Invalid)
				SetPersonality(originalPersonalityId);
		}

		public void SetPersonality(HashedString id)
		{
			originalPersonalityId = id;

			if (originalPersonalityId == MinnowConfig.ID)
			{
				var overrideAnim = Assets.GetAnim("beached_graveicons_kanim");
				var symbol = overrideAnim.GetData().build.GetSymbol("minnow");

				var kbac = GetComponent<KBatchedAnimController>();
				kbac.symbolOverrideController.AddSymbolOverride("swapme", symbol, 9999);
			}
		}
	}
}
