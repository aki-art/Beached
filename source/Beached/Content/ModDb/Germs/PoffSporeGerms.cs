using Beached.Content.DefBuilders;
using Beached.Content.Defs.Foods;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using System.Linq;
using TUNING;

namespace Beached.Content.ModDb.Germs
{
    public class PoffSporeGerms : Disease
    {
        public const string ID = "Beached_PoffSpore";
        public const float RAD_KILL_RATE = 2.5f;

        public float UVHalfLife => 2f; // UV Lamps compat

        private static readonly RangeInfo temperatureRangeInfo = MiscUtil.RangeInfoCelsius(-10, 0, 45, 50);

        private static readonly RangeInfo temperatureHalfLivesInfo = new(
            DISEASE.GROWTH_FACTOR.DEATH_MAX,
            DISEASE.GROWTH_FACTOR.DEATH_4,
            DISEASE.GROWTH_FACTOR.DEATH_4,
            DISEASE.GROWTH_FACTOR.DEATH_MAX);

        private static readonly RangeInfo pressureRangeInfo = new(0f, 0f, 2000f, 2000f);

        private static readonly RangeInfo pressureHalfLivesInfo = new(
            DISEASE.GROWTH_FACTOR.DEATH_MAX,
            DISEASE.GROWTH_FACTOR.DEATH_4,
            DISEASE.GROWTH_FACTOR.DEATH_4,
            DISEASE.GROWTH_FACTOR.DEATH_MAX);

        public PoffSporeGerms(bool statsOnly) : base(
            ID,
            20,
            temperatureRangeInfo,
            temperatureHalfLivesInfo,
            pressureRangeInfo,
            pressureHalfLivesInfo,
            RAD_KILL_RATE,
            statsOnly)
        {
            overlayColourName = ID;
        }

        public override void PopulateElemGrowthInfo()
        {
            // applies to floating in air, and being stuck to items
            var growth = new GrowthInfoBuilder(this)
                .DefaultBehavior(
                    DISEASE.UNDERPOPULATION_DEATH_RATE.NONE,
                    1.4f,
                    60_000f,
                    0.4f,
                    500,
                    3000,
                    1f / 2000f,
                    1)
                .DiesIn(Element.State.Liquid)
                .DiesAndSlowsOnSolid();

#if ELEMENTS
            growth
                .GrowsIn(PoffConfig.configs.Select(c => c.elementID).ToArray())
                .DisinfectedBy(Elements.sourBrine)
                .DiesIn(Elements.murkyBrine, SimHashes.Brine, SimHashes.SaltWater)
                .InstantlyDiesIn(Elements.sulfurousWater);
#endif

            var exposure = new ExposureRuleBuilder(this)
                .DefaultHalfLife(float.PositiveInfinity);
#if ELEMENTS
            exposure
                .DisinfectedBy(Elements.sourBrine, Elements.murkyBrine, SimHashes.Brine, SimHashes.SaltWater)
                .InstantlyDiesIn(Elements.sulfurousWater);
#endif
        }
    }
}
