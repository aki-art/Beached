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

				var m_InjectedMethod = AccessTools.Method(typeof(FetchManager_FindEdibleFetchTarget_Patch), nameof(GetPathCost),
				[
					typeof(ushort),
					typeof(FetchManager.Pickup),
					typeof(Storage)
				]);

				codes.InsertRange(index + 1,
				[
					// ushort on stack
					new CodeInstruction(OpCodes.Ldloc, 5), // TODO find, pickup2
					new CodeInstruction(OpCodes.Ldarg_1), // Storage storage
					new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				]);

				return codes;
			}

			private static ushort GetPathCost(ushort originalValue, FetchManager.Pickup pickupable, Storage storage)
			{
				if (pickupable.pickupable == null)
				{
					Log.Warning("pickupable is null");
					return originalValue;
				}

				if (pickupable.pickupable.KPrefabID == null)
				{
					Log.Warning(" pickupable.pickupable.KPrefabID is null");
					return originalValue;
				}

				if (storage == null)
				{
					Log.Warning("storage is null");
					return originalValue;
				}

				// TODO: Frosty broke this i think

				var isLowPriorityPoff = pickupable.pickupable.KPrefabID.HasTag(BTags.palateCleanserFood)
					&& storage.HasTag(BTags.palateCleansed);

				if (isLowPriorityPoff)
					return (ushort)(originalValue * 1000);

				var isVegetarianLookingAtMeat = pickupable.pickupable.KPrefabID.HasTag(BTags.meat)
					&& storage.HasTag(BTags.vegetarian);

				if (isVegetarianLookingAtMeat)
					return (ushort)(originalValue * 1000);

				return originalValue;
			}
		}

#endif
	}
}
