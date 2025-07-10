namespace Beached
{
	public partial class STRINGS
	{
		public static LocString BEACHED_MOD_NAME = "Beached";

		public static class BEACHED
		{
			public static class UI
			{
				public static class SKELETON_POI_POPUP
				{
					public static LocString TITLE = "Gift from the past";
					public static LocString BODY = "My duplicants have rummaged through the remains of a long gone duplicant. The readings show that they have been here for several thousand cycles. It appears we were not the first to arrive here... \n" +
						"\n" +
						"New buildings have become available:\n  {0}";
				}

				public static class MUFFIN_SIDESCREEN
				{
					public static class CONTENTSTOP
					{
						public static class AGESLIDER
						{
							public static LocString LABEL = "Minimum Age:";
							public static class INPUTFIELD
							{
								public static LocString UNIT = "%";
							}
						}

						public static class IGNOREEGGSCHECKBOX
						{
							public static LocString LABEL = "Ignore Eggs";
							public static LocString TOOLTIP = "Eggs will not be cracked";
						}

						public static class IGNORENAMEDCHECKBOX
						{
							public static LocString LABEL = "Ignore Named";
							public static LocString TOOLTIP = "Critters with a unique name will not be targeted.";
						}

						public static class FILTERLESSCYCLE
						{
							public static LocString TITLE = "Filterless";
							public static LocString TOOLTIP = "Critters with a unique name will not be targeted.";
							public static LocString IGNORE = "Ignore";
							public static LocString IGNORE_DESC = "Critter without a filter will not be hunted.";
							public static LocString HUNT = "Always Hunt";
							public static LocString HUNT_DESC = "Critter without a filter will be hunted.";
							public static LocString COUNT = "Default Count";
							public static LocString COUNT_DESC = "Leave X critter alive of each species.";
						}

						public static class TITLEPREFAB
						{
							public static LocString LABEL = "Keep Alive";
						}

						public static class DEFAULT
						{
							public static LocString LABEL = "Default";
						}
					}

					public static class CONTENTS
					{
						public static class ADDBUTTON
						{
							public static class BUTTON
							{
								public static LocString LABEL = "+ Add Filter";
							}
						}
					}
				}
			}
		}

		public class UI
		{
			public static LocString BEACHED_ALPHA_MESSAGE = $"Crashes, bugs and MISSING. ahead!\n" +
				$"\n" +
				$"Beached is currently in Alpha Play Test, and there are a lot ";

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
				public static class READLORE
				{
					public static LocString SEARCH_WRECKEDHABITAT = "Most of the resources have been destroyed in the crash, but my Duplicants were able to recover some Oxylite from the remnants of the oxidizer tank.";
				}

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
				public static LocString POI_UNLOCK_TITLE = "Gift from the past";
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

				public class BEACHED_DIETS
				{
					public static LocString TITLE = "Diets";
					public static LocString CONTENT = "";
					public static LocString VEGETARIAN_TITLE = "Vegetarian";
					public static LocString VEGETARIAN_CONTENT = "Vegetarian duplicants are particularly picky about their food choices, even when stranded on an Asteroid with limited food options. These duplicants will consume meat but only if there are no other options available. Should this happen, they will have a 30% stress/cycle increase.\n" +
						"\n" +
						"<b>Eggs and Brackene</b>\n" +
						"Luckily the duplicants consider these items vegetarian.\n" +
						"\n" +
						"<b>New food items</b>\n" +
						"To accomodate my duplicants, several new recipes were revised, and are available as an alternative:";
					public static LocString CARNIVORE_TITLE = "Carnivore";
					public static LocString CARNIVORE_CONTENT = "";
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
}
