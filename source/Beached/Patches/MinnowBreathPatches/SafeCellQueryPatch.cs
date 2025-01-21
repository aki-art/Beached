/*#if BIONIC
using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches.MinnowBreathPatches
{
	public class SafeCellQueryPatch
	{
		// todo. add an if
		[HarmonyPatch(typeof(SafeCellQuery), "GetFlags")]
		public class SafeCellQuery_GetFlags_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_GetCell = AccessTools.Method(typeof(GasBreatherFromWorldProvider), nameof(GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell));

				var index = codes.FindIndex(ci => ci.Calls(m_GetCell));

				if (index == -1)
					return codes;

				var m_IsAmphibious = AccessTools.DeclaredMethod(typeof(SafeCellQuery_GetFlags_Patch), nameof(IsAmphibious));
				var m_amphibiousCheck = AccessTools.DeclaredMethod(typeof(AmphibiousOxygenBreatherProvider), nameof(AmphibiousOxygenBreatherProvider.GetBestBreathableCellAroundSpecificCell));

				var afterCheck = gen.DefineLabel();
				var originalCheck = gen.DefineLabel();
				codes[index].labels.Add(originalCheck); // original O2 check
				codes[index + 1].labels.Add(afterCheck);

				// inject right after the found index
				codes.InsertRange(index,
				[
					new CodeInstruction(OpCodes.Ldarg_2), // MinionBrain
					new CodeInstruction(OpCodes.Call, m_IsAmphibious), // is amphibious, puts bool on stack
					new CodeInstruction(OpCodes.Brfalse, originalCheck),
					new CodeInstruction(OpCodes.Call, m_amphibiousCheck),
					new CodeInstruction(OpCodes.Br, afterCheck),
				]);

				return codes;
			}

			private static bool IsAmphibious(MinionBrain minion) => minion.HasTag(BTags.amphibious);
		}
	}
}
#endif
*/