using Beached.Content.Scripts.UI;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class SimpleInfoScreenPatch
	{
		public static void Patch(Harmony harmony)
		{
			var prefix = AccessTools.DeclaredMethod(typeof(SimpleInfoScreen_OnSelectTarget_Patch), "Prefix");
			var targetMethod = AccessTools.DeclaredMethod(typeof(SimpleInfoScreen), nameof(SimpleInfoScreen.OnSelectTarget));

			harmony.Patch(targetMethod, new HarmonyMethod(prefix));
		}

		public class SimpleInfoScreen_OnSelectTarget_Patch
		{
			private static void Prefix(ref SimpleInfoScreen __instance, GameObject target)
			{
				if (target.TryGetComponent(out CreatureBrain _))
				{
					var critterTraits = __instance.gameObject.AddOrGet<Beached_CritterTraitsScreen>();
					critterTraits.Initialize(__instance);
					critterTraits.SetTarget(target);

					critterTraits.gameObject.SetActive(true);

					return;
				}
				else if (__instance.TryGetComponent(out Beached_CritterTraitsScreen screen))
				{
					screen.Hide();
				}
			}
		}
	}
}
