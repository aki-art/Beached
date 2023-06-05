using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static UnstableGroundManager;

namespace Beached.Patches
{
	public class UnstableGroundManagerPatch
	{
#if ELEMENTS
		[HarmonyPatch(typeof(UnstableGroundManager), nameof(UnstableGroundManager.OnPrefabInit))]
		public class UnstableGroundManager_OnPrefabInit_Patch
		{
			public static void Prefix(ref EffectInfo[] ___effects)
			{
				var effects = new List<EffectInfo>(___effects);

				var referenceEffect = effects.Find(e => e.element == SimHashes.Sand);

				if (referenceEffect.prefab == null)
				{
					return;
				}

				effects.Add(CreateEffect(Elements.gravel, referenceEffect.prefab));
				effects.Add(CreateEffect(Elements.ash, referenceEffect.prefab));

				___effects = effects.ToArray();
			}

			private static EffectInfo CreateEffect(SimHashes element, GameObject referencePrefab)
			{
				var prefab = Object.Instantiate(referencePrefab);
				prefab.name = $"Unstable{element}";

				return new EffectInfo()
				{
					prefab = prefab,
					element = element
				};
			}
		}
#endif
	}
}
