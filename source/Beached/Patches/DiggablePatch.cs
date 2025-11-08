using HarmonyLib;

namespace Beached.Patches
{
	public class DiggablePatch
	{
		[HarmonyPatch(typeof(Diggable), nameof(Diggable.OnSpawn))]
		public class Diggable_OnSpawn_Patch
		{
			public static void Prefix(Diggable __instance)
			{
			}

			/*
						[HarmonyPatch(typeof(Diggable), "DoDigTick")]
						public class Diggable_DoDigTick_Patch
						{
							public static void Prefix(Diggable __instance, int cell, float dt, WorldDamage.DamageType damageType)
							{
								if (Grid.Damage[cell] > 1.0f)
								{
									if (__instance.worker != null)
									{
									}
								}
							}
						}*/
		}
	}
}
