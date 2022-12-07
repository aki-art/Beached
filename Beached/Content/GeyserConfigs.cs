using Beached.Content.ModDb.Germs;
using Klei;
using System.Collections.Generic;

namespace Beached.Content
{
    public class GeyserConfigs
    {
        public const string AMMONIA_VENT = "Beached_Ammonia";
        public const string MURKY_BRINE_GEYSER = "Beached_MurkyBrine";
        public const string BISMUTH_VOLCANO = "Beached_Bismuth";
        public const string CORAL_REEF = "Beached_CoralReef";

        public static void GenerateConfigs(List<GeyserGenericConfig.GeyserPrefabParams> list)
        {
            list.Add(new GeyserGenericConfig.GeyserPrefabParams(
                "beached_murkywater_geyser_kanim",
                4,
                2,
                new GeyserConfigurator.GeyserType(
                    MURKY_BRINE_GEYSER,
                    Elements.MurkyBrine,
                    GeyserConfigurator.GeyserShape.Liquid,
                    263.15f,
                    1000f,
                    2000f,
                    500f,
                    geyserTemperature: 263f),
                true));


            list.Add(new GeyserGenericConfig.GeyserPrefabParams(
                "beached_ammonia_vent_kanim",
                2,
                4,
                new GeyserConfigurator.GeyserType(
                    AMMONIA_VENT,
                    Elements.Ammonia,
                    GeyserConfigurator.GeyserShape.Gas,
                    333.15f,
                    70f,
                    140f,
                    5f,
                    60f),
                true));

            list.Add(new GeyserGenericConfig.GeyserPrefabParams(
                "beached_bismuth_volcano_kanim",
                3,
                3,
                new GeyserConfigurator.GeyserType(
                    BISMUTH_VOLCANO,
                    Elements.BismuthMolten,
                    GeyserConfigurator.GeyserShape.Molten,
                    2900f,
                    200f,
                    400f,
                    150f,
                    480f,
                    1080f,
                    0.016666668f,
                    0.1f),
                true));

            list.Add(new GeyserGenericConfig.GeyserPrefabParams(
                "geyser_liquid_water_filthy_kanim",
                4,
                2,
                new GeyserConfigurator.GeyserType(
                    CORAL_REEF,
                    SimHashes.SaltWater,
                    GeyserConfigurator.GeyserShape.Liquid,
                    GameUtil.GetTemperatureConvertedToKelvin(40, GameUtil.TemperatureUnit.Celsius),
                    2000f,
                    4000f,
                    4000f)
                .AddDisease(new SimUtil.DiseaseInfo
                {
                    idx = Db.Get().Diseases.GetIndex(BDiseases.plankton.id),
                    count = 20000
                }), true));

        }
    }
}
