using Beached.Content.Scripts.UI;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Patches
{
	public class ScheduledUIInstantiationPatch
	{
		private static GameObject critterIdentityScreen;

		[HarmonyPatch(typeof(ScheduledUIInstantiation), "OnPrefabInit")]
		public class ScheduledUIInstantiation_OnPrefabInit_Patch
		{
			public static void Prefix(ScheduledUIInstantiation __instance)
			{
				if (__instance.completed)
					return;

				if (__instance.UIElements.Any(e => e.Name == "Beached_FullScreens"))
					return;

				if (ModAssets.Prefabs.critterIdentitySidescreen == null)
				{
					Log.Warning("prefab is null!!!");
					return;
				}

				critterIdentityScreen = Object.Instantiate(ModAssets.Prefabs.critterIdentitySidescreen);
				critterIdentityScreen.AddOrGet<CanvasScaler>();
				critterIdentityScreen.AddComponent<KCanvasScaler>();

				var screen = critterIdentityScreen.AddComponent<Beached_CritterIdentityScreen>();

				screen.name = "Beached_CritterIdentityScreen";

				Transform parent = null;

				foreach (var item in __instance.UIElements)
				{
					Log.Debug($"{item.Name} | {item.Comment} | {item.prefabs?.Select(prefab => prefab.name).Join()}");

					if (item.Name == "Full Management Screens")
						parent = item.parent;
				}

				if (parent != null)
				{
					__instance.UIElements = __instance.UIElements.AddToArray(new ScheduledUIInstantiation.Instantiation()
					{
						parent = parent,
						prefabs = [screen.gameObject],
						RequiredDlcId = DlcManager.VANILLA_ID,
						Name = "Beached_FullScreens",
						Comment = "."
					});
				}
			}
		}
	}
}
