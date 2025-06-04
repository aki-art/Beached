using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class SublimatesPatch
	{

		[HarmonyPatch(typeof(Sublimates), "Emit")]
		public class Sublimates_Emit_Patch
		{
			public static void Postfix(Sublimates __instance, float mass)
			{
				if (mass > 0)
					__instance.Trigger(ModHashes.sublimated);
			}
		}
	}
}

