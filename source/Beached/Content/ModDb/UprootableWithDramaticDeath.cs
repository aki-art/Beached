using Beached.Content.Defs;
using KSerialization;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class UprootableWithDramaticDeath : Uprootable
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[SerializeField] public string deathAnimation;
		[SerializeField] public string deathSoundFx;
		[SerializeField] public float deathSoundVolume;

		[Serialize] public bool allowUprooting;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			multitoolHitEffectTag = BEffectConfigs.MUSSEL_GIBLETS;
			multitoolContext = ModAssets.CONTEXTS.HARVEST_ORANGE_SQUISH;
		}

		public UprootableWithDramaticDeath()
		{
			deathAnimation = "harvest";
			deathSoundVolume = 1.0f;
		}

		[Override]
		public static bool Uproot(Uprootable __instance)
		{
			if (__instance is UprootableWithDramaticDeath self)
			{
				if (self.uprootComplete || self.allowUprooting)
					return true;

				self.kbac.Play(self.deathAnimation);
				self.kbac.onAnimComplete += self.UpRootedForReal;

				if (!self.deathSoundFx.IsNullOrWhiteSpace())
					AudioUtil.PlaySound(self.deathSoundFx, self.transform.position, self.deathSoundVolume);

				return false;
			}

			return true;
		}

		private void UpRootedForReal(HashedString name)
		{
			allowUprooting = true;
			Uproot();
		}
	}
}
