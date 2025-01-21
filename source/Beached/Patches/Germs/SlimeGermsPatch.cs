using Beached.Content;
using HarmonyLib;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

namespace Beached.Patches.Germs
{
	public class SlimeGermsPatch
	{
		[HarmonyPatch(typeof(SlimeGerms), "PopulateElemGrowthInfo")]
		public class SlimeGerms_PopulateElemGrowthInfo_Patch
		{
			public static void Postfix(SlimeGerms __instance)
			{
				__instance.growthRules.Add(new ElementGrowthRule(Elements.saltyOxygen)
				{
					populationHalfLife = new float?(1200f),
					overPopulationHalfLife = new float?(10f)
				});

				__instance.growthRules.Add(new ElementGrowthRule(Elements.mucus)
				{
					underPopulationDeathRate = new float?(0f),
					populationHalfLife = new float?(-3000f),
					overPopulationHalfLife = new float?(3000f),
					maxCountPerKG = new float?(4500),
					diffusionScale = new float?(0.05f)
				});

				__instance.exposureRules.Add(new ElementExposureRule(Elements.saltyOxygen)
				{
					populationHalfLife = new float?(3000f)
				});

				__instance.exposureRules.Add(new ElementExposureRule(Elements.mucus)
				{
					populationHalfLife = new float?(-12000f)
				});
			}
		}
	}
}
