using Beached.Content;
using Beached.Content.Scripts.Buildings;
using HarmonyLib;

namespace Beached.Patches
{
	public class DoorPatch
	{
		[HarmonyPatch(typeof(Door), nameof(Door.Open))]
		public class Door_Open_Patch
		{
			public static void Postfix(Door __instance)
			{
				if (__instance.HasTag(BTags.lubricated) &&
					__instance.worker != null &&
					__instance.TryGetComponent(out Lubricatable lubricatable))
				{
					lubricatable.OnUse();
				}
			}
		}
	}
}
