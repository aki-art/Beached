using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class DoorPatch
	{
		[HarmonyPatch(typeof(Door), nameof(Door.UpdateAnimAndSoundParams))]
		public class Door_UpdateAnimAndSoundParams_Patch
		{
			public static void Postfix(Door __instance)
			{
				var speedMod = BAttributes.doorOpeningSpeed.Lookup(__instance);
				if (speedMod != null)
					__instance.animController.PlaySpeedMultiplier *= speedMod.GetTotalValue();
			}
		}
	}
}
