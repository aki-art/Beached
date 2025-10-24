using Beached.Content.Scripts.UI;
using HarmonyLib;

namespace Beached.Patches
{
	internal class ManagementMenuPatch
	{
		private static ManagementMenu.ManagementMenuToggleInfo info;
		private static Beached_CritterIdentityScreen critterIdentityScreen;


		[HarmonyPatch(typeof(ManagementMenu), "ToggleScreen")]
		public class ManagementMenu_ToggleScreen_Patch
		{
			public static void Prefix(ManagementMenu __instance, ManagementMenu.ScreenData screenData)
			{
				if (screenData.toggleInfo == info)
				{
					__instance.CloseActive();
				}
			}
		}
		// CRASH
		//[HarmonyPatch(typeof(ManagementMenu), "OnPrefabInit")]
		/*		public class ManagementMenu_OnPrefabInit_Patch
				{
					public static void Prefix(ManagementMenu __instance)
					{
						info = new ManagementMenu.ManagementMenuToggleInfo(
							"Critters",
							"OverviewUI_beached_critters_nav_icon",
							hotkey: Action.NumActions,
							tooltip: "tooltip");

						var instantiator = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
						critterIdentityScreen = instantiator.GetInstantiatedObject<Beached_CritterIdentityScreen>();

						__instance.fullscreenUIs = __instance.fullscreenUIs.AddToArray(info);
						__instance.AddToggleTooltipForResearch(info, "disabled tooltip");
						info.prefabOverride = Object.Instantiate(__instance.researchButtonPrefab);
						info.prefabOverride.transform.Find("TextContainer/Text").GetComponent<LocText>().text = "Test";

						__instance.ScreenInfoMatch.Add(info, new ManagementMenu.ScreenData()
						{
							screen = critterIdentityScreen,
							toggleInfo = info,
							cancelHandler = null,
							// tabIdx = 0 // seems unused
						});

						__instance.mutuallyExclusiveScreens.Add(critterIdentityScreen);
					}
				}

		[HarmonyPatch(typeof(KIconToggleMenu), "Setup", [typeof(IList<KIconToggleMenu.ToggleInfo>)])]
		public class KIconToggleMenu_Setup_Patch
		{
			public static void Prefix(KIconToggleMenu __instance, IList<KIconToggleMenu.ToggleInfo> toggleInfo)
			{
				Log.Debug("KIConToggleMenu Setup");
				if (__instance is not ManagementMenu)
					return;

				if (toggleInfo.Any(info => info.icon == "OverviewUI_beached_critters_nav_icon"))
					return;

				toggleInfo.Insert(0, info);
			}
		}*/
	}
}
