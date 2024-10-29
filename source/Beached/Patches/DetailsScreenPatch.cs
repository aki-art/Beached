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

			}
		}
	}
}
