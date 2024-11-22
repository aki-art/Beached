using HarmonyLib;
using System;

namespace Beached.Patches
{
	public class LocStringPatch
	{
		[HarmonyPatch(typeof(LocString), "GetStrings")]
		public class LocString_GetStrings_Patch
		{
			public static void Postfix(Type type, ref string[] __result)
			{
				if (type == typeof(global::STRINGS.NAMEGEN.COLONY.ADJECTIVE))
					__result = __result.AddRangeToArray(LocString.GetStrings(typeof(STRINGS.NAMEGEN.COLONY.ADJECTIVE)));
				else if (type == typeof(global::STRINGS.NAMEGEN.COLONY.NOUN))
					__result = __result.AddRangeToArray(LocString.GetStrings(typeof(STRINGS.NAMEGEN.COLONY.NOUN)));
			}
		}
	}
}
