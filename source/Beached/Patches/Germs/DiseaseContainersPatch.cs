using HarmonyLib;
using Klei.AI;

namespace Beached.Patches.Germs
{
	public class DiseaseContainersPatch
	{
		[HarmonyPatch(
			typeof(DiseaseContainers),
			"CalculateDelta",
			new[]
			{
				typeof(DiseaseHeader),
				typeof(DiseaseContainer),
				typeof(Disease),
				typeof(float),
				typeof(bool) },
			new[]
			{
				ArgumentType.Normal,
				ArgumentType.Ref,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal
			})]
		public class DiseaseContainers_CalculateDelta_Patch
		{
			public static void Postfix(ref DiseaseContainer container, ref float __result)
			{
				if (container.autoDisinfectable == null)
				{
					return;
				}

				var pos = Grid.PosToCell(container.autoDisinfectable);
				if (Grid.LightCount[pos] == 0)
				{
					__result *= 100f; // ridiculous number for testins
				}
			}
		}

		[HarmonyPatch(typeof(DiseaseContainers), "EvaluateGrowthConstants")]
		public class DiseaseContainers_EvaluateGrowthConstants_Patch
		{
			public static void Postfix(DiseaseHeader header, ref DiseaseContainer container)
			{
				if (header.primaryElement == null)
				{
					return;
				}

				var pos = Grid.PosToCell(header.primaryElement);
				if (Grid.LightCount[pos] == 0)
				{
					container.instanceGrowthRate *= 100f; // ridiculous number for testins
				}
			}
			/*            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
                        {
                            var codes = orig.ToList();

                            var m_GetGrowthRateForTags = AccessTools.Method(typeof(Disease), "GetGrowthRateForTags", new[] { typeof(HashSet<Tag>), typeof(bool) });
                            var index = codes.FindIndex(ci => ci.Calls(m_GetGrowthRateForTags)); 

                            if (index == -1)
                            {
                                Log.Warning("Could not patch DiseaseContainer.EvaluateGrowthConstants: GetGrowthRateForTags is never called.");
                                return codes;
                            }

                            var m_InsertTagsForLux = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch), "GetDangerForElement", new[] { typeof(int), typeof(int) });

                            codes.InsertRange(index, new[]
                            {
                                // byte.maxValue is loaded to the stack
                                new CodeInstruction(OpCodes.Ldloc_2), // load num to the stack
                                new CodeInstruction(OpCodes.Call, m_InsertTagsForLux)
                            });

                            return codes;
                        }

                        // Calling with existing value so there is a possibility for other mods to also add their own values
                        private static byte GetDangerForElement(int existingValue, int cell)
                        {
                            return (Grid.Element[cell].id == Elements.SaltyOxygen) ? (byte)0 : (byte)existingValue;
                        }*/
		}
	}
}
