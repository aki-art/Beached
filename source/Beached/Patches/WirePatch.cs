using Beached.Content.Defs.Buildings;
using HarmonyLib;

namespace Beached.Patches
{
	public class WirePatch
	{
		[HarmonyPatch(typeof(Wire), "GetMaxWattageAsFloat")]
		public class Wire_GetMaxWattageAsFloat_Patch
		{
			public static void Postfix(Wire.WattageRating rating, ref float __result)
			{
				if (rating == MediumWattageWireConfig.W4000)
					__result = MediumWattageWireConfig.WATTAGE;
			}
		}
	}
}
