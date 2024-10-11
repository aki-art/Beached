using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches.DatabasePatches
{
	public class ModifierSetPatch
	{
		[HarmonyPatch(typeof(ModifierSet), "Initialize")]
		public static class ModifierSet_Initialize_Patch
		{
			public static void Prefix()
			{
				BFertilityModifiers.Register();
			}

			public static void Postfix(ModifierSet __instance)
			{
				BEffects.Register(__instance);
			}
		}
	}
}
