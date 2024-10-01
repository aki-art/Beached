using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using TUNING;

namespace Beached.Content.DefBuilders
{
	// just a massive wrapper for some reasonable default presets for germ growth
	// applies to floating in air, and being stuck to items.
	public class GrowthInfoBuilder
	{
		private readonly Disease disease;

		public GrowthInfoBuilder(Disease disease, bool initializeGrowthArray = true)
		{
			this.disease = disease;
			if (initializeGrowthArray)
			{
				disease.InitializeElemGrowthArray(ref disease.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			}
			else
			{
				var elements = ElementLoader.elements;
				disease.elemGrowthInfo = new ElemGrowthInfo[elements.Count];
				for (int i = 0; i < elements.Count; i++)
				{
					disease.elemGrowthInfo[i] = Disease.DEFAULT_GROWTH_INFO;
				}
			}
		}

		public GrowthInfoBuilder DefaultBehavior(
			float underPopulationDeathRate,
			float minCountPerKG,
			float maxCountPerKG,
			float populationHalfLife,
			float overPopulationHalfLife,
			int minDiffusionCount,
			float diffusionScale,
			byte minDiffusionInfestationTickCount)
		{
			disease.AddGrowthRule(new GrowthRule()
			{
				underPopulationDeathRate = new float?(underPopulationDeathRate),
				minCountPerKG = new float?(minCountPerKG),
				maxCountPerKG = new float?(maxCountPerKG),
				populationHalfLife = new float?(populationHalfLife),
				overPopulationHalfLife = new float?(overPopulationHalfLife),
				minDiffusionCount = new int?(minDiffusionCount),
				diffusionScale = new float?(diffusionScale),
				minDiffusionInfestationTickCount = new byte?(minDiffusionInfestationTickCount),
			});

			return this;
		}

		public GrowthInfoBuilder DisinfectedBy(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
					minDiffusionCount = new int?(100000),
					diffusionScale = new float?(1f / 1000f),
				});
			}

			return this;
		}

		public GrowthInfoBuilder DiesIn(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_4),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX)
				});
			}

			return this;
		}

		public GrowthInfoBuilder InstantlyDiesIn(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_INSTANT),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_INSTANT)
				});
			}

			return this;
		}

		public GrowthInfoBuilder StagnatesIn(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.NONE),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.NONE)
				});
			}

			return this;
		}

		public GrowthInfoBuilder GrowsIn(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_4),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_4)
				});
			}

			return this;
		}

		public GrowthInfoBuilder GrowsFastIn(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_7),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_7)
				});
			}

			return this;
		}

		public GrowthInfoBuilder GrowsVeryFastIn(params SimHashes[] elements)
		{
			foreach (var element in elements)
			{
				disease.AddGrowthRule(new ElementGrowthRule(element)
				{
					populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
					overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
					underPopulationDeathRate = new float?(DISEASE.GROWTH_FACTOR.NONE)
				});
			}

			return this;
		}

		public GrowthInfoBuilder DisinfectedBy(Tag tag)
		{
			disease.AddGrowthRule(new TagGrowthRule(tag)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				minDiffusionCount = new int?(100000),
				diffusionScale = new float?(1f / 1000f),
			});

			return this;
		}

		public GrowthInfoBuilder DiesIn(Tag tag)
		{
			disease.AddGrowthRule(new TagGrowthRule(tag)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_4),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX)
			});

			return this;
		}

		public GrowthInfoBuilder Rule(GrowthRule rule)
		{
			disease.AddGrowthRule(rule);
			return this;
		}

		public GrowthInfoBuilder StagnatesIn(Tag tag)
		{
			disease.AddGrowthRule(new TagGrowthRule(tag)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.NONE),
				overPopulationHalfLife = new float?(float.PositiveInfinity)
			});

			return this;
		}

		public GrowthInfoBuilder GrowsIn(Tag tag)
		{
			disease.AddGrowthRule(new TagGrowthRule(tag)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_4),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_4)
			});

			return this;
		}

		public GrowthInfoBuilder GrowsFastIn(Tag tag)
		{
			disease.AddGrowthRule(new TagGrowthRule(tag)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_7),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_7)
			});

			return this;
		}

		public GrowthInfoBuilder GrowsVeryFastIn(Tag tag)
		{
			disease.AddGrowthRule(new TagGrowthRule(tag)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
				underPopulationDeathRate = new float?(DISEASE.GROWTH_FACTOR.NONE)
			});

			return this;
		}

		public GrowthInfoBuilder DisinfectedBy(Element.State state)
		{
			disease.AddGrowthRule(new StateGrowthRule(state)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				minDiffusionCount = new int?(100000),
				diffusionScale = new float?(1f / 1000f),
			});

			return this;
		}

		public GrowthInfoBuilder DiesIn(Element.State state)
		{
			disease.AddGrowthRule(new StateGrowthRule(state)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_4),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX)
			});

			return this;
		}

		public GrowthInfoBuilder DiesAndSlowsOnSolid()
		{
			disease.AddGrowthRule(new StateGrowthRule(Element.State.Solid)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_4),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.DEATH_MAX),
				diffusionScale = new float?(1E-06f),
				minDiffusionCount = new int?(1000000),
			});

			return this;
		}

		public GrowthInfoBuilder StagnatesIn(Element.State state)
		{
			disease.AddGrowthRule(new StateGrowthRule(state)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.NONE),
				overPopulationHalfLife = new float?(float.PositiveInfinity)
			});

			return this;
		}

		public GrowthInfoBuilder GrowsIn(Element.State state)
		{
			disease.AddGrowthRule(new StateGrowthRule(state)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_4),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_4)
			});

			return this;
		}

		public GrowthInfoBuilder GrowsFastIn(Element.State state)
		{
			disease.AddGrowthRule(new StateGrowthRule(state)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_7),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_7)
			});

			return this;
		}

		public GrowthInfoBuilder GrowsVeryFastIn(Element.State state)
		{
			disease.AddGrowthRule(new StateGrowthRule(state)
			{
				populationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
				overPopulationHalfLife = new float?(DISEASE.GROWTH_FACTOR.GROWTH_1),
				underPopulationDeathRate = new float?(DISEASE.GROWTH_FACTOR.NONE)
			});

			return this;
		}
	}
}
