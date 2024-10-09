using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.ModDb.Sicknesses;

namespace Beached
{
	public partial class STRINGS
	{
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
					public static LocString NAME = Link("Animal Handling Basics", BSkills.ANIMALHANDLING_ID);
					public static LocString DESCRIPTION = "";
				}

				public class ARCHEOLOGY
				{
					public static LocString NAME = Link("Careful Excavation", BSkills.ARCHEOLOGY_ID);
					public static LocString DESCRIPTION = "Allows a Duplicant to find hidden treasures while digging around in soft materials.";
				}

				public class ARCHEOLOGY2
				{
					public static LocString NAME = Link("Archeology", BSkills.ARCHEOLOGY2_ID);
					public static LocString DESCRIPTION = "Improves a Duplican't ability to find hidden treasures while digging around, and they can now find items in hard materials.";
				}

				public class AQUACULTURE1
				{
					public static LocString NAME = Link("Aquaculture I", BSkills.AQUACULTURE1_ID);
					public static LocString DESCRIPTION = "Better harvesting gof aquatic plants, such as corals or algaes.";
				}

				public class AQUACULTURE2
				{
					public static LocString NAME = Link("Aquaculture II", BSkills.AQUACULTURE1_ID);
					public static LocString DESCRIPTION = "Better harvesting gof aquatic plants, such as corals or algaes.";
				}

				public class MAKITRAINER1
				{
					public static LocString NAME = Link("Novice Maki Trainer", BSkills.MAKITRAINING1_ID);
					public static LocString DESCRIPTION = "Can train level 1 skills to Makis.";
				}

				public class MAKITRAINER2
				{
					public static LocString NAME = Link("Master Maki Trainer", BSkills.MAKITRAINING2_ID);
					public static LocString DESCRIPTION = "Can train level 2 skills to Makis.";
				}

				public class CRYSTALLOGRAPHY
				{
					public static LocString NAME = Link("Crystallography", BSkills.CRYSTALLOGRAPHY_ID);
					public static LocString DESCRIPTION = "Allows a Duplicant to harvest a Crystal Cluster without breaking it, allowing regrowth.";
				}

				public class GEOCHEMISTRY
				{
					public static LocString NAME = Link("Geo-Chemistry", BSkills.GEOCHEMISTRY_ID);
					public static LocString DESCRIPTION = "Enables a Duplicant to analyze crystals, allowing for synthesis of new Crystal Clusters.";
				}
			}

			public class STATS
			{
				public class BEACHED_WET
				{
					public static LocString NAME = "Wet";
					public static LocString TOOLTIP = "";
				}
			}

			public class TRAITS
			{
				public static LocString GENETIC_TRAIT = "This is a genetic trait always paired with a {0}.";

				public class BEACHED_CARNIVOROUS
				{
					public static LocString NAME = "Carnivorous";
					public static LocString DESC = "This duplicant can only digest Meats.";
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
					public static LocString DESC = "This duplicant is not very precise in dexterous with their movements and cannot to Precision Work errands.";
				}

				public class BEACHED_COMFORTSEEKER
				{
					public static LocString NAME = "Comfort Seeker";
					public static LocString DESC = "This duplicant gains a moral bonus from wearing comfortable clothing.";
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

				public class BEACHED_DEXTEROUS
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
					public static LocString NAME = Link("Plankton", PlanktonGerms.ID);
					public static LocString DESCRIPTION = Link("Tiny organisms floating in water. These microscopic " +
						"creatures are harmless to Duplicants. " +
						"They are too small to see by the naked eye, the Germ Overlay will reveal them instead.");
				}

				public class BEACHED_ICEWRATH
				{
					public static LocString NAME = Link("Ice Wraiths", IceWrathGerms.ID);
					public static LocString DESCRIPTION = Link("A type of parasitic roundworm that has been frozen away in the Permafrost for many many years. It is extremely resilient to cold temperatures and has been observed to survive being frozen at near absolute zero temperatures.");
				}

				public class BEACHED_LIMPETEGG
				{
					public static LocString NAME = Link("Limpet Eggs", LimpetEggGerms.ID);
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
					public static LocString NAME = "NA"; //KUI.FormatAsLink("Limpets", LimpetsSickness.ID);
				}

				public class BEACHED_SICKNESS_CAPPED
				{
					public static LocString NAME = Link("Capped", CappedSickness.ID);
				}

				public class BEACHED_SICKNESS_POFFMOUTH
				{
					public static LocString NAME = Link("Poffmouth", PoffSporeGerms.ID);
				}

				public class BEACHED_SICKNESS_TELEPORTITS
				{
					public static LocString NAME = Link("Teleportitis", "");
				}

				public static LocString LIGHTEXPOSURE = "Exposed to Light. Approximately {0} change per second.";
			}
		}
	}
}
