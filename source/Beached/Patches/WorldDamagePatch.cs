using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
	internal class WorldDamagePatch
	{
		[HarmonyPatch(typeof(WorldDamage), nameof(WorldDamage.OnDigComplete))]
		public class WorldDamage_OnDigComplete_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var index = codes.FindIndex(ci => ci.LoadsConstant(0.5f));

				if (index == -1) return codes;

				var m_getActualMassMultiplier = AccessTools.DeclaredMethod(typeof(WorldDamage_OnDigComplete_Patch),
					nameof(GetActualMassMultiplier));

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldarg_1), // cell
                    new CodeInstruction(OpCodes.Call, m_getActualMassMultiplier)
				});

				return codes;
			}

			// TODO: Dwarf digging integration
			private static float GetActualMassMultiplier(float originalValue, int cell)
			{
				var multiplier = originalValue;

				if (Treasury.diggers.TryGetValue(cell, out var worker))
				{
					if (worker.TryGetComponent(out MinionResume resume))
					{
						if (resume.AptitudeBySkillGroup.TryGetValue(BSkillGroups.PRECISION_ID, out var mineralogySkill))
						{
							multiplier += mineralogySkill * 0.025f;
							multiplier = Mathf.Clamp01(multiplier);
						}
					}
				}

				return multiplier;
			}
		}
	}
}
