using Beached.Content.DefBuilders;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using TUNING;

namespace Beached.Content.ModDb.Germs
{
    internal class PlanktonGerms : Disease
    {
        public const string ID = "Beached_Plankton";
        public const float RAD_KILL_RATE = 2.5f;

        public float UVHalfLife => 1f; // UV Lamps compat

        private static readonly RangeInfo temperatureRangeInfo = new RangeInfo(283.15f, 293.15f, 363.15f, 373.15f);
        private static readonly RangeInfo temperatureHalfLivesInfo = new RangeInfo(10f, 1200f, 1200f, 10f);
        private static readonly RangeInfo pressureRangeInfo = new RangeInfo(0f, 0f, 1000f, 1000f);

        public PlanktonGerms(bool statsOnly) : base(ID, 20, temperatureRangeInfo, temperatureHalfLivesInfo,
            pressureRangeInfo, RangeInfo.Idempotent(), RAD_KILL_RATE, statsOnly)
        {
            overlayColourName = ID;
        }

        public override void PopulateElemGrowthInfo()
        {
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
                .DiesIn(Element.State.Gas)
                .DiesAndSlowsOnSolid()
                .GrowsIn(GameTags.AnyWater)
                .DisinfectedBy(SimHashes.ChlorineGas, SimHashes.BleachStone, SimHashes.Ethanol)
                .DiesIn(Elements.sulfurousWater);

            InitializeElemExposureArray(ref elemExposureInfo, DEFAULT_EXPOSURE_INFO);
            new ElementExposureRule(SimHashes.ChlorineGas).populationHalfLife = new float?(10f);
            new ElementExposureRule(SimHashes.Ethanol).populationHalfLife = new float?(10f);
        }
    }
}
