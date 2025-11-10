using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.ModDb.Sicknesses;

namespace Beached
{
	public partial class STRINGS
	{
		public class DUPLICANTS
		{
			public class SKILLGROUPS
			{
				public class BEACHED_SKILLGROUP_PRECISION
				{
					public static LocString NAME = "Precision";
				}
			}

			public class STATUSITEMS
			{
				public class BEACHED_THAWING
				{
					public static LocString NAME = "Thawing";
					public static LocString TOOLTIP = "This Duplicant is melting some Ice.";
				}

				public class BEACHED_ICEWRATHLASHOUT
				{
					public static LocString NAME = "Lashing Out";
					public static LocString TOOLTIP = $"This Duplicant is agitated by their {Link("Ice Wraiths.", IceWrathSickness.ID)}";
				}

				public class BEACHED_SIREN
				{
					public static LocString NAME = "Siren";
					public static LocString TOOLTIP = "This Duplicant became very scary since stressed out.";
				}
			}

			public class ATTRIBUTES
			{
				public class BEACHED_PRECISION
				{
					public static LocString NAME = "Precision";
					public static LocString DESC = "Determines the hand skill of a duplicant, affecting how much material they can retrieve when digging, as well as completing hihg precision requiring tasks.";
					public static LocString SPEEDMODIFIER = "{0} Crafting Speed";
					public static LocString AMOUNTMODIFIER = "{0} Material Recovered";
				}


				public class BEACHED_HEATRESISTANCE
				{
					public static LocString NAME = "Heat Tolerance";
					public static LocString DESC = "This Duplicant can take heat a little better.";
				}

				// Building attributes
				// The game expects them under DUPLICANT
				public class BEACHED_BUILDING_ACIDVULNERABILITY
				{
					public static LocString NAME = "Acid Vulnerability";
					public static LocString DESC = "...";
				}

				public class BEACHED_BUILDING_OPERATINGSPEED
				{
					public static LocString NAME = "Operating Speed";
					public static LocString DESC = "...";
				}

				public class BEACHED_BUILDING_DOOROPENINGSPEED
				{
					public static LocString NAME = "Door Opening Speed";
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
					public static LocString NAME = "Minnow";
					public static LocString DESC = "Minnow's printing label actually says \"Experiment MRM8\", but she prefers to go by Minnow.";
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
					public static LocString NAME = Link("Animal Handling Basics", BSkills.ANIMALHANDLING_ID);
					public static LocString DESCRIPTION = "";
					public static LocString CAN_MOVE_CRITTERS = "Move Critters";
				}

				public class ARCHEOLOGY
				{
					public static LocString NAME = Link("Careful Excavation", BSkills.ARCHEOLOGY_ID);
					public static LocString DESCRIPTION = "Find hidden treasures while digging around in naturally spawned materials.";
					public static LocString CAN_FIND_TREASURES = "Treasure Hunting";
				}

				public class ARCHEOLOGY2
				{
					public static LocString NAME = Link("Archeology", BSkills.ARCHEOLOGY2_ID);
					public static LocString DESCRIPTION = "Twice as likely to find hidden treasures when digging up materials.";
					public static LocString CAN_FIND_MORE_TREASURES = "Better Treasure Hunting";
				}

				public class AQUACULTURE1
				{
					public static LocString NAME = Link("Aquaculture I", BSkills.AQUACULTURE1_ID);
					public static LocString DESCRIPTION = "Better harvesting of aquatic plants";
				}

				public class AQUACULTURE2
				{
					public static LocString NAME = Link("Aquaculture II", BSkills.AQUACULTURE1_ID);
					public static LocString DESCRIPTION = "Better harvesting of aquatic plants,";
				}

				public class MAKITRAINER1
				{
					public static LocString NAME = Link("Trainer I", BSkills.MAKITRAINING1_ID);
					public static LocString DESCRIPTION = "Can train Muffins at a Muffin Training Platform.";
				}

				public class MAKITRAINER2
				{
					public static LocString NAME = Link("Trainer II", BSkills.MAKITRAINING2_ID);
					public static LocString DESCRIPTION = "Can train level 2 skills to Makis.";
				}

				public class CRYSTALLOGRAPHY
				{
					public static LocString NAME = Link("Crystallography", BSkills.CRYSTALLOGRAPHY_ID);
					public static LocString DESCRIPTION = "Allows a Duplicant to harvest a Crystal Cluster without breaking it.";
					public static LocString CAN_CUT_GEMS = $"Usage of {Link("Gem Cutter", GemCutterConfig.ID)}.";
					public static LocString HARVEST_CLUSTERS = "Cluster Harvesting.";
				}

				public class GEOCHEMISTRY
				{
					public static LocString NAME = Link("Geo-Chemistry", BSkills.GEOCHEMISTRY_ID);
					public static LocString DESCRIPTION = "Enables a Duplicant to analyze crystals, allowing for synthesis of new Crystal Clusters.";
					public static LocString CAN_SYNTHETIZE_CLUSTERS = $"Usage of  {Link("Crystal Synthetizer", CrystalSynthetizerConfig.ID)}.";
				}
			}

			public class TRAITS
			{
				public static LocString GENETIC_TRAIT = "This is a genetic trait always paired with a {0}.";

				public class LIFE_GOALS
				{
					public class BEACHED_MINNOW
					{
						public static LocString NAME = "Surfin' and Snoozin'";
						public static LocString DESCRIPTION = $"This duplicant cannot stop talking about how cool it would be to have a {Link("Mechanical Surfboard", MechanicalSurfboardConfig.ID)} in their bedroom.\n" +
							$"\n<b>Objective:</b> Have a functional {Link("Mechanical Surfboard", MechanicalSurfboardConfig.ID)} placed in the same room the Duplicant's assigned bed is in.";
					}

					public class FASHION_IDOL
					{
						public static LocString NAME = "Fashion Idol";
						public static LocString DESCRIPTION = "This duplicant's dream is to be as slick as it can get. \n\n<b>Objective:</b> Achieve 50 additional decor with equipment.";
					}

					public class GOLDEN_LAVATORY
					{
						public static LocString NAME = "Golden Lavatory";
						public static LocString DESCRIPTION = $"This duplicant wishes to own their very own golden {Link("Lavatory", FlushToiletConfig.ID)} assigned. \n\n<b>Objective:</b> Assign a personal lavatory made of {Link("Gold Amalgam", SimHashes.GoldAmalgam)} Amalgam or {Link("Gold", SimHashes.Gold)} to this Duplicant.";
					}

					public class MAXIXE_PENDANT
					{
						public static LocString DESCRIPTION = "This duplicant really wishes to express themselves by wearing a Maxixe Pendant.";
					}

					public class PEARL_PENDANT
					{
						public static LocString DESCRIPTION = "This duplicant would love to don a Pearl Necklace.";
					}

					public class STRANGE_MATTER_PENDANT
					{
						public static LocString DESCRIPTION = "This duplicant has a deep desire the wear a Strange Matter Amulet.";
					}
				}

				public class STARTWITHBEACHED_BOOSTER_PRECISION1
				{
					public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BEACHED_BOOSTER_PRECISION1.NAME;
					public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BEACHED_BOOSTER_PRECISION1.DESC;
					public static LocString SHORT_DESC = $"Starts with a preinstalled <b>{(string)ITEMS.BIONIC_BOOSTERS.BEACHED_BOOSTER_PRECISION1.NAME}</b>";
					public static LocString SHORT_DESC_TOOLTIP = global::STRINGS.DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
				}

				public class STARTWITHBEACHED_BOOSTER_PRECISION2
				{
					public static LocString NAME = ITEMS.BIONIC_BOOSTERS.BEACHED_BOOSTER_PRECISION2.NAME;
					public static LocString DESC = ITEMS.BIONIC_BOOSTERS.BEACHED_BOOSTER_PRECISION2.DESC;
					public static LocString SHORT_DESC = $"Starts with a preinstalled <b>{(string)ITEMS.BIONIC_BOOSTERS.BEACHED_BOOSTER_PRECISION2.NAME}</b>";
					public static LocString SHORT_DESC_TOOLTIP = global::STRINGS.DUPLICANTS.TRAITS.STARTING_BIONIC_BOOSTER_SHARED_DESC_TOOLTIP;
				}

				public class BEACHED_CARNIVOROUS
				{
					public static LocString NAME = "Carnivorous";
					public static LocString DESC = "This duplicant can only digest Meats.";
				}

				public class BEACHED_HOTBLOODED
				{
					public static LocString NAME = "Hot Blooded";
					public static LocString DESC = "This duplicant runs hot, they can withstand heat a little better than others.";
				}

				public class BEACHED_VEGETARIAN
				{
					public static LocString NAME = "Vegetarian";
					public static LocString DESC = "This duplicant majorly stresses out if forced to eat any Meat and will only do so if there are no other options available.";
				}

				public class BEACHED_GILLS
				{
					public static LocString NAME = "Gills";
					public static LocString SHORT_DESC = "Amphibious";
					public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can also breath in any kind of Water. Does not grant immunity to wet debuffs.";
					public static LocString DESC = "This duplicant can live like a fish in the water... or at least breath like one.";
					public static LocString WATERBREATHING = "• Amphibious";
				}

				public class BEACHED_CLUMSY
				{
					public static LocString NAME = "Clumsy";
					public static LocString DESC = "This duplicant is not very precise with their movements and cannot to Precision Work errands.";
				}

				public class BEACHED_COMFORTSEEKER
				{
					public static LocString NAME = "Comfort Seeker";
					public static LocString DESC = "This duplicant gains a moral bonus from wearing comfortable clothing.";
				}

				public class BEACHED_HOPEFUL
				{
					public static LocString NAME = "Hopeful";
					public static LocString DESC = "Knowing there are such gentle giant creatures out there, this Duplicant is filled with hope.";
				}

				public class BEACHED_PLUSHIEMAKER
				{
					public static LocString NAME = "Plushie Gifter";
					public static LocString DESC = "...";
				}

				public class BEACHED_SIREN
				{
					public static LocString NAME = "Scary Scales";
					public static LocString DESC = "...";
				}

				public class BEACHED_THALASSOPHILE
				{
					public static LocString NAME = "Thalassophile";
					public static LocString DESC = "Gains bonus while in any Sea, Beach of Ocean type biome.";
					public static LocString DESC_EXTENDED = "<b>Effects while active:</b>";
				}

				public class BEACHED_DEXTEROUS
				{
					public static LocString NAME = "Dexterous";
					public static LocString DESC = "This duplicant is extremely skilled with their hands.";
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
					public static LocString DESC = "Unfortunately this duplicant has a severe reaction when consuming anything that comes from the Sea.";
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
					public static LocString NAME = Link("Plankton", PlanktonGerms.ID);
					public static LocString DESC = "Tiny organisms floating in water. These microscopic " +
						"creatures are harmless to Duplicants. " +
						"They are too small to see by the naked eye, the Germ Overlay will reveal them instead.";
				}

				public class BEACHED_ICEWRATH
				{
					public static LocString NAME = Link("Ice Wraiths", IceWrathGerms.ID);
					public static LocString DESC = "A type of parasitic roundworm that has been frozen away in the Permafrost for many many years. It is extremely resilient to cold temperatures and has been observed to survive being frozen at near absolute zero temperatures.";
				}

				public class BEACHED_SICKNESS_ICEWRATH
				{
					public static LocString NAME = Link("Ice Wrath", IceWrathSickness.ID);
				}

				public class BEACHED_SICKNESS_LIMPETS_DUPLICANT
				{
					public static LocString NAME = Link("Limpets", LimpetsSickness.ID);
				}

				public class BEACHED_LIMPETEGG
				{
					public static LocString NAME = Link("Limpet Eggs", LimpetEggGerms.ID);
					public static LocString DESC = $"Very small eggs of a Limpet colony. Limpets can latch onto various shells and carapaces, and the adult limpets will form shells of their surrounding material around themselves. When infecting a critters, these materials can be shorn off later at a {Link("Shearing Station", ShearingStationConfig.ID)}.\n" +
						$"\n" +
						$"When a Duplicant is infected, no useful material is produced, symptoms will include increased hunger and case of unsightlyness. The affliction can be cured at a {Link("Shearing Station", ShearingStationConfig.ID)} which duplicants will visit on their own as long as the building has \"Duplicant Shaving\" enabled.";
				}

				public class BEACHED_CAPSPORE
				{
					public static LocString NAME = Link("Cap-Cap Spores", CapSporeGerms.ID);
				}

				public class BEACHED_POFFSPORE
				{
					public static LocString NAME = Link("Poffshroom Spores", PoffSporeGerms.ID);
				}

				public class BEACHED_LIMPETS_DUPLICANT
				{
					public static LocString NAME = Link("Limpets", LimpetsSickness.ID);
				}

				public class BEACHED_SICKNESS_CAPPED
				{
					public static LocString NAME = Link("Capped", CappedSickness.ID);
				}

				public class BEACHED_SICKNESS_POFFMOUTH
				{
					public static LocString NAME = Link("Poffmouth", PoffSporeGerms.ID);
				}

				public static LocString LIGHTEXPOSURE = "Exposed to Light. Approximately {0} change per second.";
			}
		}
	}
}
