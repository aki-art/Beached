using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class DoorPatch
	{
		public class Door_UpdateAnimAndSoundParams_Patch
		{
			// manually patched because Door.OVERRIDE_ANIMS is a static reference to Assets.GetAnim, and directly patching crashes
			public static void Patch(Harmony harmony)
			{
				var method = AccessTools.Method("Door:UpdateAnimAndSoundParams", [typeof(bool)]);
				var postfix = AccessTools.Method(typeof(Door_UpdateAnimAndSoundParams_Patch), "Postfix", [typeof(Door)]);
				harmony.Patch(method, postfix: new HarmonyMethod(postfix));
			}

			public static void Postfix(Door __instance)
			{
				var speedMod = BAttributes.doorOpeningSpeed.Lookup(__instance);
				if (speedMod != null)
					__instance.animController.PlaySpeedMultiplier *= speedMod.GetTotalValue();
			}
		}
	}
}
