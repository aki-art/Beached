using Beached.Content;
using Beached.Content.Defs.Entities;
using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Entities.Critters.Dreckos;
using Beached.Content.Defs.Entities.Critters.Jellies;
using Beached.Content.Defs.Entities.Critters.Karacoos;
using Beached.Content.Defs.Entities.Critters.Mites;
using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Defs.Entities.Critters.Pacus;
using Beached.Content.Defs.Entities.Critters.Pufts;
using Beached.Content.Defs.Entities.Critters.Rotmongers;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.ModDb.Germs;
using FUtility.FLocalization;

namespace Beached
{
	public partial class STRINGS
	{
		public class CREATURES
		{
			public class ATTRIBUTES
			{
				public class BEACHED_LIMPETGROWTHDELTA
				{
					public static LocString NAME = "Limpets";
					public static LocString DESC = "This critter is growing Limpets on it's back.";
				}
			}

			public class STATUSITEMS
			{
				public class BEACHED_PULSING
				{
					public static LocString NAME = "Pulsing";
					public static LocString TOOLTIP = "This critter is emitting a powerful Pulse, alerting nearby creatures.";
				}

				public class BEACHED_STRANDED
				{
					public static LocString NAME = "Stranded";
					public static LocString TOOLTIP = "This critter is stranded from entering it's nest.";
				}

				public class BEACHED_COVERING
				{
					public static LocString NAME = "Covering";
					public static LocString TOOLTIP = "This critter is hiding under it's shell for protection.";
				}

				public class BEACHED_EGGSITTING
				{
					public static LocString NAME = "Egg-sitting";
					public static LocString TOOLTIP = "This Karakoo is currently coddling an Egg.";
				}

				public class BEACHED_BUILDINGNEST
				{
					public static LocString NAME = "Building Nest";
					public static LocString TOOLTIP = "This critter is constructing a home for itself.";
				}

				public class BEACHED_MIRROR
				{
					public static LocString NAME = "Fooled";
					public static LocString TOOLTIP = "This critter has been tricked by a mirror into thinking they have access to more space.";
				}

				public class BEACHED_SINGING
				{
					public static LocString NAME = "Resonating";
					public static LocString TOOLTIP = "This critter is communicating with it's peers by singing in ultra-sonic frequencies. During this state the critter is also producing large amounts of electricity.";
				}

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
					public static LocString TOOLTIP = "This predator is currently pursuing prey.";
				}

				public class BEACHED_SMOKING
				{
					public static LocString NAME = "Smoking ({0})";
					public static LocString TOOLTIP = "This item is getting smoked in this atmosphere. Change: {0}.";
				}

				public class BEACHED_LIMPETEDCRITTER
				{
					public static LocString NAME = "Limpets";
					public static LocString TOOLTIP = "This critter is growing Limpets on it's back.";
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

				public class BEACHED_LIMPET_GROWTH_RATE
				{
					public static LocString NAME = "Growing Limpets";
				}

				public class BEACHED_SHELLGROWTH
				{
					public static LocString NAME = "Shell Growth";

					public static LocString TOOLTIP = "...";
				}

				public class BEACHED_LIMPETGROWTH
				{
					public static LocString NAME = "Limpet Growth";

					public static LocString TOOLTIP = "...";
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
				public class BEACHED_SHELLGROWTH
				{
					public static LocString NAME = "Shell Growth";

					public static LocString TOOLTIP = "The amount of time required for this critter to regrow its shell";
				}

				public class BEACHED_ACIDVULNERABILITY
				{
					public static LocString NAME = "Acid Vulnerability";
					public static LocString TOOLTIP = "The more vulnerable a Critter is to Acid, the more damage they take while submerged in it.";
				}

				public class BEACHED_MOISTURE
				{
					public static LocString NAME = "Moisture";
					public static LocString TOOLTIP = "When a Critter reaches 0% Moisture, they will desiccate and die.\n" +
						"Moisture can be replenished by submerging the critter in any liquid.";
				}

				public class BEACHED_LIMPETGROWTH
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
						public static LocString DESC = $"A geyser that periodically erupts with {Link("Murky Brine", Elements.murkyBrine.ToString())}.";
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
							$" {Link("Salt Water", SimHashes.SaltWater.ToString())} rich in " +
							$"{Link("Plankton", PlanktonGerms.ID)}.";
					}

					public class BEACHED_BISMUTH_VOLCANO
					{
						public static LocString NAME = "Bismuth Volcano";
						public static LocString DESC = $"A large volcano that periodically erupts with {Link("Molten Bismuth", "BEACHEDBISMUTHMOLTEN")}";
					}

					public class BEACHED_SALT
					{
						public static LocString NAME = "Molten Salt Volcano";
						public static LocString DESC = $"A large volcano that periodically erupts with {Link("Molten Salt", "SALT")}";
					}

					public class BEACHED_HELIUM
					{
						public static LocString NAME = "Hot Helium Vent";
						public static LocString DESC = $"This vent periodically releases scorcing hot {Link("Helium", "HELIUM")}";
					}

					public class BEACHED_CORALREEF
					{
						public static LocString NAME = "Coral Reef";
						public static LocString DESC = $"This geyser peridocially emits a small amount of {Link("Salt Water", SimHashes.SaltWater.ToString())} rich in {Link("Plankton", PlanktonGerms.ID)}.";
					}

					public class BEACHED_PACU_GEYSER
					{
						public static LocString NAME = "Pacu Geyser";
						public static LocString DESC = $"A geyser that periodically erupts with" +
							$" {Link("Polluted Water", SimHashes.DirtyWater.ToString())} rich in " +
							$"{Link("Pacus", PacuConfig.ID)}.";
					}
				}

				public class OTHERS
				{
					public class BEACHED_BRINE_POOL
					{
						public static LocString NAME = Link("Brine Pool", BrinePoolConfig.ID);
						public static LocString DESC = "Releases Salt into the liquids or air it is submerged in.";
						public static LocString SALTOFF = "Converting {0} to {1}";
						public static LocString SALTOFF_TOOLTIP = "Saturates elements it is submerged in with salt at an average interval of {0}.";
					}
				}

				public class SEEDS
				{
					public class BEACHED_BONEWORM
					{
						public static LocString NAME = Link("Boneworm Bud", BonewormConfig.ID);
						public static LocString DESC = $"Egg of a {Link("Boneworm", BonewormConfig.ID)}. It can be planted in a farm plot.";
					}

					public class BEACHED_DEWNUT
					{
						public static LocString NAME = "Dewnut";
						public static LocString DESC = ".";
					}
					public class BEACHED_SIDEWAYSPLANT
					{
						public static LocString NAME = "Ath-Wart Seed";
						public static LocString DESC = "TODO.";
					}

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

					public class BEACHED_KELP
					{
						public static LocString NAME = "Kelp Seed";
						public static LocString DESC = "...";
					}

					public class BEACHED_BAMBOO
					{
						public static LocString NAME = "Bamboo Shoot";
						public static LocString DESC = "...";
					}

					public class WATERCUPS
					{
						public static LocString NAME = Link("Watercups Seed", WaterCupsConfig.ID);

						public static LocString DESC = $"The {Link("Seed", "PLANTS")} of a {BEACHED_WATERCUPS.NAME}." +
							$"\n" +
							$"\nDigging up Buried Objects may uncover a Watercups Seed.";
					}

					public class BEACHED_FILAMENT
					{
						public static LocString NAME = Link("Filament Seed", FilamentConfig.ID);
						public static LocString DESC = "...";
					}

					public class BEACHED_CELLALGAE
					{
						public static LocString NAME = Link("Bubble Cell", AlgaeCellConfig.ID);
						public static LocString DESC = $"A small osmosed piece of a {Link("Bubble Algae", AlgaeCellConfig.ID)}";
					}

					public class BEACHED_PIPTAIL
					{
						public static LocString NAME = Link("PipTail", PipTailConfig.ID);
						public static LocString DESC = ($"The {Link("Seed", "PLANTS")} of a {NAME}.");
					}

					public class BEACHED_LEAFLETCORAL
					{
						public static LocString NAME = Link("Leaflet Coral Frag", LeafletCoralConfig.ID);
						public static LocString DESC = ($"The {Link("Frag", "CORALS")} of a {Link("Leaflet Coral", LeafletCoralConfig.ID)}.");
					}

					public class BEACHED_FIRECORAL
					{
						public static LocString NAME = "Fire Coral Frag";
						public static LocString DESC = ($"The {Link("Frag", "CORALS")} of a {Link("Fire Coral", FireCoralConfig.ID)}.");
					}
				}

				public class BEACHED_FIRECORAL
				{
					public static LocString NAME = Link("Fire Coral", FireCoralConfig.ID);
					public static LocString DESC = "TODO";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_LEAFLETCORAL
				{
					public static LocString NAME = Link("Leaflet Coral", LeafletCoralConfig.ID);
					public static LocString DESC = "A coral with green capitulum resembling cabbage leaves. This coral is lined with a polymetallic outer epidermis, rich in Manganese and Iron, allowing natural slow electrolysis of Water. The plant consumes Hydrogen and releases excess Oxygen in the process.\n" +
						"\n" +
						"Inefficiently converts Water into Oxygen.";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_JELLYFISHSTROBILA
				{
					public static LocString NAME = Link("Jellyfish Strobila", JellyfishStrobilaConfig.ID);
					public static LocString DESCRIPTION = "A bunch of neatly stacked Jellyfish babies waiting to be released and drift away.";
				}

				public class BEACHED_CELLALGAE
				{
					public static LocString NAME = Link("Bubble Algae", AlgaeCellConfig.ID);
					public static LocString DESCRIPTION = $"A transparent squisjy singular cell, which can be harvested for edible {Link("Jelly", JellyConfig.ID)}.";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_PIPTAIL
				{
					public static LocString NAME = Link("Piptail", PipTailConfig.ID);
					public static LocString DESCRIPTION = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_SLICKSHELL
				{
					public static LocString NAME = Link("Slickshell", SlickShellConfig.ID);
					public static LocString DESC = $"Slickshell are a slow, docile critters that excreted {Link("Dirt", SimHashes.Dirt)} when consuming {Link("Salt", SimHashes.Salt)}. Slickshells also produce {Link("Mucus", Elements.mucus)} to aid their movement.";
					public static LocString BABY_NAME = Link("Slickshelly", SlickShellConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Slickshell Egg", SlickShellConfig.ID);
				}

				public class BEACHED_IRONSHELL
				{
					public static LocString NAME = Link("Ironshell", IronShellConfig.ID);
					public static LocString DESC = $"Ironshell are a slow, docile critters that excreted {Link("Obsidian", SimHashes.Obsidian)} when consuming {Link("Sulfur", SimHashes.Sulfur)}, and grow {Link("Pyrite", SimHashes.FoolsGold)} scales which can be shorn. Ironshells also produce {Link("Mucus", Elements.mucus)} to aid their movement.";
					public static LocString BABY_NAME = Link("Ironshelly", IronShellConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Ironshell Egg", IronShellConfig.ID);
				}

				public class BEACHED_AMMONIAPUFT
				{
					public static LocString NAME = Link("Fusty Puft", AmmoniaPuftConfig.ID);
					public static LocString DESC = $"Fusty Pufts are non-aggressive critters who that excrete {Link("Rot", Elements.rot)} with each breath.";
					public static LocString BABY_NAME = Link("Fusty Puftlet", AmmoniaPuftConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Fusty Puftlet Egg", AmmoniaPuftConfig.ID);
				}

				public class BEACHED_MERPIP
				{
					public static LocString NAME = Link("Mer-Pip", MerpipConfig.ID);
					public static LocString DESC = "Mer-Pips are an aquatic cousin to the common Pip. They can also plant coral frags to sea floors.";
					public static LocString BABY_NAME = Link("Mer-Pipsqueak", MerpipConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Mer-Pip Egg", MerpipConfig.ID);
				}

				public class BEACHED_SLAGMITE
				{
					[Note("Stalagmite + Slag + Mite")]
					public static LocString NAME = Link("Slagmite", SlagmiteConfig.ID);
					public static LocString DESC = $"Slagmites eat metal rich waste procucts.\n\nTheir shells can be mined while the critter is alive, providing various metals. This does not hurt the critter, they even appeciate the lesser weight on their backs.";
					public static LocString BABY_NAME = Link("Slagmitty", SlickShellConfig.ID);
					public static LocString BABY_DESC = $"A cute little Slagmitty.\n\nIn time it will mature into a fully grown Slagmite.";
					public static LocString EGG_NAME = Link("Slagmite Egg", SlickShellConfig.ID);
					public static LocString ODDS = "Odds:";
				}

				public class BEACHED_GLEAMITE
				{
					[Note("Gleam (like shiny metals) + Mite")]
					public static LocString NAME = Link("Gleamite", SlagmiteConfig.ID);
					public static LocString DESC = $"Slagmites eat metal rich waste procucts.\n\nTheir shells can be mined while the critter is alive, providing various metals. This does not hurt the critter, they even appeciate the lesser weight on their backs.";
					public static LocString BABY_NAME = Link("Gleamitty", SlagmiteConfig.ID);
					public static LocString BABY_DESC = $"A cute little Gleamitty.\n\nIn time it will mature into a fully grown Slagmite.";
					public static LocString EGG_NAME = Link("Gleamite Egg", SlagmiteConfig.ID);
				}

				public class BEACHED_JELLYFISH
				{
					public static LocString NAME = Link("Jellyfish", JellyfishConfig.ID);
					public static LocString DESC = "...";
					public static LocString BABY_NAME = Link("Jelly Ephyra", JellyfishConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Jellyfish Egg", JellyfishConfig.ID);
				}

				public class BEACHED_ANGULARFISH
				{
					public static LocString NAME = "Angular Fish";
					public static LocString DESC = "...";
					public static LocString BABY_NAME = "Angular Larvae";
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = "Angular Fish Egg";
				}

				public class BEACHED_KARACOO
				{
					public static LocString NAME = Link("Karacoo", KaracooConfig.ID);
					public static LocString DESC = "...";
					public static LocString BABY_NAME = Link("Karacoo hatchling", KaracooConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Karacoo Egg", KaracooConfig.ID);
				}

				public class BEACHED_MOSSYDRECKO
				{
					public static LocString NAME = Link("Mossy Drecko", MossyDreckoConfig.ID);
					public static LocString DESC = $"Mossy Dreckos are nonhostile critters that graze on {Link("Spinorila", SpinorilaConfig.ID)}, {Link("Pincha Pepperplants", SpiceVineConfig.ID)}, {Link("Balm Lily", SwampLilyConfig.ID)} or {Link("Mealwood Plants", BasicSingleHarvestPlantConfig.ID)}.\n" +
						$"\n" +
						$"Their backsides are covered in moss that only grow in {Link("Helium", SimHashes.Helium)} climates.";

					public static LocString BABY_NAME = Link("Mossy Drecklet", MossyDreckoConfig.ID);
					public static LocString BABY_DESC = $"A tiny Mossy Drecklet.\n" +
						$"\n" +
						$"In time it will mature into a fully grown {Link("Mossy Drecko", MossyDreckoConfig.ID)}.";

					public static LocString EGG_NAME = Link("Mossy Drecklet Egg", MossyDreckoConfig.ID);
				}

				public class BEACHED_MUFFIN
				{
					public static LocString NAME = Link("Muffin", MuffinConfig.ID);
					public static LocString DESC = "...";
					public static LocString BABY_NAME = Link("Muffling Whelp", MuffinConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Muffling Whelp Egg", MuffinConfig.ID);
				}

				public class BEACHED_PRINCESSPACU
				{
					public static LocString NAME = Link("Princess Pacu", PrincessPacuConfig.ID);
					public static LocString DESC = "A pacu with glistening scales shining in a myriad colors, this creature brings tears to the eyes of those who catch a glimpse of it from it's sheer beauty.";
					public static LocString BABY_NAME = Link("Princess Fry", MuffinConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Princess Fry Egg", MuffinConfig.ID);
				}

				public class BEACHED_ROTMONGER
				{
					public static LocString NAME = Link("Rotmonger", RotmongerConfig.ID);
					public static LocString DESC = $"Opportunist scavenger who feasts on decomposing organic materials. In it's stomach crystallizes pure {Link("Abyssalite", SimHashes.Katairite)}.";
					public static LocString BABY_NAME = Link("Rotring", RotmongerConfig.ID);
					public static LocString BABY_DESC = "...";
					public static LocString EGG_NAME = Link("Rotring", RotmongerConfig.ID);
				}

				public class BEACHED_DEWPALM
				{
					public static LocString NAME = Link("Dew Palm", DewPalmConfig.ID);
					public static LocString DESC = "WORK IN PROGRESS. Currently can be tapped with a bucket for Rubber. This plant will have more uses in the coming updates.";
					public static LocString DOMESTICATEDDESC = "TODO.";
				}


				public class BEACHED_CORAL_SALTYSTICK
				{
					public static LocString NAME = Link("Salty Sticks", SaltyStickConfig.ID);
					public static LocString DESCRIPTION = "Salty Stick Corals enjoy salt rich waters, the outside of their tube like bodies will build up with salt deposits over time which they eject periodically.\n" +
						"\n" +
						"Filters water saturated with Salt.";
				}

				public class BEACHED_SIDEWAYSPLANT
				{
					public static LocString NAME = Link("Ath-Wart", SidewaysPlantConfig.ID);
					public static LocString DESC = "TODO.";
					public static LocString DOMESTICATEDDESC = "TODO.";
				}

				public class BEACHED_SPINORILA
				{
					[Note("Unused")]
					public static LocString NAME = Link("Spinorila", SpinorilaConfig.ID);
					[Note("Unused")]
					public static LocString DESC = "This tropical plant grows giant edible leaves, a nutritious snack for both Karacoos and Duplicants.";
				}

				public class BEACHED_GLOWYPLANT
				{
					public static LocString NAME = Link("Glowrid", GlowyPlantConfig.ID);
					public static LocString DESC = ".";
				}

				public class BEACHED_BULBLOOM
				{
					public static LocString NAME = "Bulbloom";
					public static LocString DESC = "...";
				}

				public class BEACHED_BAMBOO
				{
					public static LocString NAME = Link("Bamboo", BambooConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_KELP
				{
					public static LocString NAME = Link("Sea Weed", KelpConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_BONEWORM
				{
					public static LocString NAME = Link("Boneworm", BonewormConfig.ID);
					public static LocString DESC = "Sessile bone eating worms that attach themselves to their food source by root like structures. Theirs  protrusions, also called parapodia, are quite mesmerizing to look at.";
					public static LocString DOMESTICATEDDESC = $"This plant improves ambient {Link("Decor", "DECOR")}.";
				}

				public class BEACHED_CORAL_WASHUSPONGE
				{
					public static LocString NAME = Link("Washu Sponge", WashuSpongeConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_WATERCUPS
				{
					public static LocString NAME = Link("Watercups", WaterCupsConfig.ID);
					public static LocString DESC = "The blue cup part is actually a modified leaf called a \"spathe\", protecting a very fragile blue spadix inside. Despite the perfect shape for it, it is not recommended to drink from this flower as it is highly toxic.";
					public static LocString DOMESTICATEDDESC = $"This plant improves ambient {Link("Decor", "DECOR")}.";
				}

				public class BEACHED_FILAMENT
				{
					public static LocString NAME = "Feel-Lament";
					public static LocString DESC = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class BEACHED_OXYBLOON
				{
					[Note("Oxygen + Balloon")]
					public static LocString NAME = Link("Oxybloon", OxybloonConfig.ID);
					public static LocString DESC = "A flowering plant with a large airsack full of breathable Oxygen. Uprooting the plant releases it's contents to the atmosphere. \n" +
						"\n" +
						$"Oxybloons will die when their Oxygen store is depleted. Oxybloons cannot reproduce.";
				}

				public class BEACHED_MUSSEL_SPROUT
				{
					[Note("From Mussel (the mollusc) and Brussel Sprout.")]
					public static LocString NAME = Link("Mussel Sprout", MusselSproutConfig.ID);
					public static LocString DESC = "The mussel sprout resembles a mollusc, with its hard shell and slimy inside. When harvested, it attempts to \"run\" away at speeds so slow they are difficult to observe with the naked eye, with the help of its prehensile tongue. \n" +
						"\n" +
						$"Mussel Sprouts can be harvested for an edible {Link("Mussel Tongue", MusselTongueConfig.ID)}. Mussel Sprouts cannot reproduce.";
				}

				public class BEACHED_POFFSHROOM
				{
					public static LocString NAME = Link("Poff Ball", PoffShroomConfig.ID);
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
					public static LocString NAME = Link("Purpicle", PurpleHangerConfig.ID);
					public static LocString DESC = "...";
				}
			}

			public class FAMILY
			{
				[Note("A slickster in a shell, also \"slick\" as in oily/slimy. It looks like a snail.")]
				public static LocString BEACHEDSLICKSHELL = Link("Slickshell", "BEACHEDSNAILSPECIES");
				[Note("Angler fish with angular patterns.")]
				public static LocString BEACHEDANGULARFISH = Link("Angular Fish", "BEACHEDANGLUARFISHSPECIES");
				[Note("A pip-cat-lemur thing. \"Maki\" means little monkey in an endearing and cute way.")]
				public static LocString BEACHEDMAKI = Link("Maki", "BEACHEDMAKISPECIES");
				[Note("Based on the irl species kakapo.")]
				public static LocString BEACHEDKARACOO = Link("Karacoo", "BEACHEDKARAKOOSPECIES");
				public static LocString BEACHEDJELLYFISH = Link("Jellyfish", "BEACHEDJELLYFISHSPECIES");
				public static LocString BEACHEDMUFFIN = Link("Muffin", "BEACHEDMUFFINSPECIES");
				public static LocString BEACHEDMITE = Link("Mite", "BEACHEDMITESPECIES");
				public static LocString BEACHEDFUA = Link("Fua", "BEACHEDFUAPECIES");
				public static LocString BEACHEDROTMONGER = Link("Rotmonger", "BEACHEDROTMONGERPECIES");
				public static LocString BEACHEDDRIFTER = Link("Drifter", "BEACHEDDRIFTERPECIES");
			}

			public class FAMILY_PLURAL
			{
				[Note("A slickster in a shell, also \"slick\" as in oily/slimy. It looks like a snail.")]
				public static LocString BEACHEDSNAILSPECIES = Link("Slickshells", "BEACHEDSNAILSPECIES");
				[Note("Angler fish with angular patterns.")]
				public static LocString BEACHEDANGULARFISHSPECIES = Link("Angular Fish", "BEACHEDANGLUARFISHSPECIES");
				[Note("A pip-cat-lemur thing. \"Maki\" means little monkey in an endearing and cute way.")]
				public static LocString BEACHEDMAKISPECIES = Link("Makis", "BEACHEDMAKISPECIES");
				[Note("Based on the irl species kakapo.")]
				public static LocString BEACHEDKARACOOSPECIES = Link("Karacoos", "BEACHEDKARACOOSPECIES");
				public static LocString BEACHEDJELLYFISHSPECIES = Link("Jellyfishes", "BEACHEDJELLYFISHSPECIES");
				public static LocString BEACHEDMUFFINSPECIES = Link("Muffins", "BEACHEDMUFFINSPECIES");
				public static LocString BEACHEDMITESPECIES = Link("Mites", "BEACHEDMITESPECIES");
				public static LocString BEACHEDFUAPECIES = Link("Fuas", "BEACHEDFUAPECIES");
				public static LocString BEACHEDROTMONGERSPECIES = Link("Rotmongers", "BEACHEDROTMONGERPECIES");
				public static LocString BEACHEDDRIFTERSPECIES = Link("Drifters", "BEACHEDDRIFTERPECIES");
			}
		}
	}
}
