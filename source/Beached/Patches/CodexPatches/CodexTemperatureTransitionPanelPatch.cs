using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches.CodexPatches
{
	public class CodexTemperatureTransitionPanelPatch
	{
		// some trickery to make Beached_PermaFrost_Transitional integrate into the codex seamlessly
		[HarmonyPatch(typeof(CodexTemperatureTransitionPanel), "ConfigureResults")]
		public class CodexTemperatureTransitionPanel_ConfigureResults_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var f_highTempTransitionOreId = typeof(Element).GetField(nameof(Element.highTempTransitionOreID));

				var index = codes.FindIndex(ci => ci.LoadsField(f_highTempTransitionOreId));

				if (index == -1)
					return codes;

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(CodexTemperatureTransitionPanel_ConfigureResults_Patch), nameof(CheckResult));

				codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, m_InjectedMethod));

				return codes;
			}

			private static SimHashes CheckResult(SimHashes target) => target == Elements.permaFrost_Transitional ? SimHashes.Fossil : target;
		}
	}
}
