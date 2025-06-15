#if TRANSPILERS
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public class AssignableSideScreenRowPatch
	{
		[HarmonyPatch(typeof(AssignableSideScreenRow), nameof(AssignableSideScreenRow.GetTooltip))]
		public class AssignableSideScreenRow_GetTooltip_Patch
		{
			// changes the tooltip for Capped dupes, when tring to assign a helmet (Capped disables helmets)
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var f_DISABLED_TOOLTIP = typeof(global::STRINGS.UI.UISIDESCREENS.ASSIGNABLESIDESCREEN).GetField("DISABLED_TOOLTIP", BindingFlags.Static | BindingFlags.Public);
				var index = codes.FindIndex(ci => ci.LoadsField(f_DISABLED_TOOLTIP));

				if (index == -1)
					return codes;

				var m_GetNewString = AccessTools.Method(
					typeof(AssignableSideScreenRow_GetTooltip_Patch),
					nameof(GetNewString),
					[typeof(string), typeof(AssignableSideScreen)]);

				var f_sideSreen = typeof(AssignableSideScreenRow).GetField("sideScreen");

				codes.InsertRange(index + 1, new[]
				{
					// string is on stack
					new CodeInstruction(OpCodes.Ldarg_0), // load instance
					new CodeInstruction(OpCodes.Ldfld, f_sideSreen), // load sideScreen
					new CodeInstruction(OpCodes.Call, m_GetNewString)
				});

				return codes;
			}

			private static string GetNewString(string originalEntry, AssignableSideScreen sideScreen)
			{
				return sideScreen.targetAssignable != null && sideScreen.targetAssignable.GetComponent<HelmetController>() != null
					? STRINGS.UI.BEACHED_MISC.CAPPED
					: originalEntry;
			}
		}
	}
}

#endif
