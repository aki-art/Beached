using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.ModDb
{
    public class BAttributes
    {
        /// <summary>
        /// <see cref=""/>
        /// </summary>
        public static Attribute handSteadiness;
        public static string PRECISION_ID = "Beached_Precision";

        public static void Register(Database.Attributes parent)
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
