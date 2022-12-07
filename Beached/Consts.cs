using UnityEngine;

namespace Beached
{
    public class CONSTS
    {
        public const float CYCLE_LENGTH = 600f;

        public static class DUPLICANTS
        {
            public static class LIFEGOALS
            {
                public const int MORALBONUS = 2;
            }
        }

        public static class EXPOSURE_EFFECT
        {
            public const float CLEANSING = -1f;
            public const float COMFORTABLE = -0.25f;
            public const float NEUTRAL = 0f;
            public const float MILDLY_IRRITATING = 0.5f;
            public const float VERY_IRRITATING = 1f;
            public const float OH_HECK_IT_BURNS = 3f;
        }

        public static class SNAPONS
        {
            public const string CAP = "Beached_SnapOn_Cap";
        }

        public class BATCH_TAGS
        {
            public const int SWAPS = -77805842;
            public const int INTERACTS = -1371425853;
        }

        public class COLORS
        {
            public static Color KLEI_PINK = new Color32(127, 61, 94, 255);
            public static Color KLEI_BLUE = new Color32(62, 67, 87, 255);
        }

        // TUNING is missing half of these
        public static class AUDIO_CATEGORY
        {
            public const string METAL = "Metal";
            public const string GLASS = "Glass";
            public const string HOLLOWMETAL = "HollowMetal";
            public const string PLASTIC = "Plastic";
            public const string SOLIDMETAL = "SolidMetal";
        }

        public static class NAV_GRID
        {
            public const string WALKER_BABY = "WalkerBabyNavGrid";
            public const string WALKER_1X1 = "WalkerNavGrid1x1";
            public const string WALKER_1X2 = "WalkerNavGrid1x2";
            public const string MINION = "MinionNavGrid";
            public const string ROBOT = "RobotNavGrid";
            public const string DIGGER = "DiggerNavGrid";
            public const string DRECKO = "DreckoNavGrid";
            public const string DRECKO_BABY = "DreckoBabyNavGrid";
            public const string FLYER_1X1 = "FlyerNavGrid1x1";
            public const string FLYER_1X2 = "FlyerNavGrid1x2";
            public const string FLYER_2X2 = "FlyerNavGrid2x2";
            public const string SLICKSTER = "FloaterNavGrid";
            public const string SWIMMER = "SwimmerNavGrid";
            public const string PIP = "SquirrelNavGrid";
        }

        public static class BUILD_CATEGORY
        {
            ///<summary>Base</summary>
            public const string BASE = "Base";

            ///<summary>Oxygen</summary>
            public const string OXYGEN = "Oxygen";

            ///<summary>Power</summary>
            public const string POWER = "Power";

            ///<summary>Food</summary>
            public const string FOOD = "Food";

            ///<summary>Plumbing</summary>
            public const string PLUMBING = "Plumbing";

            ///<summary>Ventilation</summary>
            public const string HVAC = "HVAC";

            ///<summary>Refinement</summary>
            public const string REFINING = "Refining";

            ///<summary>Medicine</summary>
            public const string MEDICAL = "Medical";

            ///<summary>Furniture</summary>
            public const string FURNITURE = "Furniture";

            ///<summary>Stations</summary>
            public const string EQUIPMENT = "Equipment";

            ///<summary>Utilities</summary>
            public const string UTILITIES = "Utilities";

            ///<summary>Automation</summary>
            public const string AUTOMATION = "Automation";

            ///<summary>Shipping</summary>
            public const string CONVEYANCE = "Conveyance";

            ///<summary>Rocketry</summary>
            public const string ROCKETRY = "Rocketry";

            ///<summary>Radiation</summary>
            public const string HEP = "HEP";
        }

    }
}
