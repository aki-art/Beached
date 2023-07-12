using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Comets;
using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Entities.Critters;
using Beached.Content.Defs.Equipment;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.ModDb.Sicknesses;
using KLEISTRINGS = STRINGS;

namespace Beached
{
	public class STRINGS
	{
		public class BUILDCATEGORIES
		{
			public class BEACHED_POIS
			{
				public static LocString NAME = "Beached POIs";
				public static LocString TOOLTIP = "debug mode";
			}
		}

		public class BUILDINGS
		{
			public class PREFABS
			{
				public class BEACHED_AMMONIAGENERATOR
				{
					public static LocString NAME = FormatAsLink("Ammonia Generator", AmmoniaGeneratorConfig.ID);
					public static LocString DESC = "Ammonia generators are not very efficient, and emit a lot of waste materials.";
					public static LocString EFFECT = $"Converts {ELEMENTS.AMMONIA.NAME} into electrical {FormatAsLink("Power", "POWER")}, " +
													 $"{KLEISTRINGS.ELEMENTS.WATER.NAME} and {ELEMENTS.NITROGEN.NAME}.";
				}

				public class BEACHED_ATMOSPHERICFORCEFIELDGENERATOR
				{
					public static LocString NAME = FormatAsLink("Atmospheric Forcefield Generator", ForceFieldGeneratorConfig.ID);
					public static LocString DESC = "...";
					public static LocString EFFECTS = "Shields the surface of an asteroid from incoming Meteors. Does not allow rockets to pass.";
				}

				public class BEACHED_AQUARIUM
				{
					public static LocString NAME = "Aquarium";
					public static LocString DESC = "";
					public static LocString EFFECT = "Houses several aquatic critters, plants or corals. Varying effects.";
				}


				public class BEACHED_BAMBOOPLATFORM
				{
					public static LocString NAME = "Bamboo Walkway";
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_CONDUITAQUARIUM
				{
					public static LocString NAME = "Conduit Coral Bed";
					public static LocString DESC = "...";
					public static LocString EFFECT = "Houses a single coral, connected to a liquid conduit.";
				}

				public class BEACHED_DNAINJECTOR
				{
					public static LocString NAME = FormatAsLink("DNA Injector", ForceFieldGeneratorConfig.ID);
					public static LocString DESC = "...";
					public static LocString EFFECTS = "Allows administering DNA samples to Eggs, applying traits to newborn critters.";
				}

				public class BEACHED_INTERPLANETARYPOWEROUTLET
				{
					public static LocString NAME = "Interplanetary Power Outlet";
					public static LocString DESC = "Ranged power!";
					public static LocString EFFECT = "Provides wireless power transmission between two remote locations. The outlet is highly volatile" +
													 "and will electrute and super-heat it's nearby area.";
				}

				public class BEACHED_INTERPLANETARYPOWERINLET
				{
					public static LocString NAME = "Interplanetary Power Inlet";
					public static LocString DESC = "Ranged power!";
					public static LocString EFFECT = "Receives wireless power transmission between two remote locations. The inlet is highly volatile" +
													 "and will electrute and super-heat it's nearby area.";
				}

				public class BEACHED_LABORATORYTILES
				{
					public static LocString NAME = "Laboratory Tile";
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_MAKIHUT
				{
					public static LocString NAME = "Maki Hut";
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_MAKITRAININGGROUND
				{
					public static LocString NAME = "Maki Training Ground";
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_MINIFRIDGE
				{
					public static LocString NAME = FormatAsLink("Mini-Fridge", MiniFridgeConfig.ID);
					public static LocString DESC = "A tiny fridge to store a tiny bit of food for the tiny dupes.";
					public static LocString EFFECT = "TRANSLATION NOT NEEDED - gets copied from regular fridge";
				}

				public class BEACHED_MOSSBED
				{
					public static LocString NAME = FormatAsLink("Moss Frame", MossBedConfig.ID);
					public static LocString DESC = "Grows a single tile of moss over a period of time. Requires a once time delivery of water; once grown the moss is converted to a natural tile.";
					public static LocString EFFECT = "Natural tiles can be used as walls and floors or for wild planting.";
				}

				public class BEACHED_TERRARIUM
				{
					public static LocString NAME = "Terrarium";
					public static LocString DESC = "...";
					public static LocString EFFECT = "";
				}

				public class BEACHED_TREETAP
				{
					public static LocString NAME = "Tree Tap";
					public static LocString DESC = "...";
					public static LocString EFFECT = "Collects Sap from a tree it is attached to.";
				}

				public class BEACHED_CHIME
				{
					public static LocString NAME = FormatAsLink("Chime", ChimeConfig.ID);
					public static LocString DESC = "Pretty things suspended in air, creating music.";
					public static LocString EFFECT = "Emits a soothing sound when stimulated by changing air pressure, decreasing Stress of nearby Duplicants.";
				}

				public class BEACHED_SALTLICK
				{
					[Note("Visible in UI or codex.")]
					public static LocString NAME = "Critter Lick";
					[Note("The name that appears on the actual building. ie. Salt-Lick, or Sulfur-Lick")]
					public static LocString FORMATTED_NAME = "{Element}-Lick";
					public static LocString DESC = "A block of lickable material. Delicious!";
					public static LocString EFFECT = "Allows critters to consume additional materials, boosting production. \n" +
													 $"{CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES} can be fully sustained on appropiate licks.\n\n" +
													 "Requires refilling once depleted.";
				}

				public class BEACHED_SMOKINGRACK
				{
					public static LocString NAME = "Smoking Rack";
					public static LocString DESC = "";

					public static LocString EFFECT =
						"Uses Salt and Carbon Dioxide to smoke food, extending their shelf life and improving quality.\n\n" +
						$"Can also use ambient Carbon Dioxide if it'a already hot enough (at least {GameUtil.GetFormattedTemperature(343.15f)}).";
				}

				public class BEACHED_WOODCARVING
				{
					public static LocString NAME = FormatAsLink("Wood Carving", WoodCarvingConfig.ID);
					public static LocString DESC = "";
					public static LocString EFFECT = "";

					public class FACADES
					{
						public class OWL
						{
							public static LocString NAME = "Wood Owl";
							public static LocString DESC = "";
						}

						public class PIGTOTEM
						{
							public static LocString NAME = "Pig Totem";
							public static LocString DESC = "";
						}

						public class MAJORAMASK
						{
							public static LocString NAME = "Odd Mask";
							public static LocString DESC = "";
						}

						public class TERRIER
						{
							public static LocString NAME = "The Good Terrier";
							public static LocString DESC = "";
						}
					}
				}
			}

			public class STATUSITEMS
			{
				public class BEACHED_LUBRICATED
				{
					public static LocString NAME = "Lubricated";

					public static LocString TOOLTIP = "A {0} has been placed on this bed.\n\n<b>Effects</b>";
				}

				public class BEACHED_PLUSHED
				{
					public static LocString NAME = "{0} Plush";

					public static LocString TOOLTIP = "A {0} has been placed on this bed.\n\n<b>Effects</b>";
				}
			}
		}

		public class CODEX
		{
			public class STORY_TRAITS
			{
				public class AFFG
				{
					public static LocString NAME = "Atmospheric Force-Field Generator";
					public static LocString DESCRIPTION = "TODO";
				}
			}
		}

		public class COLONY_ACHIEVEMENTS
		{
			public class MISC_REQUIREMENTS
			{
			}
		}

		public class COMETS
		{
			public class SHRAPNEL
			{
				public static LocString NAME = "Shrapnel";
				public static LocString DESC = "Small piece of a metal violently flung from an explosion.";
			}

			public class BEACHED_DIAMONDCOMET
			{
				public static LocString NAME = FormatAsLink("Diamond Comet", DiamondCometConfig.ID);
				public static LocString DESC = "TODO";
			}

			public class BEACHED_SPARKLINGZIRCONCOMET
			{
				public static LocString NAME = FormatAsLink("Sparkling Zircon Comet", SparklingZirconCometConfig.ID);
				public static LocString DESC = "TODO";
			}

			public class BEACHED_SPARKLINGAQUAMARINECOMET
			{
				public static LocString NAME = FormatAsLink("Sparkling Aquamarine Comet", SparklingAquamarineCometConfig.ID);
				public static LocString DESC = "TODO";
			}

			public class BEACHED_SPARKLINGDIAMONDCOMET
			{
				public static LocString NAME = FormatAsLink("Sparkling Diamond Comet", "");
				public static LocString DESC = "TODO";
			}

			public class BEACHED_SPARKLINGVOIDCOMET
			{
				public static LocString NAME = FormatAsLink("Sparkling Void Comet", SparklingVoidCometConfig.ID);
				public static LocString DESC = "TODO";
			}
		}

		public class CORALS
		{
			public class BEACHED_CORAL_WASHUSPONGE
			{
				public static LocString NAME = "Washu Sponge";
				public static LocString DESCRIPTION = "Woshu Sponge can sustain itself by consuming germs. This happens to be very useful for those wishing to keep their liquid reservoirs clean.\n" +
					"\n" +
					"Removes germs from liquids, and produces edible frags.";
			}

			public class BEACHED_SALTY_STICK_CORAL
			{
				public static LocString NAME = "Saltis Tik Coral";
				public static LocString DESCRIPTION = "Saltis Tik Corals enjoy salt rich waters, their tubes outside will build up with salt deposits over time which they eject periodically.\n" +
					"\n" +
					"Filters Salt Water or Brine, producing Salt and Water.";
			}

			public class BEACHED_VISCOUS_CORAL
			{
				public static LocString NAME = "Viscous Coral";
				public static LocString DESCRIPTION = "Viscous Corals really like to consume oil, very unlike most other sea life.\n" +
					"\n" +
					"{DescriptionEnd}";

				public static LocString DESCRIPTION_END_NO_BITUMEN = "Converts Crude Oil into Petroleum.";
				public static LocString DESCRIPTION_END_YES_BITUMEN = "Converts Crude Oil into Petroleum and Bitumen.";
			}
		}

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

				public class BEACHED_GENETICALLYMODIFIED
				{
					public static LocString NAME = "Genetically Altered";
					public static LocString TOOLTIP = "This egg has been genetically modified: \n" +
						"{0}\n\n" +
						"It cannot receive any more modifications.";
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

					public class CORAL_REEF
					{
						public static LocString NAME = "Coral Reef";
						public static LocString DESC = $"A geyser that periodically erupts with" +
							$" {FormatAsLink("Salt Water", SimHashes.SaltWater.ToString().ToUpperInvariant())} rich in " +
							$"{FormatAsLink("Plankton", "PlanktonGerms"/*PlanktonGerms.ID.ToUpperInvariant()*/)}.";
					}

					public class BISMUTH_VOLCANO
					{
						public static LocString NAME = "Bismuth Volcano";
						public static LocString DESC = $"A large volcano that periodically erupts with {ELEMENTS.BISMUTHMOLTEN.NAME}";
					}

					public class PACU_GEYSER
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
						public static LocString NAME = FormatAsLink("Small Cell", CellAlgaeConfig.ID);
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
					public static LocString DESCRIPTION = "A coral with green capitulum resembling cabbage leaves. This coral consumes Hydrogen directly from fresh Water, releasing the excess Oxygen in the process.\n" +
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
					public static LocString NAME = FormatAsLink("Bubble Algae", CellAlgaeConfig.ID);
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

				public class DEWPALM
				{
					public static LocString NAME = "Dew Palm";
					public static LocString DESC = "...";
				}
				public class MANGROVE
				{
					public static LocString NAME = "Gutwood";
					public static LocString DESC = "...";
				}

				public class BULBLOOM
				{
					public static LocString NAME = "Bulbloom";
					public static LocString DESC = "...";
				}

				public class BAMBOO
				{
					public static LocString NAME = "Bamboo"; //Clickety Clack? Clack Cane?
					public static LocString DESC = "...";
				}

				public class KELP
				{
					public static LocString NAME = "Kelp";
					public static LocString DESC = "...";
				}


				public class WATERCUPS
				{
					public static LocString NAME = "Watercups";
					public static LocString DESC = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class FILAMENT
				{
					public static LocString NAME = "Filament";
					public static LocString DESC = "...";
					public static LocString DOMESTICATEDDESC = "...";
				}

				public class MUSSEL_SPROUT
				{
					public static LocString NAME = "Mussel Sprout";
					public static LocString DESC = "...";
				}

				public class POFFSHROOM
				{
					public static LocString NAME = "Poff Ball";
					public static LocString DESC = "...";
				}

				public class GLOWCAP
				{
					public static LocString NAME = "Gloom Shroom";
					public static LocString DESC = "...";
				}

				public class BLADDERTREE
				{
					public static LocString NAME = "Gutwood";
					public static LocString DESC = "...";
				}

				public class DEEPGRASS
				{
					public static LocString NAME = "Deepgrass";
					public static LocString DESC = "...";
				}

				internal class PURPLEHANGER
				{
					public static LocString NAME = "Purpicle";
					public static LocString DESC = "...";
				}
			}

			public class FAMILY
			{
				[Note("A slickster in a shell, also \"slick\" as in oily/slimy. It looks like a snail.")]
				public static LocString BEACHEDSLICKSHELL = "Slickshell";
				[Note("Angler fish with angular patterns.")]
				public static LocString BEACHEDANGULARFISH = "Angular Fish";
				[Note("A pip-cat-lemur thing. \"Maki\" means little monkey in an endearing and cute way.")]
				public static LocString BEACHEDMAKI = "Maki";
				public static LocString BEACHEDJELLYFISH = "Jellyfish";
			}

			public class FAMILY_PLURAL
			{
				[Note("A slickster in a shell, also \"slick\" as in oily/slimy. It looks like a snail.")]
				public static LocString BEACHEDSNAILSPECIES = "Slickshells";
				[Note("Angler fish with angular patterns.")]
				public static LocString BEACHEDANGULARFISHSPECIES = "Angular Fish";
				[Note("A pip-cat-lemur thing. \"Maki\" means little monkey in an endearing and cute way.")]
				public static LocString BEACHEDMAKISPECIES = "Makis";
				public static LocString BEACHEDJELLYFISHSPECIS = "Jellyfish";
			}
		}

		public class CLUSTER_NAMES
		{
			public class OCEANARIA
			{
				public static LocString NAME = "Oceanaria";
				public static LocString DESCRIPTION = "<description>";
			}

			public class BEACHEDTEST
			{
				public static LocString NAME = "Beached Test";
				public static LocString DESCRIPTION = "<description>";
			}
		}

		public class DEATHS
		{
			public class DESICCATION
			{
				public static LocString NAME = "Desiccation";
				public static LocString DESCRIPTION = "...";
			}
		}

		public class DUPLICANTS
		{
			public class ATTRIBUTES
			{
				public class BEACHED_PRECISION
				{
					public static LocString NAME = "Precision";
					public static LocString DESC = "...";
				}

				// Building attributes
				// The game expects them under DUPLICANT
				public class BEACHED_BUILDING_ACIDVULNERABILITY
				{
					public static LocString NAME = "Acid Vulnerability";
					public static LocString DESC = "...";
				}
			}

			public class CHOREGROUPS
			{
				public class BEACHED_CHOREGROUP_HANDYWORK
				{
					public static LocString NAME = "Precision Work";
					public static LocString DESC = "...";
				}
			}

			public class PERSONALITIES
			{
				public class BEACHED_MINNOW
				{
					public static LocString NAME = "MinnowDISPLAYNAME";
					public static LocString DESC = "Minnow's printing label actually says \"Experiment MRM8\", but she prefers to go by Minnow.";
				}

				public class BEACHED_MIKA
				{
					public static LocString NAME = "Mika";
					public static LocString DESC = "It is scientifically impossible to not get along with a Mika. He's just that cool.";
				}

				public class BEACHED_VAHANO
				{
					public static LocString NAME = "Va'Hano";
					public static LocString DESC = "This Va'Hano's favorite food is roasted Tropical Pacu, served extra rare.";
				}
			}

			public class ROLES
			{
				public class ANIMALHANDLING
				{
					public static LocString NAME = FormatAsLink("Animal Handling Basics", BSkills.ANIMALHANDLING_ID);
					public static LocString DESCRIPTION = "";
				}

				public class ARCHEOLOGY
				{
					public static LocString NAME = FormatAsLink("Careful Excavation", BSkills.ARCHEOLOGY_ID);
					public static LocString DESCRIPTION = "Allows a Duplicant to find hidden treasures while digging around in soft materials.";
				}

				public class ARCHEOLOGY2
				{
					public static LocString NAME = FormatAsLink("Archeology", BSkills.ARCHEOLOGY2_ID);
					public static LocString DESCRIPTION = "Improves a Duplican't ability to find hidden treasures while digging around, and they can now find items in hard materials.";
				}

				public class AQUACULTURE1
				{
					public static LocString NAME = FormatAsLink("Aquaculture I", BSkills.AQUACULTURE1_ID);
					public static LocString DESCRIPTION = "Better harvesting gof aquatic plants, such as corals or algaes.";
				}

				public class AQUACULTURE2
				{
					public static LocString NAME = FormatAsLink("Aquaculture II", BSkills.AQUACULTURE1_ID);
					public static LocString DESCRIPTION = "Better harvesting gof aquatic plants, such as corals or algaes.";
				}

				public class MAKITRAINER1
				{
					public static LocString NAME = FormatAsLink("Novice Maki Trainer", BSkills.MAKITRAINING1_ID);
					public static LocString DESCRIPTION = "Can train level 1 skills to Makis.";
				}

				public class MAKITRAINER2
				{
					public static LocString NAME = FormatAsLink("Master Maki Trainer", BSkills.MAKITRAINING2_ID);
					public static LocString DESCRIPTION = "Can train level 2 skills to Makis.";
				}

				public class CRYSTALLOGRAPHY
				{
					public static LocString NAME = FormatAsLink("Crystallography", BSkills.CRYSTALLOGRAPHY_ID);
					public static LocString DESCRIPTION = "Allows a Duplicant to harvest a Crystal Cluster without breaking it, allowing regrowth.";
				}

				public class GEOCHEMISTRY
				{
					public static LocString NAME = FormatAsLink("Geo-Chemistry", BSkills.GEOCHEMISTRY_ID);
					public static LocString DESCRIPTION = "Enables a Duplicant to analyze crystals, allowing for synthesis of new Crystal Clusters.";
				}
			}

			public class TRAITS
			{
				public static LocString GENETIC_TRAIT = "This is a genetic trait always paired with a {0}.";

				public class BEACHED_GILLS
				{
					public static LocString NAME = "Gills";
					public static LocString SHORT_DESC = "Amphibious";
					public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can also breath in any kind of Water. Does not grant immunity to wet debuffs.";
					public static LocString DESC = "This duplicant can live like a fish in the water... or at least breath like one.";
					public static LocString WATERBREATHING = "• Amphibious";
				}

				public class BEACHED_COMFORT_SEEKER
				{
					public static LocString NAME = "Comfort Seeker";
					public static LocString DESC = "This duplicant gains a moral bonus from wearing comfortable clothing.";
				}

				public class BEACHED_PLUSHIE_MAKER
				{
					public static LocString NAME = "Plushie Gifter";
					public static LocString DESC = "...";
				}

				public class BEACHED_SIREN
				{
					public static LocString NAME = "Scary Scales";
					public static LocString DESC = "...";
				}

				public class PRECISIONUP
				{
					public static LocString NAME = "Dexterous";
					public static LocString DESC = "This duplicant can live like a fish in the water... or at least breath like one.";
				}

				public class BEACHED_ASMRLOVER
				{
					public static LocString NAME = "ASMR Fan";
					public static LocString DESC = "This duplicant sleeps better with something to listen to.";
				}

				public class BEACHED_FURALLERGY
				{
					public static LocString NAME = "Fur Allergy";
					public static LocString DESC = "This duplicant will sneeze and get hives when exposed to furry critters, such as Pips, Pufts or Makis.";
				}

				public class BEACHED_SEAFOODALLERGY
				{
					public static LocString NAME = "Sea-Food Allergy";
					public static LocString DESC = "...";
				}
			}

			public class DISEASES
			{
				public class BEACHED_FUR_ALLERGY
				{
					public static LocString SOURCE = "Contact with {Critter}";
				}

				public class BEACHED_PLANKTON
				{
					public static LocString NAME = FormatAsLink("Plankton", PlanktonGerms.ID);
					public static LocString DESCRIPTION = FormatAsLink("Tiny organisms floating in water. These microscopic " +
						"creatures are harmless to Duplicants. " +
						"They are too small to see by the naked eye, the Germ Overlay will reveal them instead.");
				}

				public class BEACHED_LIMPETEGG
				{
					public static LocString NAME = FormatAsLink("Limpet Eggs", LimpetEggGerms.ID);
				}

				public class BEACHED_CAPSPORE
				{
					public static LocString NAME = FormatAsLink("Grimcap Spores", CapSporeGerms.ID);
				}

				public class BEACHED_POFFSPORE
				{
					public static LocString NAME = FormatAsLink("Poffshroom Spores", PoffSporeGerms.ID);
				}

				public class BEACHED_LIMPETS_DUPLICANT
				{
					public static LocString NAME = "NA"; //KUI.FormatAsLink("Limpets", LimpetsSickness.ID);
				}

				public class BEACHED_SICKNESS_CAPPED
				{
					public static LocString NAME = FormatAsLink("Capped", CappedSickness.ID);
				}

				public class BEACHED_SICKNESS_POFFMOUTH
				{
					public static LocString NAME = FormatAsLink("Poffmouth", PoffSporeGerms.ID);
				}

				public class BEACHED_SICKNESS_TELEPORTITS
				{
					public static LocString NAME = FormatAsLink("Teleportitis", "");
				}

				public static LocString LIGHTEXPOSURE = "Exposed to Light. Approximately {0} change per second.";
			}


		}

		public class EFFECTS
		{
			public class BEACHED_STEPPEDINMUCUS
			{
				public static LocString NAME = "Slippery Feet";
				public static LocString DESC = $"This duplicant has stepped in {ELEMENTS.MUCUS.NAME}.";
			}

			public class BEACHED_MUCUS_SOAKED
			{
				public static LocString NAME = "Slimy";
				public static LocString DESC = $"This duplicant has recently submerged in {ELEMENTS.MUCUS.NAME}.";
			}

			public class BEACHED_OCEANBREEZE
			{
				public static LocString NAME = "Ocean Breeze";
				public static LocString DESC = $"Breathing in fresh {ELEMENTS.SALTYOXYGEN.NAME} is improving this duplicants mood and respiration.";
			}

			public class BEACHED_POFFCLEANEDTASTEBUDS
			{
				public static LocString NAME = "Cleansed Palate";
				public static LocString DESC = $"This duplicant is less picky about their food.";
			}

			public class BEACHED_EFFECT_PLUSHIEVOLE
			{
				public static LocString NAME = "Shove-Vole Plushie";
				public static LocString DESC = $"This duplicant owns an adorable pup plush.";
			}

			public class BEACHED_EFFECT_PLUSHIEPUFT
			{
				public static LocString NAME = "Puft Plushie";
				public static LocString DESC = $"This duplicant owns a wonderfully round puft plushie.";
			}

			public class BEACHED_EFFECT_PLUSHIEPACU
			{
				public static LocString NAME = "Pacu Plushie";
				public static LocString DESC = $"This duplicant owns a soft and squishy Pacu plushie.";
			}

			public class BEACHED_LIMPETHOST
			{
				public static LocString NAME = "Limpets";

				public static LocString DESC = "This critter is being overgrown by Limpets. \n" +
						"Once fully grown, they can be sheared off for resources.";
			}

			class BEACHED_SCARED
			{
				public static LocString NAME = "Scared";
				public static LocString DESC = $"This duplicant has been terrified by darkness.";
			}

			class BEACHED_SCARED_SIREN
			{
				public static LocString NAME = "Scared";
				public static LocString DESC = $"This duplicant has seen something really scary!";
			}

			public class BEACHED_WISHINGSTAR
			{
				public static LocString NAME = "Wishing Star";
				public static LocString DESC = "This Duplicant has wished upon a star. What did they wish for? " +
					"Can't tell, it's birthday wish rules, if they say it won't come true!";
			}
		}

		public class ELEMENTS
		{
			public class AMBER
			{
				public static LocString NAME = FormatAsLink("Amber");
				public static LocString DESC = "TODO";
			}

			public class AMMONIA
			{
				public static LocString NAME = FormatAsLink("Ammonia");
				public static LocString DESC = "TODO";
			}

			public class AMMONIAFROZEN
			{
				public static LocString NAME = FormatAsLink("Frozen Ammonia", Elements.ammoniaFrozen.ToString());
				public static LocString DESC = "TODO";
			}

			public class AMMONIALIQUID
			{
				public static LocString NAME = FormatAsLink("Liquid Ammonia", Elements.ammoniaLiquid.ToString());
				public static LocString DESC = "TODO";
			}

			public class ASH
			{
				public static LocString NAME = FormatAsLink("Ash");
				public static LocString DESC = "TODO";
			}

			public class AQUAMARINE
			{
				public static LocString NAME = FormatAsLink("Aquamarine");
				public static LocString DESC = "TODO";
			}

			public class BASALT
			{
				public static LocString NAME = FormatAsLink("Basalt");
				public static LocString DESC = "TODO";
			}

			public class BERYLLIUM
			{
				public static LocString NAME = FormatAsLink("Beryllium");
				public static LocString DESC = "TODO";
			}

			public class BERYLLIUMGAS
			{
				public static LocString NAME = FormatAsLink("Beryllium Gas", Elements.berylliumGas.ToString());
				public static LocString DESC = "TODO";
			}

			public class BERYLLIUMMOLTEN
			{
				public static LocString NAME = FormatAsLink("Molten Beryllium", Elements.berylliumMolten.ToString());
				public static LocString DESC = "TODO";
			}

			public class BISMUTH
			{
				public static LocString NAME = FormatAsLink("Bismuth");
				public static LocString DESC = "TODO";
			}

			public class BISMUTHGAS
			{
				public static LocString NAME = FormatAsLink("Bismuth Gas", Elements.bismuthGas.ToString());
				public static LocString DESC = "TODO";
			}

			public class BISMUTHMOLTEN
			{
				public static LocString NAME = FormatAsLink("Molten Bismuth", Elements.bismuthMolten.ToString());
				public static LocString DESC = "TODO";
			}

			public class BISMUTHORE
			{
				public static LocString NAME = FormatAsLink("Bismuth Ore", Elements.bismuthOre.ToString());
				public static LocString DESC = "TODO";
			}

			public class BONE
			{
				public static LocString NAME = FormatAsLink("Bone");
				public static LocString DESC = "TODO";
			}

			public class CALCIUM
			{
				public static LocString NAME = FormatAsLink("Calcium", Elements.calcium.ToString());
				public static LocString DESC = "TODO";
			}

			public class CALCIUMGAS
			{
				public static LocString NAME = FormatAsLink("Calcium Gas", Elements.calciumGas.ToString());
				public static LocString DESC = "TODO";
			}

			public class CALCIUMMOLTEN
			{
				public static LocString NAME = FormatAsLink("Molten Calcium", Elements.calciumMolten.ToString());
				public static LocString DESC = "TODO";
			}

			public class COQUINA
			{
				public static LocString NAME = FormatAsLink("Coquina");
				public static LocString DESC = "TODO";
			}

			public class CRACKEDNEUTRONIUM
			{
				public static LocString NAME = FormatAsLink("Cracked Neutronium");
				public static LocString DESC = "This Neutronium has been shattered loose by unknown forces. It is highly unstable, and attempting to mine it" +
											   "will yield nothing.";
			}

			public class GRAVEL
			{
				public static LocString NAME = FormatAsLink("Gravel");
				public static LocString DESC = "Coarse loose material full of pebbles and dirt.";
			}

			public class HEULANDITE
			{
				public static LocString NAME = FormatAsLink("Zeolite");
				public static LocString DESC = "TODO";
			}

			public class LATEX
			{
				public static LocString NAME = FormatAsLink("Latex");
				public static LocString DESC = "TODO";
			}

			public class LITTER
			{
				public static LocString NAME = FormatAsLink("Litter");
				public static LocString DESC = "TODO";
			}

			public class IRIDIUM
			{
				public static LocString NAME = FormatAsLink("Iridium");
				public static LocString DESC = "TODO";
			}

			public class IRIDIUMGAS
			{
				public static LocString NAME = FormatAsLink("Iridium Gas");
				public static LocString DESC = "TODO";
			}

			public class IRIDIUMMOLTEN
			{
				public static LocString NAME = FormatAsLink("Iridium");
				public static LocString DESC = "TODO";
			}

			[Note("Made up element, Radioactive high end material upgraded from Iridium and Radium.")]
			public class IRRADIUM
			{
				public static LocString NAME = FormatAsLink("Irradium");
				public static LocString DESC = "TODO";
			}

			public class METAMORPHICROCK
			{
				public static LocString NAME = FormatAsLink("Metamorphic Rock");
				public static LocString DESC = "TODO";
			}

			public class MOSS
			{
				public static LocString NAME = FormatAsLink("Moss");
				public static LocString DESC = "TODO";
			}

			public class MUCUS
			{
				[Note("Snail mucus")]
				public static LocString NAME = FormatAsLink("Mucus");
				public static LocString DESC = "TODO";
			}

			public class MUCUSFROZEN
			{
				[Note("Snail mucus")]
				public static LocString NAME = FormatAsLink("Frozen Mucus", Elements.mucusFrozen.ToString());
				public static LocString DESC = "TODO";
			}

			public class MURKYBRINE
			{
				[Note("basically Polluted Brine")]
				public static LocString NAME = FormatAsLink("Murky Brine", Elements.murkyBrine.ToString());
				public static LocString DESC = "TODO";
			}

			public class MYCELIUM
			{
				public static LocString NAME = FormatAsLink("Mycelium");
				public static LocString DESC = "TODO";
			}

			public class NITROGEN
			{
				public static LocString NAME = FormatAsLink("Nitrogen");
				public static LocString DESC = "TODO";
			}

			public class PEARL
			{
				[Note("Solidified mass of pearls stuck together")]
				public static LocString NAME = FormatAsLink("Pearl");
				public static LocString DESC = "TODO";
			}

			public class PERMAFROST
			{
				public static LocString NAME = FormatAsLink("Permafrost");
				public static LocString DESC = "TODO";
			}

			public class ROT
			{
				public static LocString NAME = FormatAsLink("Rot");
				public static LocString DESC = "TODO";
			}

			public class ROOT
			{
				public static LocString NAME = FormatAsLink("Root");
				public static LocString DESC = "TODO";
			}

			public class RUBBER
			{
				[Note("Natural rubber made from tree latex.")]
				public static LocString NAME = FormatAsLink("Rubber");
				public static LocString DESC = "TODO";
			}

			public class SALTYOXYGEN
			{
				[Note("Air with salty water vapour suspended in it, like near seas and oceans.")]
				public static LocString NAME = FormatAsLink("Salty Oxygen", Elements.saltyOxygen.ToString());
				public static LocString DESC = "TODO";
			}

			public class SALTYOXYGENFROZEN
			{
				[Note("Air with salty water vapour suspended in it, like near seas and oceans.")]
				public static LocString NAME = FormatAsLink("Frozen Salty Oxygen");
				public static LocString DESC = "TODO";
			}

			public class SOURBRINE
			{
				[Note("Brine with high concentration of Hydrogen sulfide & Methane (aka Sour Gas)")]
				public static LocString NAME = FormatAsLink("Sour Brine");
				public static LocString DESC = "TODO";
			}

			public class SOURBRINEICE
			{
				[Note("Brine with high concentration of Hydrogen sulfide & Methane (aka Sour Gas)")]
				public static LocString NAME = FormatAsLink("Sour Brine Ice");
				public static LocString DESC = "TODO";
			}

			public class SELENITE
			{
				public static LocString NAME = FormatAsLink("Selenite");
				public static LocString DESC = "TODO";
			}

			public class SILTSTONE
			{
				public static LocString NAME = FormatAsLink("Siltstone");
				public static LocString DESC = "TODO";
			}

			public class SILT
			{
				public static LocString NAME = FormatAsLink("Silt");
				public static LocString DESC = "TODO";
			}

			public class SULFUROUS_ICE
			{
				public static LocString NAME = FormatAsLink("Frozen Sulfuric Acid", Elements.sulfurousIce.ToString());
				public static LocString DESC = "TODO";
			}

			public class SULFUROUS_WATER
			{
				public static LocString NAME = FormatAsLink("Sulfuric Acid", Elements.sulfurousWater.ToString());
				public static LocString DESC = "TODO";
			}

			public class ZINC
			{
				public static LocString NAME = FormatAsLink("Zinc");
				public static LocString DESC = "TODO";
			}

			public class ZINCORE
			{
				public static LocString NAME = FormatAsLink("Zinc Ore");
				public static LocString DESC = "TODO";
			}

			public class ZINCGAS
			{
				public static LocString NAME = FormatAsLink("Zinc Gas", Elements.zincGas.ToString());
				public static LocString DESC = "TODO";
			}

			public class ZINCMOLTEN
			{
				public static LocString NAME = FormatAsLink("Molten Zinc", Elements.zincMolten.ToString());
				public static LocString DESC = "TODO";
			}

			public class ZIRCONIUM
			{
				public static LocString NAME = FormatAsLink("Zirconium");
				public static LocString DESC = "TODO";
			}

			public class ZIRCONIUMORE
			{
				public static LocString NAME = FormatAsLink("Zircon");
				public static LocString DESC = "TODO";
			}

			public class ZIRCONIUMGAS
			{
				public static LocString NAME = FormatAsLink("Zirconium Gas", Elements.zirconiumGas.ToString());
				public static LocString DESC = "TODO";
			}

			public class ZIRCONIUMMOLTEN
			{
				public static LocString NAME = FormatAsLink("Molten Zirconium", Elements.zirconiumMolten.ToString());
				public static LocString DESC = "TODO";
			}
		}

		public class EQUIPMENT
		{
			public class PREFABS
			{
				public class BEACHSHIRT
				{
					public static LocString NAME = "Beach Shirt";
				}

				public class BEACHED_EQUIPMENT_RUBBERBOOTS
				{
					public static LocString NAME = FormatAsLink("Rubber Boots", RubberBootsConfig.ID);
					public static LocString GENERICNAME = "Rubber Boots";
					public static LocString DESCRIPTION = "Protects the feet of Duplicants from being wet.";
				}

				public class BEACHED_EQUIPMENT_MAXIXEPENDANT
				{
					public static LocString NAME = FormatAsLink("Maxixe Pendant", MaxixePendantConfig.ID);
					public static LocString GENERICNAME = "Maxixe Pendant";
					public static LocString DESCRIPTION = "...";
				}

				public class BEACHED_EQUIPMENT_PEARLNECKLACE
				{
					public static LocString NAME = FormatAsLink("Pearl Necklace", PearlNecklaceConfig.ID);
					public static LocString GENERICNAME = "Pearl Necklace";
					public static LocString DESCRIPTION = "This pearlescent shine would swoon anyone!";
				}

				public class BEACHED_EQUIPMENT_HADEANZIRCONAMULET
				{
					public static LocString NAME = FormatAsLink("Hadean Zircon Amulet", HadeanZirconAmuletConfig.ID);
					public static LocString GENERICNAME = "Hadean Zircon Amulet";
					public static LocString DESCRIPTION = "The bright red sparkle of this gem reminds Duplicants of the importance of working hard.";
				}

				public class BEACHED_EQUIPMENT_HEMATITENECKLACE
				{
					public static LocString NAME = FormatAsLink("Hematite Necklace", HematiteNecklaceConfig.ID);
					public static LocString GENERICNAME = "Hematite Necklace";
					public static LocString DESCRIPTION = "Heavy and stylish.";
				}

				public class BEACHED_EQUIPMENT_ZEOLITEPENDANT
				{
					public static LocString NAME = FormatAsLink("Zeolite Pendant", ZeolitePendantConfig.ID);
					public static LocString GENERICNAME = "Zeolite Pendant";
					public static LocString DESCRIPTION = "This gemstone has a particularly soothing color. When duplicants look at it, they feel at ease.";
				}

				public class BEACHED_EQUIPMENT_STRANGEMATTERAMULET
				{
					[Note("https://en.wikipedia.org/wiki/Strange_matter")]
					public static LocString NAME = FormatAsLink("Strange Amulet", StrangeMatterAmuletConfig.ID);
					public static LocString GENERICNAME = "Strange Amulet";
					public static LocString DESCRIPTION = "A piece of material with Universe-defying qualities; safely contained.";
				}
			}
		}

		public class GERMS
		{
			public class BEACHED_PLANKTON
			{
				public static LocString NAME = "Plankton";
			}

			public class BEACHED_LIMPETEGG
			{
				public static LocString NAME = "Limpet Eggs";
			}

			public class BEACHED_CAPSPORE
			{
				public static LocString NAME = "Grimcap Spores";
			}
		}

		public class ITEMS
		{
			public class MISC
			{
				public class BEACHED_GENETIC_SAMPLE
				{
					public static LocString NAME = "{0} Genetic Sample";
					public static LocString DESC = "...";
				}

				public class BEACHED_PALATECLEANSERFOOD
				{
					public static LocString NAME = "Palate Cleanser";
					public static LocString DESC = "...";
				}

				public class BEACHED_BLUEPRINT
				{
					public static LocString NAME = "Blueprint: {0}";
					public static LocString DESC = "...";
				}

				public class BEACHED_SEASHELL
				{
					public static LocString NAME = "Seashell";
					public static LocString DESC = "...";
				}
			}

			public class FOOD
			{
				public class BEACHED_ASPICLICE
				{
					public static LocString NAME = FormatAsLink("Aspic Lice", JellyConfig.ID);
					public static LocString DESC = "Lice suspended in jello.";
				}

				public class BEACHED_ASTROBAR
				{
					public static LocString NAME = FormatAsLink("Astrobar", AstrobarConfig.ID);
					public static LocString DESC = "Delicious and nutritious candy bar, with a sticky and gooey filling that " +
												   "sticks to the roof of the mouth.";
				}

				public class BEACHED_LEGENDARYSTEAK
				{
					public static LocString NAME = FormatAsLink("Legendary Steak", LegendarySteakConfig.ID);
					public static LocString DESC = "Truly, the rarest steak of them all. A wonderful cut of meat that melts in the mouth.";
				}

				public class BEACHED_BERRYJELLY
				{
					public static LocString NAME = FormatAsLink("Berry-Jelly", JellyConfig.ID);
					public static LocString DESC = "A jiggly treat hiding tasty berries within.";
				}

				public class BEACHED_NUTTYDELIGHT
				{
					public static LocString NAME = "Nutty Delight";
					public static LocString DESC = "...";
				}

				public class BEACHED_STUFFEDSNAIL
				{
					public static LocString NAME = "Stuffed Snail";
					public static LocString DESC = "...";
				}

				public class BEACHED_RAWSNAIL
				{
					public static LocString NAME = "Raw Snail";
					public static LocString DESC = "...";
				}

				public class BEACHED_SPONGECAKE
				{
					public static LocString NAME = "Sponge Cake";
					public static LocString DESC = "Fluffy and delighful, with a hint of seafood flavor.";
				}

				public class BEACHED_CRABCAKE
				{
					public static LocString NAME = "Crabcake";
					public static LocString DESC = "...";
				}

				public class BEACHED_JELLY
				{
					public static LocString NAME = FormatAsLink("Jelly", JellyConfig.ID);
					public static LocString DESC = "See-through edible blob. It tastes like solidified water.";
				}

				public class BEACHED_LIMPET
				{
					public static LocString NAME = "Raw Limpet";
					public static LocString DESC = "...";
				}

				public class BEACHED_MUSSELTONGUE
				{
					public static LocString NAME = FormatAsLink("Mussel Tongue", MusselTongueConfig.ID);
					public static LocString DESC = "Edible tongue of a Mussel Sprout. It has a somewhat bitter, green flavour, and the consistency of fresh clam.";
				}

				public class BEACHED_GLAZEDDEWNUT
				{
					public static LocString NAME = "Glazed Dewnut";
					public static LocString DESC = "...";
				}

				public class BEACHED_HIGHQUALITYMEAT
				{
					public static LocString NAME = FormatAsLink("High Quality Meat", HighQualityMeatConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_SMOKEDMEAT
				{
					public static LocString NAME = FormatAsLink("Smoked Meat", SmokedMeatConfig.ID);
					public static LocString DESC = "Meat imbued with the wonderful aroma of smoke.";
				}

				public class BEACHED_SMOKEDFISH
				{
					public static LocString NAME = FormatAsLink("Smoked Fish", SmokedFishConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_SMOKEDLICE
				{
					public static LocString NAME = FormatAsLink("Smoked Lice", SmokedLiceConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_SMOKEDPLANTMEAT
				{
					public static LocString NAME = "Smoked PLant Meat";
					public static LocString DESC = "...";
				}

				public class BEACHED_SMOKEDSNAIL
				{
					public static LocString NAME = "Smoked Snail";
					public static LocString DESC = "...";
				}

				public class BEACHED_SMOKEDTOFU
				{
					public static LocString NAME = FormatAsLink("Smoked Tofu", SmokedTofuConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_OXYGEN_POFF_RAW
				{
					public static LocString NAME = FormatAsLink("Blue Poff", PoffConfig.GetRawId(SimHashes.Oxygen));
					public static LocString DESC = "...";
				}

				public class BEACHED_OXYGEN_POFF_COOKED
				{
					public static LocString NAME = FormatAsLink("Cooked Blue Poff", PoffConfig.GetCookedId(SimHashes.Oxygen));
					public static LocString DESC = "...";
				}

				public class BEACHED_NITROGEN_POFF_RAW
				{
					public static LocString NAME = FormatAsLink("White Poff", PoffConfig.GetRawId(Elements.nitrogen));
					public static LocString DESC = "So bland, it sucks the taste out of the tongue.";
				}

				public class BEACHED_NITROGEN_POFF_COOKED
				{
					public static LocString NAME = FormatAsLink("Cooked White Poff", PoffConfig.GetCookedId(Elements.nitrogen));
					public static LocString DESC = "Still has an astonishing lack of flavor, but now it's also mushy.";
				}

				public class BEACHED_SALTYOXYGEN_POFF_RAW
				{
					public static LocString NAME = FormatAsLink("Pale Poff", PoffConfig.GetRawId(Elements.saltyOxygen));
					public static LocString DESC = "Tastes like an eraser.";
				}

				public class BEACHED_SALTYOXYGEN_POFF_COOKED
				{
					public static LocString NAME = FormatAsLink("Cooked Pale Poff", PoffConfig.GetCookedId(Elements.saltyOxygen));
					public static LocString DESC = "Pleasantly chewy, with a strong flavor of salt.";
				}

				public class BEACHED_AMMONIA_POFF_RAW
				{
					public static LocString NAME = FormatAsLink("Purple Poff", PoffConfig.poffLookup[Elements.ammonia].raw);
					public static LocString DESC = "Leaves a stinging feel in the mouth.";
				}

				public class BEACHED_AMMONIA_POFF_COOKED
				{
					public static LocString NAME = FormatAsLink("Cooked Purple Poff", PoffConfig.poffLookup[Elements.ammonia].cooked);
					public static LocString DESC = "Tastes alright, as long as you pinch your nose and don't smell it.";
				}
			}

			public class GEMS
			{
				[Note("Attached to the descriptions of each gems.")]
				public static LocString CUTTING = "Can be cut in a Gem Cutter.";

				public class AMBER_INCLUSION_BUG
				{
					public static LocString NAME = "Bug Amber Inclusion";
					public static LocString DESCRIPTION = "...";
				}

				public class AMBER_INCLUSION_HATCH
				{
					public static LocString NAME = "Hatch Amber Inclusion";
					public static LocString DESCRIPTION = "...";
				}

				public class AMBER_INCLUSION_MICRORAPTOR
				{
					public static LocString NAME = "Microraptor Amber Inclusion";
					public static LocString DESCRIPTION = "...";
				}

				public class AMBER_INCLUSION_SCORPION
				{
					public static LocString NAME = "Scorpion Amber Inclusion";
					public static LocString DESCRIPTION = "...";
				}

				public class AMBER_INCLUSION_ANCIENT_FOSSIL_FRAGMENT
				{
					public static LocString NAME = "Ancient Fossil Fragment Inclusion";
					public static LocString DESCRIPTION = "...";
				}


				public class FLAWLESS_DIAMOND
				{
					public static LocString NAME = "Flawless Diamond";
					public static LocString DESCRIPTION = "...";
				}

				public class HADEAN_ZIRCON
				{
					[Note("Hadean Zircon is a specific type of real life Zircon, from the Hadean period from 4 billion years ago.")]
					public static LocString NAME = "Hadean Zircon";
					public static LocString DESCRIPTION = "The estimated age of this gemstone is 4 billion years old.";
				}

				public class MAXIXE
				{
					[Note("Maxixe beryl is a real gemstone, but probably has little direct translations. \"Blue Beryl\" works too. https://en.wikipedia.org/wiki/Beryl")]
					public static LocString NAME = "Maxixe";
					public static LocString DESCRIPTION = "A beryl gemstone with a captivatingly dark blue shade.";
				}

				public class MOTHER_PEARL
				{
					public static LocString NAME = "Mother Pearl";
					public static LocString DESCRIPTION = "An enormous, perfectly round pearl with a pearlescent sheen.";
				}

				public class STRANGE_MATTER
				{
					[Note("https://en.wikipedia.org/wiki/Strange_matter")]
					public static LocString NAME = FormatAsLink("Strange Matter", RareGemsConfig.STRANGE_MATTER);
					public static LocString DESCRIPTION = "A droplet of strange quark matter, safely contained.";
				}
			}

			public class INDUSTRIAL_PRODUCTS
			{
				public class BEACHED_SLICKSHELLSHELL
				{
					public static LocString NAME = "Slickshell House";
					public static LocString DESC = "...";
				}
			}
		}

		public class MISC
		{
			public class BEACHED_SANDY_SEASHELL
			{
				public static LocString NAME = "Seashell";
				public static LocString DESCRIPTION = "";
			}

			public class BEACHED_SANDY_SLICKSHELL
			{
				public static LocString NAME = "Slickshell Shell";
				public static LocString DESCRIPTION = "No one's home.";
			}

			public class BEACHED_MAKIBED
			{
				public static LocString NAME = "Maki Bed";
				public static LocString DESCRIPTION = "Resting spot of a companion Maki.";
			}

			public class PLUSHIES
			{
				public static LocString PUFT = "Puft Plushie";
				public static LocString PACU = "Pacu Plushie";
				public static LocString VOLE = "Shove Vole Plushie";
			}

			public class TAGS
			{
				public static LocString BEACHED_CORAL = "Coral";
				public static LocString BEACHED_CRYSTAL = "Crystal";
				public static LocString BEACHED_MEAT = "Test (meats)";
			}
		}

		public class NAMEGEN
		{
			public class WORLD
			{
				public class ROOTS
				{
					public static LocString OCEANARIA = "Oca\nSalt\nMarin\nDeep\nBrin\nBlu\nSalin\nCoral\nAq\nPelag\nNaut\nNav\n";
				}
			}
		}

		public class RESEARCH
		{
			public class TREES
			{
				public static LocString TITLE_UNKNOWN = "Unknown";
			}
		}

		public class ROOMS
		{
			public class CRITERIA
			{
				public class NATURAL_POI
				{
					public static LocString NAME = "A natural feature";
					public static LocString DESCRIPTION = "Requires a naturally spawned Point Of Interest; such as geysers, vents, static Experiments, unique plants, geodes.";
				}
			}

			public class DETAILS
			{
				public class NATURAL_POI
				{
					public static LocString NAME = "Has natural vista";
					public static LocString DESCRIPTION = "Requires a naturally spawned Point Of Interest; such as geysers, vents, static Experiments, unique plants, geodes.";
				}
			}

			public class TYPES
			{
				public class BEACHED_NATUREVISTA
				{
					public static LocString NAME = "Nature Vista";
					public static LocString DESCRIPTION = "...";
					public static LocString EFFECT = "- Increased Motivation";
					public static LocString TOOLTIP = "A Nature Vista will inspire Duplicants to achieve greater things in life.";
				}
			}
		}

		public class SUBWORLDS
		{
			public class BAMBOO
			{
				public static LocString NAME = "Bamboo Forest";
				public static LocString DESC = "TODO";
			}

			public class BEACH
			{
				public static LocString NAME = "Beach";
				public static LocString DESC = "These sandy shoreslines are bountiful with useful resources, providing great supplies for a colony, " +
					"although the large amount of water and sand may make excavation challenging.";
				public static LocString UTILITY = "The layers of Siltstone, Salt and Sand suggest this area was once fully submerged under an ocean, " +
					"luckily for it is now mostly dry land. The Bismuth Ore deposits will serve as a valuable metal source for my colony.";
			}

			public class DEPTHS
			{
				public static LocString NAME = "Depths";
				public static LocString DESC = "An unusual darkness veils the Depths, hiding dangerous creatures and traps, and scaring Duplicants. " +
					"Illumination of the area is neccessary to explore, but there may be something of great value hidden deep within.";
				public static LocString UTILITY = "TODO";
			}

			public class FUNGALJUNGLE
			{
				public static LocString NAME = "Fungal Jungle";
				public static LocString DESC = "A bioluminescent world of mushrooms.";
				public static LocString UTILITY = "TODO";
			}

			public class SEA
			{
				public static LocString NAME = "Sea";
				public static LocString DESC = "TODO";
				public static LocString UTILITY = "TODO";
			}

			public class REEF
			{
				public static LocString NAME = "Coral Reef";
				public static LocString DESC = "TODO";
				public static LocString UTILITY = "TODO";
			}

			public class ROT
			{
				public static LocString NAME = "Rot";
				public static LocString DESC = "Strange twisting cavers lined with supple amounts of Bone, Lime and Rot. The local lifeforms appear" +
					"to have adaped to scavenging these resources.";
				public static LocString UTILITY = "TODO";
			}

			public class SNOWYBEACH
			{
				public static LocString NAME = "Snowy Beach";
				public static LocString DESC = "Once warm and inviting sandy beached, now frozen over and covered in Snow. This place seems largely inhabitable, but" +
					" there are many treasures hidden withing the Ice. I must be careful, there also seems to be a lot of ancient viruses and other danges wairing beneath" +
					" the Ice.";
				public static LocString UTILITY = "TODO";
			}
		}

		public class TRUETILES
		{
			public class TEXTUREPACKS
			{
				public class BEACHED
				{
					public static LocString NAME = "Beached";
					public static LocString DESCRIPTION = "Additional tiles for Beached elements.";
				}
			}
		}

		public class UI
		{
			public static LocString CHARACTERCONTAINER_LIFEGOAL_TRAIT = "<color=#e6d084>Life Goal: {0}</color>";

			public class BEACHED_MISC
			{
				public static LocString CAPPED = "Capped {0}";
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

		public class WORLDS
		{
			public class TINYBEACHEDSTARTCLUSTER
			{
				public static LocString NAME = "Beached Dev Test";
				public static LocString DESCRIPTION = "Small testing world. Not for gameplay.";
			}

			public class TINYBEACHEDSTART
			{
				public static LocString NAME = "Test World";
				public static LocString DESCRIPTION = "";
			}

			public class BEACHEDSTART
			{
				[Note("Based on Pelagic layer naming system of planets. Astropelagos = Ocean layer of the stars.")]
				public static LocString NAME = "Astropelagos";
				public static LocString DESCRIPTION = "";
			}
		}

		public class WORLD_TRAITS
		{
			public class DAMP
			{
				public static LocString NAME = "Damp";
				public static LocString DESCRIPTION = "This world is unusually humid, aiding plant growth and germ .";
			}

			public class ARID
			{
				public static LocString NAME = "Arid";
				public static LocString DESCRIPTION = "This world is unusually dry, plants have difficulty growing in this environment.";
			}

			public class EXTRAREEF
			{
				public static LocString NAME = "Coral Reef";
				public static LocString DESCRIPTION = "A submerged cave of corals, pearls and a plankton providing underwater geyser.";
			}

			public class SULFUROUS_CORE
			{
				public static LocString NAME = "Sulfurous Core";
				public static LocString DESCRIPTION = "This world has a core of molten Sulfur. And snails.";
			}

			public class CRYSTAL_GEODES
			{
				[Note("Caves with crystal blocks, and growing crystal clusters inside.")]
				public static LocString NAME = "Crystal Geodes";
				public static LocString DESCRIPTION = "Geodes of crystal clusters spawn in this world";
			}
		}

		public static string FormatAsLink(string text, string id = null)
		{
			text = KLEISTRINGS.UI.StripLinkFormatting(text);

			if (id.IsNullOrWhiteSpace())
			{
				id = text;
				id = id.Replace(" ", "");
			}

			id = id.ToUpperInvariant();
			id = id.Replace("_", "");

			return $"<link=\"{id}\">{text}</link>";
		}
	}
}
