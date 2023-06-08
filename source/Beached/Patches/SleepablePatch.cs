using Beached.Content.Scripts;
using HarmonyLib;
using Klei.AI;

namespace Beached.Patches
{
	public class SleepablePatch
	{
		[HarmonyPatch(typeof(Sleepable), nameof(Sleepable.OnPrefabInit))]
		public class Sleepable_OnPrefabInit_Patch
		{
			public static void Postfix(Sleepable __instance)
			{
				if (__instance.GetComponent<Assignable>() != null)
				{
					__instance.gameObject.AddComponent<TargetOfGoalTracker>().targetTag = MechanicalSurfboardConfig.ID;
					__instance.gameObject.AddComponent<TargetOfGoalTracker>().targetTag = ItemPedestalConfig.ID;

					__instance.gameObject.AddOrGet<Beached_PlushiePlaceable>();
				}
			}
		}

		[HarmonyPatch(typeof(Sleepable), nameof(Sleepable.OnStopWork))]
		public class Sleepable_OnStopWork_Patch
		{
			public static void Postfix(Sleepable __instance)
			{
				if (__instance.worker == null)
					return;

				if (__instance.TryGetComponent(out Beached_PlushiePlaceable plushie)
					&& __instance.worker.TryGetComponent(out Effects effects))
					effects.Add(plushie.GetEffectId(), true);
			}
		}
	}
}
