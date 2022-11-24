using Newtonsoft.Json;

namespace Beached.Settings
{
    public class CrossWorld
    {
        public ElementSettings Elements { get; set; } = new ElementSettings();

        public class ElementSettings
        {
            public bool LimeToCalcium { get; set; } = false;

            public bool CrystalCategory { get; set; } = false;

            public bool ElementInteractions { get; set; } = false;

            // storing original data because users can edit yamls, and i dont want to override that
            [JsonIgnore]
            public Tag originalDiamondCategory;

            [JsonIgnore]
            public Tag originalAbyssaliteCategory;

            [JsonIgnore]
            public float originalLimeHighTemp;

            [JsonIgnore]
            public Tag originalLimeHighTempTarget;
        }
    }
}
