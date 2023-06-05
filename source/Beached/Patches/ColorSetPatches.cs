using Beached.Content.ModDb.Germs;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	internal class ColorSetPatches
	{
		[HarmonyPatch(typeof(ColorSet), nameof(ColorSet.Init))]
		public static class ColorSet_Init_Patch
		{
			public static void Prefix(Dictionary<string, Color32> ___namedLookup, ref bool __state)
			{
				__state = ___namedLookup == null;
			}

			public static void Postfix(ref Dictionary<string, Color32> ___namedLookup, ref bool __state)
			{
				if (__state)
					BDiseases.AddGermColors(___namedLookup);
			}
		}
	}
}
