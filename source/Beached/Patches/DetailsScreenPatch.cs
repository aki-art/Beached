using Beached.Content.Scripts.UI;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class DetailsScreenPatch
	{
		[HarmonyPatch(typeof(DetailsScreen), nameof(DetailsScreen.OnPrefabInit))]
		public static class DetailsScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				SideScreenUtil.AddClonedSideScreen<Beached_SampleSelectorSidesScreen>("BeachedDNAInjectorSideScreen", typeof(ConfigureConsumerSideScreen));
				SideScreenUtil.AddCustomSideScreen<Beached_SimplifiedFilterSideScreen>("BeachedSimplifiedFilterableSideScreen", Object.Instantiate(ModAssets.Prefabs.universalSidescreen));
				SideScreenUtil.AddCustomSideScreen<Beached_CollarDispenserSidescreen>("BeachedCollarDispenserSideScreen", Object.Instantiate(ModAssets.Prefabs.universalSidescreen));
				SideScreenUtil.AddCustomSideScreen<Beached_RubberTappableSidescreen>("BeachedRubberTappableSideScreen", Object.Instantiate(ModAssets.Prefabs.universalSidescreen));
				SideScreenUtil.AddClonedSideScreen<Beached_CrystalClusterSideScreen>("BeachedCrystalClusterSideScreen", typeof(ReceptacleSideScreen), true);
				SideScreenUtil.AddCustomSideScreen<Beached_MuffinSideScreen>("BeachedMuffinSideScreen", Object.Instantiate(ModAssets.Prefabs.muffinSideScreen));
			}
		}

		[HarmonyPatch(typeof(DetailsScreen), "OnNameChanged")]
		public class DetailsScreen_OnNameChanged_Patch
		{
			public static void Prefix(DetailsScreen __instance, ref string newName)
			{
				Log.Debug($"on rename {newName}");
				if (__instance.target == null)
					return;

				Log.Debug($"not null");
				if (__instance.target.TryGetComponent(out Beached_CritterNameOverlay _) && newName.IsNullOrWhiteSpace())
				{
					Log.Debug($"has component & newName is nullws");
					newName = Assets.GetPrefab(__instance.target.PrefabID()).GetProperName();
					Log.Debug("changed name frm null to newName");
				}
			}
		}
	}
}
