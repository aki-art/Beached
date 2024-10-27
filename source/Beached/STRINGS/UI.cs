namespace Beached
{
	public partial class STRINGS
	{
		public static LocString BEACHED_MOD_NAME = "Beached";

		public class UI
		{
			public static class DIET
			{
				public static LocString EXTRA_PRODUCE = "    • Additional excretion: {tag}";
				public static LocString EXTRA_PRODUCE_TOOLTIP = "This critter will also \"produce\" {tag}, for {percent} of consumed mass.";
			}

			public static class MINEABLE
			{
				public static LocString TITLE = "Mineable";
				public static LocString TOOLTIP = "This critters outer can be mined and harvested, without causing any harm to the creature.";
			}

			public static class BEACHED_USERMENUACTIONS
			{
				public static class TAPPABLE
				{
					public static LocString REMOVE_TAP = "Remove Tap";
					public static LocString CANCEL_TAP = "Cancel Tapping";
					public static LocString TAP = "Tap";
					public static LocString TOOLTIP = "Collect rubber from this tree.";
				}
			}

			public class SANDBOX
			{
				public static LocString ALL = "All";
				public static LocString FLORA = "Flora";
				public static LocString FAUNA = "Fauna";
				public static LocString GEYSERS = "Geysers";
				public static LocString GEMS = "Gems";
				public static LocString EQUIPMENT = "Equipment";
				public static LocString SET_PIECES = "Set Pieces";
				public static LocString FOOD = "Food";
				public static LocString GENETIC_SAMPLES = "Genetic Samples";
				public static LocString AMBER_INCLUSIONS = "Amber Inclusions";
				public static LocString GLACIERS = "Glaciers";
			}

			public static LocString CHARACTERCONTAINER_LIFEGOAL_TRAIT = "<color=#e6d084>Life Goal: {0}</color>";

			public class BEACHED_MISC
			{
				public static LocString CAPPED = "Capped {0}";
				public static LocString BEACHED_CONTENT = "Content added by <color=d6926d><i>Beached</i></color>";
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
				public class BEACHED_GENETICOVERLAY
				{
					public static LocString NAME = "GENETICS OVERLAY";
					public static LocString HOVERTITLE = "GENETICS";
					public static LocString BUTTON = "Genetics Overlay";
					public static LocString TOOLTIP = "Displays the Genetic modifications and DNA capacity of critters on screen.";
				}
			}

			public class SPACEDESTINATIONS
			{
				public class CLUSTERMAPMETEORSHOWERS
				{
					public class BEACHED_DIAMOND
					{
						public static LocString NAME = "Shooting Stars";
						public static LocString DESCRIPTION = "Diamond and rare various gemstone meteors.";
					}

					public class BEACHED_ABYSSALITE
					{
						public static LocString NAME = "Abyssalite Meteors";
						public static LocString DESCRIPTION = "TODO";
					}

					public class BEACHED_AMBERGREIS
					{
						public static LocString NAME = "Ambergreis Rain";
						public static LocString DESCRIPTION = "Enormous droplets of Ambergreis rain down on the surface.";
					}
				}

				public class HARVESTABLE_POI
				{
					public class BEACHED_HARVESTABLESPACEPOI_PEARLESCENTASTEROIDFIELD
					{
						public static LocString NAME = "Pearlescent Asteroid Field";
						public static LocString DESC = "A clump of disarrayed sand, pearls and bismuth crystals.";
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
}
