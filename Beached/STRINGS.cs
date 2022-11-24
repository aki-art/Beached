using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.ModDb.Sicknesses;

namespace Beached
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class BEACHED_AMMONIAGENERATOR
                {
                    public static LocString NAME = FormatAsLink("Ammonia Generator", AmmoniaGeneratorConfig.ID);
                    public static LocString DESC = "...";
                    public static LocString EFFECTS = global::STRINGS.BUILDINGS.PREFABS.HYDROGENGENERATOR.EFFECT;
                }

                public class BEACHED_SEASHELLCHIME
                {
                    public static LocString NAME = "Seashell Chime";
                    public static LocString DESC = "Pretty sea shells suspended in air, creating music.";
                    public static LocString EFFECTS = "Emits a soothing sound when stimulated by changing air pressure, decreasing Stress of nearby Duplicants.";
                }

                public class BEACHED_MINIFRIDGE
                {
                    public static LocString NAME = "Mini-Fridge";
                    public static LocString DESC = "A tiny fridge to store a tiny bit of food for the tiny dupes.";
                    public static LocString EFFECTS = global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.EFFECT;
                }

                public class BEACHED_BAMBOOPLATFORM
                {
                    public static LocString NAME = "Bamboo Walkway";
                    public static LocString DESC = "...";
                    public static LocString EFFECTS = "...";
                }

                public class BEACHED_MAKITRAININGGROUND
                {
                    public static LocString NAME = "Maki Training Ground";
                    public static LocString DESC = "...";
                    public static LocString EFFECTS = "...";
                }

                public class BEACHED_MAKIHUT
                {
                    public static LocString NAME = "Maki Hut";
                    public static LocString DESC = "...";
                    public static LocString EFFECTS = "...";
                }

                public class BEACHED_BLADDERTREETAP
                {
                    public static LocString NAME = "Tree Tap";
                    public static LocString DESC = "...";
                    public static LocString EFFECTS = "...";
                }
            }
        }

        public class CORALS
        {
            public class SINGLE_CELL
            {
                public static LocString NAME = FormatAsLink("Cell", "SingleCellConfig.ID");
                public static LocString DESCRIPTION = "\n" +
                    "\n" +
                    "Filters Polluted Water, producing Water and Slime.";
            }

            public class BEACHED_LEAFLET_CORAL
            {
                public static LocString NAME = "Leaflet Coral";
                public static LocString DESCRIPTION = "A coral with green capitulum resembling cabbage leaves. This coral consumes Hydrogen directly from fresh Water, releasing the excess Oxygen in the process.\n" +
                    "\n" +
                    "Inefficiently converts Water into Oxygen.";
            }

            public class BEACHED_WOSHU_CORAL
            {
                public static LocString NAME = "Woshu Sponge";
                public static LocString DESCRIPTION = "Woshu Sponge can sustain itself by consuming germs. This happens to be very useful for those wishing to keep their liquid reservoirs clean.\n" +
                    "\n" +
                    "Removes germs from liquids.";
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

            public class STATS
            {
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
                        public static LocString DESC = $"A geyser that periodically erupts with {FormatAsLink("Murky Brine", Elements.MurkyBrine.ToString())}.";
                    }

                    public class BEACHED_BISMUTHVOLCANO
                    {
                        public static LocString NAME = "Bismuth Volcano";
                        public static LocString DESC = $"...";
                    }

                    public class BEACHED_AMMONIAVENT
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
                        public static LocString DESC = "A large volcano that periodically erupts with " + "NA"; //KUI.FormatAsLink("molten Bismuth", BElements.MoltenBismuth.ToString().ToUpperInvariant()) + ".";
                    }

                    public class PACU_GEYSER
                    {
                        public static LocString NAME = "Pacu Geyser";
                        public static LocString DESC = $"A geyser that periodically erupts with" +
                            $" {FormatAsLink("Polluted Water", SimHashes.DirtyWater.ToString())} rich in " +
                            $"{FormatAsLink("Pacus", PacuConfig.ID)}.";
                    }
                }

                public class SEEDS
                {
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
                }

                public class BEACHED_SLICKSHELL
                {
                    public static LocString NAME = "Slickshell";
                    public static LocString DESC = "...";
                    public static LocString BABY_NAME = "Slick Shellitle";
                    public static LocString BABY_DESC = "...";
                    public static LocString EGG_NAME = "Slickshell Egg";
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
            }

            public class FAMILY
            {
                public static LocString BEACHEDSLICKSHELL = "Slickshell";
                public static LocString BEACHEDANGULARFISH = "Angular Fish";
                public static LocString BEACHEDMAKI = "Maki";
            }

            public class FAMILY_PLURAL
            {
                public static LocString BEACHEDSNAILSPECIES = "Slickshells";
                public static LocString BEACHEDANGULARFISHSPECIES = "Angular Fish";
                public static LocString BEACHEDMAKISPECIES = "Makis";
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
                public class MINNOW
                {
                    public static LocString NAME = "Minnow";
                    public static LocString DESC = "Minnow's printing label actually says \"Experiment MRM8\", but she prefers to go by Minnow.";
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
                public class BEACHED_GILLS
                {
                    public static LocString NAME = "Gills";
                    public static LocString SHORT_DESC = "Amphibious";
                    public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can also breath in any kind of Water. Does not grant immunity to wet debuffs.";
                    public static LocString DESC = "This duplicant can live like a fish in the water... or at least breath like one.";
                    public static LocString WATERBREATHING = "• Amphibious";
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
            }

            public class DISEASES
            {
                public class BEACHED_PLANKTON
                {
                    public static LocString NAME = FormatAsLink("Plankton", PlanktonGerms.ID);
                }

                public class BEACHED_LIMPETEGG
                {
                    public static LocString NAME = FormatAsLink("Limpet Eggs", LimpetEggGerms.ID);
                }

                public class BEACHED_CAPSPORE
                {
                    public static LocString NAME = FormatAsLink("Grimcap Spores", CapSporeGerms.ID);
                }

                public class BEACHED_LIMPETS_DUPLICANT
                {
                    public static LocString NAME = "NA"; //KUI.FormatAsLink("Limpets", LimpetsSickness.ID);
                }

                public class BEACHED_SICKNESS_CAPPED
                {
                    public static LocString NAME = FormatAsLink("Capped", CappedSickness.ID);
                }

                public static LocString LIGHTEXPOSURE = "Exposed to Light. Approximately {0} change per second.";
            }
        }

        public class EFFECTS
        {
            public class BEACHED_OCEANBREEZE
            {
                public static LocString NAME = "Ocean Breeze";

                public static LocString DESC = "...";
            }

            public class BEACHED_LIMPETHOST
            {
                public static LocString NAME = "Limpets";

                public static LocString DESC = "This critter is being overgrown by Limpets. \n" +
                        "Once fully grown, they can be sheared off for resources.";
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
                public static LocString NAME = FormatAsLink("Frozen Ammonia", Elements.AmmoniaFrozen.ToString());
                public static LocString DESC = "TODO";
            }

            public class AMMONIALIQUID
            {
                public static LocString NAME = FormatAsLink("Liquid Ammonia", Elements.AmmoniaLiquid.ToString());
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
                public static LocString NAME = FormatAsLink("Beryllium Gas", Elements.BerylliumGas.ToString());
                public static LocString DESC = "TODO";
            }

            public class BERYLLIUMMOLTEN
            {
                public static LocString NAME = FormatAsLink("Molten Beryllium", Elements.BerylliumMolten.ToString());
                public static LocString DESC = "TODO";
            }

            public class BISMUTH
            {
                public static LocString NAME = FormatAsLink("Bismuth");
                public static LocString DESC = "TODO";
            }

            public class BISMUTHGAS
            {
                public static LocString NAME = FormatAsLink("Bismuth Gas", Elements.BismuthGas.ToString());
                public static LocString DESC = "TODO";
            }

            public class BISMUTHMOLTEN
            {
                public static LocString NAME = FormatAsLink("Molten Bismuth", Elements.BismuthMolten.ToString());
                public static LocString DESC = "TODO";
            }

            public class BISMUTHORE
            {
                public static LocString NAME = FormatAsLink("Bismuth Ore", Elements.BismuthOre.ToString());
                public static LocString DESC = "TODO";
            }

            public class BONE
            {
                public static LocString NAME = FormatAsLink("Bone");
                public static LocString DESC = "TODO";
            }

            public class CALCIUM
            {
                public static LocString NAME = FormatAsLink("Calcium", Elements.Calcium.ToString());
                public static LocString DESC = "TODO";
            }

            public class CALCIUMGAS
            {
                public static LocString NAME = FormatAsLink("Calcium Gas", Elements.CalciumGas.ToString());
                public static LocString DESC = "TODO";
            }

            public class CALCIUMMOLTEN
            {
                public static LocString NAME = FormatAsLink("Molten Calcium", Elements.CalciumMolten.ToString());
                public static LocString DESC = "TODO";
            }

            public class GRAVEL
            {
                public static LocString NAME = FormatAsLink("Gravel");
                public static LocString DESC = "TODO";
            }

            public class HEULANDITE
            {
                public static LocString NAME = FormatAsLink("Zeolite");
                public static LocString DESC = "TODO";
            }

            public class MOSS
            {
                public static LocString NAME = FormatAsLink("Moss");
                public static LocString DESC = "TODO";
            }

            public class MUCUS
            {
                public static LocString NAME = FormatAsLink("Mucus");
                public static LocString DESC = "TODO";
            }

            public class MUCUSFROZEN
            {
                public static LocString NAME = FormatAsLink("Frozen Mucus", Elements.MucusFrozen.ToString());
                public static LocString DESC = "TODO";
            }

            public class MURKYBRINE
            {
                public static LocString NAME = FormatAsLink("Murky Brine", Elements.MurkyBrine.ToString());
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
                public static LocString NAME = FormatAsLink("Pearl");
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


            public class SALTYOXYGEN
            {
                public static LocString NAME = FormatAsLink("Salty Oxygen", Elements.SaltyOxygen.ToString());
                public static LocString DESC = "TODO";
            }

            public class SALTYOXYGENFROZEN
            {
                public static LocString NAME = FormatAsLink("Frozen Salty Oxygen");
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
                public static LocString NAME = FormatAsLink("Sulfurous Ice", Elements.SulfurousIce.ToString());
                public static LocString DESC = "TODO";
            }

            public class SULFUROUS_WATER
            {
                public static LocString NAME = FormatAsLink("Sulfurous Water", Elements.SulfurousWater.ToString());
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
                public static LocString NAME = FormatAsLink("Zinc Gas", Elements.ZincGas.ToString());
                public static LocString DESC = "TODO";
            }

            public class ZINCMOLTEN
            {
                public static LocString NAME = FormatAsLink("Molten Zinc", Elements.ZincMolten.ToString());
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
                public static LocString NAME = FormatAsLink("Zirconium Gas", Elements.ZirconiumGas.ToString());
                public static LocString DESC = "TODO";
            }

            public class ZIRCONIUMMOLTEN
            {
                public static LocString NAME = FormatAsLink("Molten Zirconium", Elements.ZirconiumMolten.ToString());
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
                public class BEACHED_BLUEPRINT
                {
                    public static LocString NAME = "Blueprint: {0}";
                    public static LocString DESC = "...";
                }
            }

            public class FOOD
            {
                public class NUTTYDELIGHT
                {
                    public static LocString NAME = "Nutty Delight";
                    public static LocString DESC = "...";
                }

                public class STUFFEDSNAIL
                {
                    public static LocString NAME = "Stuffed Snail";
                    public static LocString DESC = "...";
                }

                public class RAWSNAIL
                {
                    public static LocString NAME = "Raw Snail";
                    public static LocString DESC = "...";
                }

                public class CRABMEAT
                {
                    public static LocString NAME = "Crab Meat";
                    public static LocString DESC = "...";
                }

                public class CRABCAKE
                {
                    public static LocString NAME = "Crabcake";
                    public static LocString DESC = "...";
                }

                public class LIMPET
                {
                    public static LocString NAME = "Limpet";
                    public static LocString DESC = "...";
                }
                public class TONGUE
                {
                    public static LocString NAME = "Tongue";
                    public static LocString DESC = "Edible tongue of a Mussel Sprout. Best eaten raw.";
                }

                public class GLAZEDDEWNUT
                {
                    public static LocString NAME = "Glazed Dewnut";
                    public static LocString DESC = "...";
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
            public class BEACHED_MAKIBED
            {
                public static LocString NAME = "Maki Bed";
                public static LocString DESCRIPTION = "Resting spot of a companion Maki.";
            }

            public class TAGS
            {
                public static LocString BEACHED_CORAL = "Coral";
                public static LocString BEACHED_CRYSTAL = "Crystal";
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

        public static string FormatAsLink(string text, string id = null)
        {
            text = global::STRINGS.UI.StripLinkFormatting(text);

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
