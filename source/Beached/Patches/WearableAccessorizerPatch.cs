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
			public static void Postfix(WearableAccessorizer __instance, Equippable equippable, KAnimFile animFile)
			{
				if (equippable != null && equippable.def.Slot == BAssignableSlots.JEWELLERY_ID)
				{
					var necklace = BAccessories.WearableTypes.necklace;

					if (__instance.wearables.ContainsKey(necklace))
					{
						var wearable = __instance.wearables[necklace];
						__instance.RemoveAnimBuild(wearable.BuildAnims[0], wearable.buildOverridePriority);
					}

					__instance.wearables[necklace] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);

					__instance.ApplyWearable();
				}
			}
		}

		[HarmonyPatch(typeof(WearableAccessorizer), "ApplyWearable")]
		public static class WearableAccessorizer_ApplyWearable_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_GetValues = AccessTools.Method(typeof(Enum), "GetValues", [typeof(Type)]);

				// find injection point
				var index = codes.FindIndex(ci => ci.Calls(m_GetValues));

				if (index == -1)
				{
					Log.Warning("Could not patch WearableAccessorizer.ApplyWearable");
					return codes;
				}

				var m_AddToEnumValues = AccessTools.DeclaredMethod(typeof(WearableAccessorizer_ApplyWearable_Patch), nameof(AddToEnumValues));

				if (m_AddToEnumValues == null)
					Log.Warning("method is null");


				// inject right after the found index
				codes.InsertRange(index + 1,
				[
					new CodeInstruction(OpCodes.Call, m_AddToEnumValues)
				]);

				FUtility.Log.PrintInstructions(codes);

				return codes;
			}

			private static Array AddToEnumValues(Array enums)
			{
				return enums
					.Cast<WearableAccessorizer.WearableType>()
					.AddItem(BAccessories.WearableTypes.necklace)
					.ToArray();
			}
		}
	}
}
