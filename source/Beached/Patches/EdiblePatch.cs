using Beached.Content;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	public class EdiblePatch
	{
		// patched manually, because this method references SpiceGrinderConfig, which then references Db.Get(), which causes a crash if called too early
		public class Edible_OnSpawn_Patch
		{
			public static void Patch(Harmony harmony)
			{
				var targetMethod = AccessTools.Method("Edible:OnSpawn", []);
				var postfix = AccessTools.Method(typeof(Edible_OnSpawn_Patch), nameof(Postfix));

				harmony.Patch(targetMethod, postfix: new HarmonyMethod(postfix));
			}

			public static void Postfix(Edible __instance)
			{
				if (__instance.HasTag(BTags.meat))
					__instance.GetComponent<KSelectable>().AddStatusItem(BStatusItems.meat);
			}
		}

		public class Edible_AddOnConsumeEffects_Patch
		{
			// patched later, because his method calls a static property with a getter that references Db.Get(), which crashes if called too early
			public static void Patch(Harmony harmony)
			{
				var targetMethod = AccessTools.Method("Edible:AddOnConsumeEffects", [typeof(Worker)]);
				var postfix = AccessTools.Method(typeof(Edible_AddOnConsumeEffects_Patch), nameof(Postfix));

				harmony.Patch(targetMethod, postfix: new HarmonyMethod(postfix));
			}

			public static void Postfix(Edible __instance, Worker worker)
			{
				if (worker == null)
					return;

				if (!worker.TryGetComponent(out Beached_MinionStorage minionStorage))
					return;

				if (__instance.HasTag(BTags.palateCleanserFood))
				{
					minionStorage.OnPalateCleansed();
				}

				if (__instance.HasTag(BTags.meat) && minionStorage.HasTag(BTags.vegetarian))
				{
					minionStorage.OnUnsavoryMealConsumed();
				}
			}
		}
	}
}
