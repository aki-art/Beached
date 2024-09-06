using Beached.Content.Scripts;
using Beached.Content.Scripts.UI;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class SimpleInfoScreenPatch
	{
		public static void Patch(Harmony harmony)
		{
			Log.Debug("patching critter traits panel");

			var prefix = AccessTools.DeclaredMethod(typeof(SimpleInfoScreen_OnSelectTarget_Patch), "Prefix");
			var targetMethod = AccessTools.DeclaredMethod(typeof(SimpleInfoScreen), nameof(SimpleInfoScreen.OnSelectTarget));

			harmony.Patch(targetMethod, new HarmonyMethod(prefix));
		}

		public class SimpleInfoScreen_OnSelectTarget_Patch
		{
			private static bool HasBeachedAddedTraits(GameObject go) => go.TryGetComponent(out CreatureBrain _)
				|| go.TryGetComponent(out Beached_GeyserTraits _);


			public static void Prefix(ref SimpleInfoScreen __instance, GameObject target)
			{
				if (HasBeachedAddedTraits(target))
				{
					var traits = __instance.gameObject.AddOrGet<Beached_AdditionalEntitiesTraitsScreen>();
					traits.Initialize(__instance);
					traits.SetTarget(target);

					traits.gameObject.SetActive(true);

					return;
				}
				else if (__instance.TryGetComponent(out Beached_AdditionalEntitiesTraitsScreen screen))
				{
					screen.Hide();
				}
			}
		}
	}
}
