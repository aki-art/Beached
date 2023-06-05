using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class LegacyModMainPatch
	{
#if ELEMENTS
		[HarmonyPatch(typeof(LegacyModMain), nameof(LegacyModMain.ConfigElements))]
		public class LegacyModMain_ConfigElements_Patch
		{
			public static void Postfix()
			{
				Elements.AddAttributeModifiers();
			}
		}
#endif
	}
}
