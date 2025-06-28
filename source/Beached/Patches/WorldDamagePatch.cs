#if TRANSPILERS
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
	public class WorldDamagePatch
	{
		[HarmonyPatch(typeof(WorldDamage), nameof(WorldDamage.OnDigComplete))]
		public class WorldDamage_OnDigComplete_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				List<CodeInstruction> codes = orig.ToList();

				int index = codes.FindIndex(ci => ci.LoadsConstant(0.5f));

				if (index == -1) return codes;

				System.Reflection.MethodInfo m_getActualMassMultiplier = AccessTools.DeclaredMethod(typeof(WorldDamage_OnDigComplete_Patch),
					nameof(GetActualMassMultiplier));

				// inject right after the found index
				codes.InsertRange(index + 1,
				[
					new CodeInstruction(OpCodes.Ldarg_1), // cell
					new CodeInstruction(OpCodes.Call, m_getActualMassMultiplier)
				]);

				return codes;
			}

			// TODO: Dwarf digging integration
			private static float GetActualMassMultiplier(float originalValue, int cell)
			{
				float multiplier = originalValue;

				if (Treasury.diggers.TryGetValue(cell, out WorkerBase worker))
				{
					if (worker.TryGetComponent(out MinionResume resume))
					{
						if (resume.AptitudeBySkillGroup.TryGetValue(BSkillGroups.PRECISION_ID, out float mineralogySkill))
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
#endif