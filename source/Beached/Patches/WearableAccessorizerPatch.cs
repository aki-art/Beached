using Beached.Content.ModDb;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public static class WearableAccessorizerPatch
	{
		[HarmonyPatch(typeof(WearableAccessorizer), "ApplyEquipment")]
		public static class WearableAccessorizer_ApplyEquipment_Patch
		{
			private static readonly Dictionary<string, WearableAccessorizer.WearableType> wearableTypes = new()
			{
				{ BAssignableSlots.JEWELLERY_ID, BAccessories.WearableTypes.necklace },
				{ BAssignableSlots.SHOES_ID, BAccessories.WearableTypes.shoes },
			};

			public static void Postfix(WearableAccessorizer __instance, Equippable equippable, KAnimFile animFile)
			{
				if (equippable != null && wearableTypes.TryGetValue(equippable.def.Slot, out WearableAccessorizer.WearableType type))
				{
					if (__instance.wearables.ContainsKey(type))
					{
						WearableAccessorizer.Wearable wearable = __instance.wearables[type];
						__instance.RemoveAnimBuild(wearable.BuildAnims[0], wearable.buildOverridePriority);
					}

					__instance.wearables[type] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);

					__instance.ApplyWearable();
				}
			}
		}

		[HarmonyPatch(typeof(WearableAccessorizer), "ApplyWearable")]
		public static class WearableAccessorizer_ApplyWearable_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				List<CodeInstruction> codes = orig.ToList();

				System.Reflection.MethodInfo m_GetValues = AccessTools.Method(typeof(Enum), "GetValues", [typeof(Type)]);

				// find injection point
				int index = codes.FindIndex(ci => ci.Calls(m_GetValues));

				if (index == -1)
				{
					Log.Warning("Could not patch WearableAccessorizer.ApplyWearable");
					return codes;
				}

				System.Reflection.MethodInfo m_AddToEnumValues = AccessTools.DeclaredMethod(typeof(WearableAccessorizer_ApplyWearable_Patch), nameof(AddToEnumValues));

				// inject right after the found index
				codes.InsertRange(index + 1,
				[
					new CodeInstruction(OpCodes.Call, m_AddToEnumValues)
				]);

				return codes;
			}

			private static Array AddToEnumValues(Array enums)
			{
				return enums
					.Cast<WearableAccessorizer.WearableType>()
					.AddItem(BAccessories.WearableTypes.necklace)
					.AddItem(BAccessories.WearableTypes.shoes)
					.ToArray();
			}
		}
	}
}
