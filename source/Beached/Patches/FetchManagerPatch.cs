using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
namespace Beached.Patches
{
	public class FetchManagerPatch
	{
#if TRANSPILERS

		// makes poffs the last food option to find
		[HarmonyPatch(typeof(FetchManager), nameof(FetchManager.FindEdibleFetchTarget))]
		public class FetchManager_FindEdibleFetchTarget_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var f_PathCost = typeof(FetchManager.Pickup).GetField(nameof(FetchManager.Pickup.PathCost));
				var index = codes.FindLastIndex(ci => ci.LoadsField(f_PathCost));

				if (index == -1)
					return codes;

				var m_InjectedMethod = AccessTools.Method(typeof(FetchManager_FindEdibleFetchTarget_Patch), nameof(GetPathCost), new[]
				{
					typeof(ushort),
					typeof(FetchManager.Pickup),
					typeof(Storage)
				});

				codes.InsertRange(index + 1, new[]
				{
                    // ushort on stack
                    new CodeInstruction(OpCodes.Ldloc, 4), // TODO find, pickup2
                    new CodeInstruction(OpCodes.Ldarg_1), // Storage storage
                    new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static ushort GetPathCost(ushort originalValue, FetchManager.Pickup pickupable, Storage storage)
			{
				var isLowPriorityPoff = pickupable.pickupable.KPrefabID.HasTag(BTags.palateCleanserFood)
					&& storage.HasTag(BTags.palateCleansed);

				return (ushort)(isLowPriorityPoff
					? originalValue * 1000
					: originalValue);
			}
		}

#endif
	}
}
