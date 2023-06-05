using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class SafeCellQueryPatch
	{
		[HarmonyPatch(typeof(SafeCellQuery), nameof(SafeCellQuery.GetFlags))]
		public class SafeCellQuery_GetFlags_Patch
		{
			public static void Postfix(MinionBrain brain, ref SafeCellQuery.SafeFlags __result)
			{
				if (brain.HasTag(BTags.amphibious))
				{
					var breathableWater = (__result & SafeCellQuery.SafeFlags.IsBreathable) == __result;

					if (breathableWater)
					{
						__result |= SafeCellQuery.SafeFlags.IsNotLiquid;
						__result |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
					}
				}
			}
		}
	}
}
