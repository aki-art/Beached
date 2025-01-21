using Beached.Content.ModDb;
using System;
using System.Reflection;

namespace Beached.Patches
{
	public class DoorPatch
	{
		//[HarmonyPatch(typeof(Door), nameof(Door.UpdateAnimAndSoundParams))]
		public class Door_UpdateAnimAndSoundParams_Patch
		{
			public static MethodInfo TargetMethod()
			{
				var type = Type.GetType("Door");

				Debug.Assert(type != null, "type is null");
				return type.GetMethod("UpdateAnimAndSoundParams", [typeof(bool)]);
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
