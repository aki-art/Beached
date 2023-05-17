using Beached.Content.DefBuilders;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
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
            new GrowthInfoBuilder(this)
                .DefaultBehavior(
                    DISEASE.UNDERPOPULATION_DEATH_RATE.NONE,
                    1.4f,
                    6000f,
                    0.4f,
                    500,
                    3000,
                    1f / 2000f,
                    1)
                .DiesIn(Element.State.Liquid)
                .DiesAndSlowsOnSolid()
                .GrowsVeryFastIn(SimHashes.ContaminatedOxygen, SimHashes.CarbonDioxide)
                .DisinfectedBy(SimHashes.ChlorineGas, SimHashes.SourGas, SimHashes.BleachStone, Elements.sourBrine)
                .DiesIn(Elements.murkyBrine, SimHashes.Brine, SimHashes.SaltWater, Elements.saltyOxygen, Elements.zincOre, Elements.zinc)
                .InstantlyDiesIn(Elements.sulfurousWater);

            // applies to the atmosphere around an item
            new ExposureRuleBuilder(this)
                .DisinfectedBy(SimHashes.Chlorine, SimHashes.SourGas, Elements.sourBrine)
                .InstantlyDiesIn(Elements.sulfurousWater);
        }
    }
}
