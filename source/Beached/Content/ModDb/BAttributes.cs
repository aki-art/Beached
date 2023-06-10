using Klei.AI;
using TUNING;

namespace Beached.Content.ModDb
{
	public class BAttributes
	{
		public const string PRECISION_ID = "Beached_Precision";

		public static Attribute handSteadiness;

		public static class Critters
		{
			public static Attribute acidVulnerability;
		}

		// also used for Elements
		public static class Buildings
		{
			public static Attribute acidResistance;
		}

		[DbEntry]
		public static void RegisterBuildingAttributes(Database.BuildingAttributes __instance)
		{
			Buildings.acidResistance = __instance.Add(new Attribute(
				 "Beached_Building_AcidResistant",
				 true,
				 Attribute.Display.General,
				 false));
		}

		[DbEntry]
		public static void RegisterCritterAttributes(Database.CritterAttributes __instance)
		{
			Critters.acidVulnerability = __instance.Add(new Attribute(
				"Beached_AcidVulnerability",
				STRINGS.CREATURES.STATS.ACIDVULNERABILITY.NAME,
				"",
				STRINGS.CREATURES.STATS.ACIDVULNERABILITY.TOOLTIP,
				0f,
				Attribute.Display.General,
				false));

			Critters.acidVulnerability.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None));
		}

		[DbEntry]
		public static void RegisterDuplicantAttributes(Database.Attributes __instance)
		{
			handSteadiness = __instance.Add(new Attribute(
				PRECISION_ID,
				true,
				Attribute.Display.Skill,
				true,
				0f,
				null,
				null,
				ModAssets.Sprites.MOD_MINERALOGIST));

			handSteadiness.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));

			MiscUtil.AddToStaticReadonlyArray<string, DUPLICANTSTATS>("ALL_ATTRIBUTES", PRECISION_ID);
		}
	}
}
