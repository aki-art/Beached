using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using TUNING;

namespace Beached.Content.DefBuilders
{
    // applies to the atmosphere around an item
    public class ExposureRuleBuilder
    {
        private Disease disease;

        public ExposureRuleBuilder(Disease disease)
        {
            this.disease = disease;
            disease.InitializeElemExposureArray(ref disease.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
        }

        public ExposureRuleBuilder DefaultHalfLife(float populationHalfLife)
        {
            disease.AddExposureRule(new ExposureRule()
            {
                populationHalfLife = new float?(populationHalfLife)
            });

            return this;
        }

        public ExposureRuleBuilder GrowsIn(params SimHashes[] elements)
        {
            foreach (var element in elements)
            {
                disease.AddExposureRule(new ElementExposureRule(element)
                {
                    populationHalfLife = DISEASE.GROWTH_FACTOR.GROWTH_4
                });
            }

            return this;
        }

        public ExposureRuleBuilder DisinfectedBy(params SimHashes[] elements)
        {
            foreach (var element in elements)
            {
                disease.AddExposureRule(new ElementExposureRule(element)
                {
                    populationHalfLife = DISEASE.GROWTH_FACTOR.DEATH_MAX
                });
            }

            return this;
        }

        public ExposureRuleBuilder InstantlyDiesIn(params SimHashes[] elements)
        {
            foreach (var element in elements)
            {
                disease.AddExposureRule(new ElementExposureRule(element)
                {
                    populationHalfLife = DISEASE.GROWTH_FACTOR.DEATH_INSTANT
                });
            }

            return this;
        }
    }
}
