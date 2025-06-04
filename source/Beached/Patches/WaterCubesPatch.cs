using HarmonyLib;

namespace Beached.Patches
{
	public class WaterCubesPatch
	{
		[HarmonyPatch(typeof(WaterCubes), nameof(WaterCubes.Init))]
		public class WaterCubes_Init_Patch
		{
			public static void Postfix(WaterCubes __instance)
			{
				FadeWaterOpaqueness(__instance);
			}


			private static void FadeWaterOpaqueness(WaterCubes waterCubes)
			{
				waterCubes.material.SetFloat("_BlendScreen", 0.5f);
			}
		}
	}
}
