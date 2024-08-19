using Beached.Content.Overlays;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
	public class SimDebugViewPatch
	{
		[HarmonyPatch(typeof(SimDebugView), nameof(SimDebugView.OnPrefabInit))]
		public static class SimDebugView_OnPrefabInit_Patch
		{
			public static void Postfix(Dictionary<HashedString, Func<SimDebugView, int, Color>> ___getColourFuncs)
			{
				___getColourFuncs.Add(ConductionOverlayMode.ID, ConductionOverlayMode.GetColorForCell);
				___getColourFuncs.Add(ElementInteractionsOverlayMode.ID, ElementInteractionsOverlayMode.GetColorForCell);
			}
		}

		[HarmonyPatch(typeof(SimDebugView), nameof(SimDebugView.GetOxygenMapColour))]
		public class SimDebugView_GetOxygenMapColour_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var index = codes.FindIndex(ci => ci.opcode == OpCodes.Ble_Un_S); // <=

				if (index == -1)
					return codes;

				var targetIndex = codes.FindIndex(index, ci => ci.opcode == OpCodes.Beq_S); // ==

				if (targetIndex == -1)
					return codes;

				var f_Element = AccessTools.Field(typeof(Grid), "Element");
				var f_Breathable = AccessTools.Field(typeof(GameTags), "Breathable");
				var m_HasTag = AccessTools.Method(typeof(Element), "HasTag", [typeof(Tag)]);

				var EnoughMassLabel = codes[targetIndex].operand;

				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldsfld, f_Element), // Grid.Element
                    new CodeInstruction(OpCodes.Ldarg_1), // cell
                    new CodeInstruction(OpCodes.Ldelem_Ref), // Grid.Element[cell]
                    new CodeInstruction(OpCodes.Ldsfld, f_Breathable), // GameTags.Breathable
                    new CodeInstruction(OpCodes.Call, m_HasTag), // Grid.Element[cell].HasTag
                    new CodeInstruction(OpCodes.Brtrue, EnoughMassLabel), // Grid.Element[cell].id
                });

				return codes;
			}
		}
	}
}
