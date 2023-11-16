using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	public class EdiblePatch
	{
		public class Edible_AddOnConsumeEffects_Patch
		{
			// patched later, because his method calls a static property with a getter that references Db.Get(), which crashes if called too early
			public static void Patch(Harmony harmony)
			{
				var targetMethod = AccessTools.Method("Edible:AddOnConsumeEffects", new[] { typeof(Worker) });
				var postfix = AccessTools.Method(typeof(Edible_AddOnConsumeEffects_Patch), nameof(Postfix));

				harmony.Patch(targetMethod, postfix: new HarmonyMethod(postfix));
			}

			public static void Postfix(Edible __instance, Worker worker)
			{
				if (worker == null)
					return;

				if (__instance.HasTag(BTags.palateCleanserFood))
				{
					if (worker.TryGetComponent(out Beached_MinionStorage minionStorage))
						minionStorage.OnPalateCleansed();
				}
			}
		}
	}
}
