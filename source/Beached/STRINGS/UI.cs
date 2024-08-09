using FUtility.FLocalization;

namespace Beached.STRINGS
{
	public class UI : StringsBase
	{
		public static LocString CHARACTERCONTAINER_LIFEGOAL_TRAIT = "<color=#e6d084>Life Goal: {0}</color>";

		public class BEACHED_MISC
		{
			public static LocString CAPPED = "Capped {0}";
		}

		public class BUILDCATEGORIES
		{
			public class BEACHED_POIS
			{
				public static LocString NAME = "Beached POIs";
				public static LocString TOOLTIP = "debug mode";
			}
		}

		public class OVERLAY
		{
			public class BEACHED_CONDUCTIONOVERLAY
			{
				public static LocString NAME = "ELECTRIC CONDUCTION OVERLAY";
				public static LocString HOVERTITLE = "ELECTRIC CONDUCTION";
				public static LocString BUTTON = "Electric Conduction Overlay";
				public static LocString TOOLTIP = "Displays the relative electrical conductivity of materials.";
			}
		}

		public class SPACEDESTINATIONS
		{
			public class CLUSTERMAPMETEORSHOWERS
			{
				public class BEACHED_DIAMOND
				{
					public static LocString NAME = "Shooting Stars";
					public static LocString DESCRIPTION = "TODO";
				}

				public class BEACHED_ABYSSALITE
				{
					public static LocString NAME = "Abyssalite Meteors";
					public static LocString DESCRIPTION = "TODO";
				}
			}

			public class HARVESTABLE_POI
			{
				public class BEACHED_HARVESTABLESPACEPOI_PEARLESCENTASTEROIDFIELD
				{
					public static LocString NAME = "Pearlescent Asteroid Field";
					public static LocString DESC = "TODO";
				}

				public class BEACHED_HARVESTABLESPACEPOI_AMMONITE
				{
					public static LocString NAME = "Ancient Ammonite Shell";
					public static LocString DESC = "A long perished remains of an abnormally large Ammonite. I hope nothing this " +
						"large exists alive elsewhere.";
				}
			}
		}

		public class CODEX
		{
			public static LocString GUIDES = "Beached Guides";

			public class CRITTER_HAPPINESS
			{
				public static LocString TITLE = "Critter Happiness";
				public static LocString CONTENT = "Critter Happiness";
			}

			public class SPORES
			{
				public static LocString TITLE = "So about those 'shrooms...";
				public static LocString CONTENT = "Mushrooms in this world seem to work a little differently. " +
					"Instead of a single, seed sized spore, mushrooms now emit a cloud of spores into the atmosphere. " +
					"The spores are too small to see by the naked eye, the Germ Overlay will reveal them instead. When the spores make contact " +
					"with soft ground, such as Sand or Dirt, a new fungus will eventually spawn there.\n" +
					"\n" +
					"If not managed, these mushrooms could easily take over my entire colony, I best be careful not to let that happen. " +
					"In any case, I should probably invest into medical equipment to deal with any new diseases, just in case.";

				public static LocString MUSHROOM_DEFENSE_TITLE = "Defenses";
				public static LocString MUSHROOM_DEFENSE = "Too stop the spread of unruly mushroom spores, I can light up any areas to protect it;" +
					"mushrooms cannot spawn in lit areas, and the light decimates any airborne spores as well. Covering the ground with built Tiles" +
					" is also an effective means to stop the spreading.";
			}
		}
	}

}
