using Beached.Content.Scripts.UI;
using HarmonyLib;

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
				SideScreenUtil.AddCustomSideScreen<Beached_SimplifiedFilterSideScreen>("BeachedSimplifiedFilterableSideScreen", ModAssets.Prefabs.universalSidescreen);
				SideScreenUtil.AddCustomSideScreen<Beached_CollarDispenserSidescreen>("BeachedCollarDispenserSideScreen", ModAssets.Prefabs.universalSidescreen);
				SideScreenUtil.AddCustomSideScreen<Beached_RubberTappableSidescreen>("BeachedRubberTappableSideScreen", ModAssets.Prefabs.universalSidescreen);
			}
		}
	}
}
