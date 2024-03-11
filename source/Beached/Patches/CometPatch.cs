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

				if (!Beached_Grid.forceFieldLevelPerWorld.TryGetValue(myWorldId, out var forceFieldY))
					return;

				if (forceFieldY == Beached_Grid.INVALID_FORCEFIELD_OFFSET)
					return;

				Grid.PosToXY(__instance.transform.position, out _, out var y);

				if (y <= forceFieldY && __instance.TryGetComponent(out PrimaryElement primaryElement))
				{
					var cell = Grid.PosToCell(__instance);
					var previousCell = Grid.PosToCell(__instance.previousPosition);
					__instance.Explode(__instance.transform.GetPosition(), cell, previousCell, primaryElement.Element);
					__instance.hasExploded = true;

					if (__instance.destroyOnExplode)
						Util.KDestroyGameObject(__instance.gameObject);
				}
			}
		}
	}
}
