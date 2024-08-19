using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
	public class MinionPesonalityPanelPatch
	{
		// TODO
		//[HarmonyPatch(typeof(MinionPersonalityPanel), nameof(MinionPersonalityPanel.RefreshTraits))]
		public class MinionPersonalityPanel_RefreshTraits_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var m_GetTooltip = AccessTools.Method(typeof(Trait), nameof(Trait.GetTooltip));

				var index = codes.FindIndex(ci => ci.Calls(m_GetTooltip));

				if (index == -1)
					return codes;

				var f_selectedTarget = AccessTools.Field(typeof(TargetScreen), "selectedTarget");

				var m_GetToolTip = AccessTools.Method(typeof(MinionPersonalityPanel_RefreshTraits_Patch), nameof(GetToolTip),
				[
					typeof(string),
					typeof(Trait),
					typeof(GameObject)
				]);

				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldloc_3), // trait (iterator) was 1
                    new CodeInstruction(OpCodes.Ldarg_0), // this
                    new CodeInstruction(OpCodes.Ldfld, f_selectedTarget), // .selectedTarget
                    new CodeInstruction(OpCodes.Call, m_GetToolTip) // GetToolTip(str, trait, this.selectedTarget);
                });

				return codes;
			}

			private static string GetToolTip(string tooltip, Trait trait, GameObject target)
			{
				if (BTraits.LIFE_GOALS.Contains(trait.Id))
				{
					if (target.TryGetComponent(out Beached_LifeGoalTracker storage))
					{
						foreach (var item in storage.fulfilledLifegoalModifiers)
						{
							tooltip += "\n";
							tooltip += string.Format(global::STRINGS.UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(item.Value.ToString(), true), item.GetName());
							tooltip += storage.isGoalFulfilled ? " (Active)" : " (Inactive)";
						}
					}
				}

				return tooltip;
			}
		}
	}
}
