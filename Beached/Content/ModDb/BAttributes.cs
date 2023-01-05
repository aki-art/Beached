using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.ModDb
{
    public class BAttributes
    {
        public static string PRECISION_ID = "Beached_Precision";

        public static Attribute handSteadiness;

        public static class Critters
        {
            public static Attribute acidVulnerability;
        }

        public static void Register(Database.Attributes parent)
        {
            RegisterDuplicantAttributes(parent);
            RegisterCritterAttributes(parent);

        }

        private static void RegisterCritterAttributes(Database.Attributes parent)
        {
            Critters.acidVulnerability = parent.Add(new Attribute(
                "Beached_AcidVulnerability",
                STRINGS.CREATURES.STATS.ACIDVULNERABILITY.NAME,
                "",
                STRINGS.CREATURES.STATS.ACIDVULNERABILITY.TOOLTIP,
                0f,
                Attribute.Display.General,
                false));

            Critters.acidVulnerability.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None));
        }

        private static void RegisterDuplicantAttributes(Database.Attributes parent)
        {
            handSteadiness = parent.Add(new Attribute(
                PRECISION_ID,
                true,
                Attribute.Display.Skill,
                true,
                0f,
                null,
                null,
                ModAssets.Sprites.MOD_MINERALOGIST));

            handSteadiness.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));

            var ref_ALL_ATTRIBUTES = AccessTools.FieldRefAccess<string[]>(typeof(DUPLICANTSTATS), "ALL_ATTRIBUTES");
            var existingValues = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES)
            {
                PRECISION_ID
            };

            ref_ALL_ATTRIBUTES() = existingValues.ToArray();
        }
    }
}
