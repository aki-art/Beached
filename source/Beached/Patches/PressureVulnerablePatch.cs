using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class PressureVulnerablePatch
	{
		[HarmonyPatch(typeof(PressureVulnerable), "SlicedSim1000ms")]
		public class PressureVulnerable_SlicedSim1000ms_Patch
		{
			public static void Postfix(PressureVulnerable __instance)
			{
				if (__instance.HasTag(BTags.acidImmune))
					return;

				if (__instance.currentAtmoElement != null && __instance.currentAtmoElement.id == Elements.sulfurousWater)
				{
					if (__instance.TryGetComponent(out Uprootable uprootable))
						uprootable.Uproot();
				}
			}
		}

		[HarmonyPatch(typeof(PressureVulnerable), "IsSafeElement")]
		public class PressureVulnerable_IsSafeElement_Patch
		{
			public static void Postfix(PressureVulnerable __instance, ref bool __result)
			{
				if (__instance.HasTag(BTags.aquariumPlanted))
					__result = true;
			}
		}
	}
}
