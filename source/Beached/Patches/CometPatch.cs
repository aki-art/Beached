using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	public class CometPatch
	{
		// comets check for a force field and explode on contact.
		[HarmonyPatch(typeof(Comet), nameof(Comet.Sim33ms))]
		public class Comet_Sim33ms_Patch
		{
			public static void Postfix(Comet __instance)
			{
				if (__instance.hasExploded)
					return;

				var myWorldId = __instance.GetMyWorldId();

				if (Beached_Mod.Instance.forceFields.TryGetValue(myWorldId, out var forceField)
					&& forceField.IsIntersecting(__instance.gameObject))
				{
					__instance.Explode();
				}
			}
		}
	}
}
