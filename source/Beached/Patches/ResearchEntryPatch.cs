using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
	public class ResearchEntryPatch
	{
		[HarmonyPatch(typeof(ResearchEntry), "SetTech")]
		public class ResearchEntry_SetTech_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				List<CodeInstruction> codes = orig.ToList();

				System.Reflection.MethodInfo m_GetFreeIcon = AccessTools.Method(typeof(ResearchEntry), nameof(ResearchEntry.GetFreeIcon), []);

				int index = codes.FindIndex(ci => ci.Calls(m_GetFreeIcon));

				if (index == -1)
				{
					Log.Warning("Could not patch ResearchEntry.SetTech. (1)");
					return codes;
				}

				System.Reflection.MethodInfo m_GetComponent = AccessTools.Method(typeof(Component), nameof(Component.GetComponent), [], [typeof(ToolTip)]);

				int index2 = codes.FindIndex(index, ci => ci.Calls(m_GetComponent));

				if (index2 == -1)
				{
					Log.Warning("Could not patch ResearchEntry.SetTech. (2)");
					return codes;
				}

				System.Reflection.MethodInfo m_ModifyDLCIcon = AccessTools.DeclaredMethod(typeof(ResearchEntry_SetTech_Patch), "ModifyDLCIcon");

				System.Reflection.FieldInfo f_description = typeof(TechItem).GetField("description");
				int anchorIndex = codes.FindIndex(c => c.LoadsField(f_description));

				if (anchorIndex == -1)
				{
					Log.TranspilerIssue("Could not patch ResearchEntry.SetTech. (3)");
					return codes;
				}

				int localIndex = MiscUtil.GetLocalIndex(codes[anchorIndex]);

				if (localIndex == -1)
				{
					Log.TranspilerIssue("Could not patch ResearchEntry.SetTech. (3)");
					return codes;
				}

				// inject right after the found index
				codes.InsertRange(index2 + 1,
				[
					// dup tooltip component on stack
					new CodeInstruction(OpCodes.Dup),
					new CodeInstruction(OpCodes.Ldloc, localIndex),
					new CodeInstruction(OpCodes.Call, m_ModifyDLCIcon)
				]);

				return codes;
			}

			private static void ModifyDLCIcon(ToolTip toolTip, TechItem techItem)
			{
				if (toolTip == null)
				{
					Log.Warning("Tooltip is null. ResearchEntry.SetTech/ModifyDLCIcon");
					return;
				}

				if (toolTip.TryGetComponent(out HierarchyReferences hierarchyRefs))
				{
					if (techItem.Id.StartsWith("Beached_"))
					{
						KImage dlcOverlay = hierarchyRefs.GetReference<KImage>("DLCOverlay");
						dlcOverlay.gameObject.SetActive(true);
						//dlcOverlay.sprite = Assets.GetSprite("beached_tech_banner");
						dlcOverlay.color = ModAssets.Colors.beached;
					}
				}
			}
		}
	}
}
