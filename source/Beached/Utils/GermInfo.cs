
using Klei.AI.DiseaseGrowthRules;
using TUNING;

namespace Beached.Utils
{
	public class GermUtil
	{
		public ElementGrowthRule GrowFastIn(SimHashes element)
		{
			return new ElementGrowthRule(element)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_1),
				maxCountPerKG = new float?(10000),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_3),
				diffusionScale = new float?(0.05f)
			};
		}
		public ElementGrowthRule DisinfectedBy(SimHashes element)
		{
			return new ElementGrowthRule(SimHashes.ChlorineGas)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				minDiffusionCount = new int?(100000),
				diffusionScale = new float?(0.001f)
			};
		}

		public ElementExposureRule HalfLifeOnElement(SimHashes element, float halflife)
		{
			return new ElementExposureRule(element)
			{
				populationHalfLife = new float?(halflife)
			};
		}

		public ElementGrowthRule PersistOn(SimHashes element)
		{
			return new ElementGrowthRule(element)
			{
				populationHalfLife = new float?(float.PositiveInfinity),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_1)
			};
		}

		public StateGrowthRule SlowlyDieOnSolid()
		{
			return new StateGrowthRule(Element.State.Solid)
			{
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(3000f),
				overPopulationHalfLife = new float?(1200f),
				diffusionScale = new float?(1E-06f),
				minDiffusionCount = new int?(1000000)
			};
		}

		public StateGrowthRule GrowOnSolid()
		{
			return new StateGrowthRule(Element.State.Solid)
			{
				underPopulationDeathRate = new float?(0f),
				populationHalfLife = new float?(-3000f),
				overPopulationHalfLife = new float?(3000f),
				maxCountPerKG = new float?(4500),
				diffusionScale = new float?(0.05f)
			};
		}
	}
}
