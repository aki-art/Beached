using Beached.Content.Scripts;
using HarmonyLib;
using Neutronium.PostProcessing.LUT;
using static Beached.ModAssets;

namespace Beached.Patches
{
	public class CameraControllerPatch
	{
		// replaces the LUT with a more vibrant, colorful overlay
		[HarmonyPatch(typeof(CameraController), nameof(CameraController.OnPrefabInit))]
		public class CameraController_OnPrefabInit_Patch
		{
			[HarmonyPriority(Priority.High)]
			public static void Prefix()
			{
				var useCustomLUT = Beached_WorldLoader.Instance.IsBeachedContentActive || Mod.settings.CrossWorld.UseVibrantLUTEverywhere;

				if (useCustomLUT)
					Mod.lutAPI.RegisterLUT("Beached_VibrantDayLUT", LUTSlot.Day, 300, Textures.LUTDay);
				else
					Mod.lutAPI.UnregisterLUT("Beached_VibrantDayLUT");
			}
		}
	}
}
