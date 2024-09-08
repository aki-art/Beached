using Beached.Content;
using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Entities.Critters;
using Beached.Content.Defs.Flora;
using FUtility.FLocalization;

namespace Beached
{
	public partial class STRINGS
	{
		public class CREATURES
		{
			public class STATUSITEMS
			{
				public class BEACHED_DESICCATION
				{
					public static LocString NAME = "Desiccating";
					public static LocString TOOLTIP = "This critter is unable to moisturize itself and is drying up.\n" +
						"Submerge in any liquid to rejuvenate.\n" +
						"\n" +
						"Death in {0}s";
				}

				public class BEACHED_CONTROLLERBYCOLLARDISPENSER
				{
					public static LocString NAME = "Instructed";
					public static LocString TOOLTIP = "This hunter is selective about it's prey, as instructed. \n\n" +
						"Allowed prey: {PermittedCritters}\n" +
						"\n" +
						"{PerCritter}.";

					public static LocString PER_CRITTER = "It will leave {MinCount} of each premitted critter alive.";
					public static LocString GLOBAL = "It will leave {MinCount} total critters alive in it's encluse, regardless of species.";
				}

				public class BEACHED_CULTIVATINGGERMS
				{
					public static LocString NAME = "{Germ} Habitat";
					public static LocString TOOLTIP = "This object is providing an ideal environment to {Germ}, boosting their reproduction rate.";
				}

				public class BEACHED_GENETICALLYMODIFIED
				{
					public static LocString NAME = "Genetically Altered";
					public static LocString TOOLTIP = "This egg has been genetically modified: \n" +
						"{0}\n\n" +
						"It cannot receive any more modifications.";
				}

				public class BEACHED_HUNTING
				{
					public static LocString NAME = "Hunting";
					public static LocString TOOLTIP = "This critter is looking for prey.";
				}

				public class BEACHED_SMOKING
				{
					public static LocString NAME = "Smoking ({0})";
					public static LocString TOOLTIP = "This item is getting smoked in this atmosphere. Change: {0}.";
				}
			}

			public class MODIFIERS
			{
				public class MOISTURE_LOSS_RATE
				{
					public static LocString NAME = "Not in liquid";
				}

				public class MOVEMENT_MOISTURE_LOSS
				{
					public static LocString NAME = "Moving";
				}

				public class LIMPET_GROWTH_RATE
				{
					public static LocString NAME = "Growing Limpets";
				}
			}

			public class TRAITS
			{
				public class GEYSERS
				{
					public class BEACHED_LARGE
					{
						public static LocString NAME = "Large";
						public static LocString PREFIX = "Large";
						public static LocString DESC = "This {GeyserType} is unusually large, with increased output.";
					}

					public class BEACHED_SMALL
					{
						public static LocString NAME = "Small";
						public static LocString PREFIX = "Small";
						public static LocString DESC = "This {GeyserType} is unusually small, with decreased output.";
					}
				}

				public class BEACHED_GMOTRAITS_BLAND
				{
					public static LocString NAME = "Bland";
					public static LocString DESC = "This critter is a bit easier on the eyes, it provides no decor.";
				}

				public class BEACHED_GMOTRAITS_PRODUCTIVE1
				{
					public static LocString NAME = "Kinda Productive";
					public static LocString DESC = "This critter has doubled metabolism and production.";
				}

				public class BEACHED_GMOTRAITS_PRODUCTIVE2
				{
					public static LocString NAME = "Moderately Productive";
					public static LocString DESC = "This critter has quadropled metabolism and production.";
				}

				public class BEACHED_GMOTRAITS_PRODUCTIVE3
				{
					public static LocString NAME = "Super Productive";
					public static LocString DESC = "This critter has octupled metabolism and production.";
				}

				public class BEACHED_GMOTRAITS_MEATY
				{
					public static LocString NAME = "Meaty";
					public static LocString DESC = "This critter looks exceptionally tasty.";
				}

				public class BEACHED_GMOTRAITS_FABULOUS
				{
					public static LocString NAME = "Fabulous";
					public static LocString DESC = "My duplicants find this critter the most beautiful creature they have ever laid eyes upon.";
				}

				public class BEACHED_GMOTRAITS_LASTING
				{
					public static LocString NAME = "Lasting";
					public static LocString DESC = "This critter will far outlive it's friends.";
				}

				public class BEACHED_GMOTRAITS_HYPOALLERGENIC
				{
					public static LocString NAME = "Hypoallergenic";
					public static LocString DESC = "Even the worst of fur allergics can safely pet this critter.";
				}

				public class BEACHED_GMOTRAITS_EVERLASTING
				{
					public static LocString NAME = "Everlasting";
					public static LocString DESC = "This critter does not seem to age.";
				}

				public class BEACHED_GMOTRAITS_FLUFFY
				{
					public static LocString NAME = "Fluffy";
					public static LocString DESC = "This critter can withstand much colder environments than normal.";
				}

				public class BEACHED_GMOTRAITS_BREEZY
				{
					public static LocString NAME = "Breezy";
					public static LocString DESC = "This critter can withstand much hotter environments than normal.";
				}
			}

			public class STATS
			{
				public class ACIDVULNERABILITY
				{
					public static LocString NAME = "Acid Vulnerability";
					public static LocString TOOLTIP = "";
				}

				public class MOISTURE
				{
					public static LocString NAME = "Moisture";
					public static LocString TOOLTIP = "When a Critter reaches 0% Moisture, they will desiccate and die.\n" +
						"Moisture can be replenished by submerging the critter in any liquid.";
				}

				public class LIMPETGROWTH
				{
					public static LocString NAME = "Limpets";
					public static LocString TOOLTIP = "This critter is growing Limpets on it's back.";
				}
			}

			public class SPECIES
			{
				// no clue why these are counted as "creature", but this is where the game expects them
				public class GEYSER
				{
					public class BEACHED_MURKYBRINE
					{
						public static LocString NAME = "Murky Brine Geyser";
						public static LocString DESC = $"A geyser that periodically erupts with {FormatAsLink("Murky Brine", Elements.murkyBrine.ToString())}.";
					}

					public class BEACHED_BISMUTHVOLCANO
					{
						public static LocString NAME = "Bismuth Volcano";
						public static LocString DESC = $"...";
					}

					public class BEACHED_AMMONIA
					{
						public static LocString NAME = "Ammonia Vent";
						public static LocString DESC = $"...";
					}

					public class BEACHED_CORAL_REEF
					{
						public static LocString NAME = "Coral Reef";
						public static LocString DESC = $"A geyser that periodically erupts with" +
							$" {FormatAsLink("Salt Water", SimHashes.SaltWater.ToString().ToUpperInvariant())} rich in " +
							$"{FormatAsLink("Plankton", "PlanktonGerms"/*PlanktonGerms.ID.ToUpperInvariant()*/)}.";
					}

					public class BEACHED_BISMUTH_VOLCANO
					{
						public static LocString NAME = "Bismuth Volcano";
						public static LocString DESC = $"A large volcano that periodically erupts with {STRINGS.ELEMENTS.BISMUTHMOLTEN.NAME}";
					}

					public class BEACHED_PACU_GEYSER
					{
						public static LocString NAME = "Pacu Geyser";
						public static LocString DESC = $"A geyser that periodically erupts with" +
							$" {FormatAsLink("Polluted Water", SimHashes.DirtyWater.ToString())} rich in " +
							$"{FormatAsLink("Pacus", PacuConfig.ID)}.";
					}
				}

				public class OTHERS
				{
					public class BEACHED_BRINE_POOL
					{
						public static LocString NAME = "Brine Pool";
						public static LocString DESC = "Releases Salt into the liquids or air it is submerged in.";
						public static LocString SALTOFF = "Converting {0} to {1}";
						public static LocString SALTOFF_TOOLTIP = "Saturates elements it is submerged in with salt at an average interval of {0}.";
					}
				}

				public class SEEDS
				{
					public class BEACHED_CORAL_WASHUSPONGE
					{
						public static LocString NAME = "Washu Sponge Frag";
						public static LocString DESC = "...";
					}

					public class BEACHED_CORAL_SALTYSTICK
					{
						public static LocString NAME = "Salty Sticks Frag";
						public static LocString DESC = "...";
					}

					public class WATERCUPS
					{
						public static LocString NAME = "Watercups Seed";
						public static LocString DESC = "...";
					}

					public class FILAMENT
					{
						public static LocString NAME = "Filament Seed";
						public static LocString DESC = "...";
					}

					public class BEACHED_CELLALGAE
					{
						public static LocString NAME = FormatAsLink("Small Cell", Content.Defs.Flora.AlgaeCellConfig.ID);
						public static LocString DESC = "...";
					}

					public class BEACHED_PIPTAIL
					{
						public static LocString NAME = FormatAsLink("PipTail", PipTailConfig.ID);
						public static LocString DESC = ($"The {FormatAsLink("Seed", "PLANTS")} of a {NAME}.");
					}

					public class BEACHED_LEAFLETCORAL
					{
						public static LocString NAME = "Leaflet Coral Frag";
						public static LocString DESC = ($"The {FormatAsLink("Frag", "CORALS")} of a {NAME}.");
					}
				}

				public class BEACHED_LEAFLETCORAL
				{
					public static LocString NAME = "Leaflet Coral";
					public static LocString DESC = "A coral with green capitulum resembling cabbage leaves. This coral is lined with a polymetallic outer epidermis, rich in Manganese and Iron, allowing natural slow electrolysis of Water. The plant consumes Hydrogen and releases excess Oxygen in the process.\n" +
						"\n" +
						"Inefficiently converts Water into Oxygen.";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_JELLYFISHSTROBILA
				{
					public static LocString NAME = "Jellyfish Strobila";
					public static LocString DESCRIPTION = "A bunch of neatly stacked Jellyfish babies waiting to be released and drift away.";
				}

				public class BEACHED_CORAL_SALTYSTICK
				{
					public static LocString NAME = FormatAsLink("Salty Sticks", SaltyStickConfig.ID);
					public static LocString DESC = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_CELLALGAE
				{
					public static LocString NAME = FormatAsLink("Bubble Algae", Content.Defs.Flora.AlgaeCellConfig.ID);
					public static LocString DESCRIPTION = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_PIPTAIL
				{
					public static LocString NAME = FormatAsLink("Piptail", PipTailConfig.ID);
					public static LocString DESCRIPTION = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_SLICKSHELL
				{
					public static LocString NAME = FormatAsLink("Slickshell", SlickShellConfig.ID);
					public static LocString DESC = "...";
					public static LocString BABY_NAME = FormatAsLink("Slickshelly", SlickShellConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = FormatAsLink("Slickshell Egg", SlickShellConfig.ID);
				}

				public class BEACHED_JELLYFISH
				{
					public static LocString NAME = FormatAsLink("Jellyfish", JellyfishConfig.ID);
					public static LocString DESC = "...";
					public static LocString BABY_NAME = FormatAsLink("Jelly Ephyra", JellyfishConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = FormatAsLink("Jellyfish Egg", JellyfishConfig.ID);
				}

				public class BEACHED_ANGULARFISH
				{
					public static LocString NAME = "Angular Fish";
					public static LocString DESC = "...";
					public static LocString BABY_NAME = "Angular Larvae";
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = "Angular Fish Egg";
				}

				public class BEACHED_MAKI
				{
					public static LocString NAME = "Maki";
					public static LocString DESC = "...";
					public static LocString BABY_NAME = "Maki Kit";
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = "Maki Egg";
				}

				public class BEACHED_MUFFIN
				{
					public static LocString NAME = FormatAsLink("Muffin", MuffinConfig.ID);
					public static LocString DESC = "...";
					public static LocString BABY_NAME = FormatAsLink("Muffling Whelp", MuffinConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = FormatAsLink("Muffling Whelp Egg", MuffinConfig.ID);
				}

				public class BEACHED_DEWPALM
				{
					public static LocString NAME = "Dew Palm";
					public static LocString DESC = "...";
				}

				public class BEACHED_MANGROVE
				{
					public static LocString NAME = "Gutwood";
					public static LocString DESC = "...";
				}

				public class BEACHED_BULBLOOM
				{
					public static LocString NAME = "Bulbloom";
					public static LocString DESC = "...";
				}

				public class BEACHED_BAMBOO
				{
					public static LocString NAME = "Bamboo"; //Clickety Clack? Clack Cane?
					public static LocString DESC = "...";
				}

				public class BEACHED_KELP
				{
					public static LocString NAME = FormatAsLink("Sea Weed", KelpConfig.ID);
					public static LocString DESC = "...";
				}


				public class BEACHED_WATERCUPS
				{
					public static LocString NAME = FormatAsLink("Watercups", WaterCupsConfig.ID);
					public static LocString DESC = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_FILAMENT
				{
					public static LocString NAME = "Feel-Lament";
					public static LocString DESC = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_MUSSEL_SPROUT
				{
					[Note("From Mussel (the mollusc) and Brussel Sprout.")]
					public static LocString NAME = "Mussel Sprout";
					public static LocString DESC = "The mussel sprout resembles a mollusc, with it's hard shell and slimy inside. When harvested it attempts to \"run\" away at speeds so slow they are difficult to observe with the naked eye, with the help of its prehensile tongue. ";
				}

				public class BEACHED_POFFSHROOM
				{
					public static LocString NAME = "Poff Ball";
					public static LocString DESC = "...";
				}

				public class BEACHED_GLOWCAP
				{
					public static LocString NAME = "Gloom Shroom";
					public static LocString DESC = "...";
				}

				public class BEACHED_BLADDERTREE
				{
					public static LocString NAME = "Gutwood";
					public static LocString DESC = "...";
				}

				public class BEACHED_DEEPGRASS
				{
					public static LocString NAME = "Deepgrass";
					public static LocString DESC = "...";
				}

				public class BEACHED_PURPLEHANGER
				{
					public static LocString NAME = "Purpicle";
					public static LocString DESC = "...";
				}
			}

			public class FAMILY
			{
				[Note("A slickster in a shell, also \"slick\" as in oily/slimy. It looks like a snail.")]
				public static LocString BEACHEDSLICKSHELL = FormatAsLink("Slickshell", "BEACHEDSNAILSPECIES");
				[Note("Angler fish with angular patterns.")]
				public static LocString BEACHEDANGULARFISH = "Angular Fish";
				[Note("A pip-cat-lemur thing. \"Maki\" means little monkey in an endearing and cute way.")]
				public static LocString BEACHEDMAKI = "Maki";
				public static LocString BEACHEDJELLYFISH = "Jellyfish";
			}

			public class FAMILY_PLURAL
			{
				[Note("A slickster in a shell, also \"slick\" as in oily/slimy. It looks like a snail.")]
				public static LocString BEACHEDSNAILSPECIES = FormatAsLink("Slickshells", "BEACHEDSNAILSPECIES");
				[Note("Angler fish with angular patterns.")]
				public static LocString BEACHEDANGULARFISHSPECIES = "Angular Fish";
				[Note("A pip-cat-lemur thing. \"Maki\" means little monkey in an endearing and cute way.")]
				public static LocString BEACHEDMAKISPECIES = "Makis";
				public static LocString BEACHEDJELLYFISHSPECIS = "Jellyfish";
			}
		}
	}
}
