using System.Collections.Generic;

namespace Beached.Content
{
    public class GeyserConfigs
    {
        public const string AMMONIA_VENT = "Beached_Ammonia";
        public const string MURKY_BRINE_GEYSER = "Beached_MurkyBrine";
        public const string BISMUTH_VOLCANO = "Beached_Bismuth";

        public static void GenerateConfigs(List<GeyserGenericConfig.GeyserPrefabParams> list)
        {
            list.Add(new GeyserGenericConfig.GeyserPrefabParams(
                "beached_murkywater_geyser_kanim",
                4,
                2,
                new GeyserConfigurator.GeyserType(
                    MURKY_BRINE_GEYSER,
                    Elements.MurkyBrine,
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
                    2900f,
                    200f,
                    400f,
                    150f,
                    480f,
                    1080f,
                    0.016666668f,
                    0.1f),
                true));

        }
    }
}
