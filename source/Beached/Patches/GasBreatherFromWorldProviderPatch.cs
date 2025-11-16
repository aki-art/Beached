using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public class GasBreatherFromWorldProviderPatch
	{
#if TRANSPILERS
		[HarmonyPatch(typeof(GasBreatherFromWorldProvider), "GetBestBreathableCellAroundSpecificCell",
		[
			typeof(int),
						typeof(CellOffset[]),
						typeof(OxygenBreather),
						typeof(float)
		],
		[
			ArgumentType.Normal,
						ArgumentType.Normal,
						ArgumentType.Normal,
						ArgumentType.Out
		])]

		public class GasBreatherFromWorldProvider_GetBestBreathableCellAroundSpecificCell_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_GetBreathableCellMass = AccessTools.DeclaredMethod(typeof(GasBreatherFromWorldProvider), nameof(GasBreatherFromWorldProvider.GetBreathableCellMass));

				if (m_GetBreathableCellMass == null)
				{
					Log.TranspilerIssue("m_GetBreathableCellMass is null");
					return codes;
				}

				var index = codes.FindIndex(ci => ci.Calls(m_GetBreathableCellMass));

				if (index == -1)
				{
					Log.TranspilerIssue("No entry point found");
					return codes;
				}

				var m_IsAmphibious = AccessTools.DeclaredMethod(
					typeof(GasBreatherFromWorldProvider_GetBestBreathableCellAroundSpecificCell_Patch),
					nameof(IsAmphibious),
					[typeof(OxygenBreather)]);

				if (m_IsAmphibious == null)
				{
					Log.TranspilerIssue("m_IsAmphibious is null");
					return codes;
				}

				var m_AmphibiousCheck = AccessTools.DeclaredMethod(
					typeof(AmphibiousOxygenBreatherProvider),
					nameof(AmphibiousOxygenBreatherProvider.GetBreathableCellMass));

				if (m_AmphibiousCheck == null)
				{
					Log.TranspilerIssue("m_AmphibiousCheck is null");
					return codes;
				}

				var afterCheck = gen.DefineLabel();
				var originalCheck = gen.DefineLabel();

				codes[index].labels.Add(originalCheck); // original O2 check
				codes[index + 1].labels.Add(afterCheck);

				var m_Original = AccessTools.DeclaredMethod(
					typeof(GasBreatherFromWorldProvider),
					nameof(GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell),
					[
						typeof(int), typeof(CellOffset[]), typeof(OxygenBreather), typeof(float).MakeByRefType()
						]);

				if (m_Original == null)
				{
					Log.TranspilerIssue("m_Original is null");
					return codes;
				}

				var breatherParamIdx = m_Original.GetParameters().ToList().FindIndex(p => p.ParameterType == typeof(OxygenBreather));

				if (breatherParamIdx == -1)
				{
					Log.Warning("Transpiler issue: GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell does not have an OxygenBreather parameter.");

					return codes;
				}

				// inject right after the found index
				codes.InsertRange(index,
				[
					new CodeInstruction(OpCodes.Ldarg, breatherParamIdx), // OxygenBreather
						new CodeInstruction(OpCodes.Call, m_IsAmphibious), // is amphibious, puts bool on stack
						new CodeInstruction(OpCodes.Brfalse, originalCheck),
						new CodeInstruction(OpCodes.Call, m_AmphibiousCheck),
						new CodeInstruction(OpCodes.Br, afterCheck),
					]);

				return codes;
			}

			private static bool IsAmphibious(OxygenBreather breather) => breather != null && breather.HasTag(BTags.amphibious);
		}

		[HarmonyPatch(typeof(OxygenBreather), nameof(OxygenBreather.BreathableGasConsumed), [
			typeof(SimHashes),
			typeof(float),
			typeof(float),
			typeof(byte),
			typeof(int),
			])]
		public class OxygenBreather_BreathableGasConsumed_Patch
		{
			// Trigger events for atmospheres
			// liquids are included for Minnow's waterbreathing ability
			public static void Postfix(OxygenBreather __instance, SimHashes elementConsumed, float massConsumed)
			{
				if (elementConsumed == Elements.saltyOxygen || elementConsumed == SimHashes.SaltWater)
					__instance.Trigger((int)ModHashes.greatAirQuality, (object)elementConsumed);
				else if (elementConsumed == SimHashes.DirtyWater || elementConsumed == Elements.murkyBrine)
					__instance.BoxingTrigger((int)GameHashes.PoorAirQuality, massConsumed);
			}
		}
	}
#endif
}
