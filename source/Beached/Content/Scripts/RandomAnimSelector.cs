using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class RandomAnimSelector : KMonoBehaviour
	{
		[SerializeReference] public List<HashedString> animNames;
		[SerializeField] public KAnim.PlayMode playMode;

		[Serialize] public HashedString animName;

		[MyCmpReq] public KBatchedAnimController kbac;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (!IsValidAnim())
				animName = animNames.GetRandom();

			RefreshAnimation();
		}

		private void RefreshAnimation()
		{
			kbac.Play(animName, playMode);
		}

		public bool IsValidAnim()
		{
			return animName.IsValid && kbac.HasAnimation(animName);
		}
	}
}
