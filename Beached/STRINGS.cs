using System.Diagnostics.CodeAnalysis;
using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Entities.Critters;
using Beached.Content.Defs.Entities.Plants;
using Beached.Content.Defs.Items.Foods;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.ModDb.Sicknesses;
using JetBrains.Annotations;
using Microsoft.SqlServer.Server;
using KLEISTRINGS = STRINGS;

namespace Beached
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class STRINGS
    {
        public class BUILDCATEGORIES
        {
            public class BEACHED_POIS
            {
                public static LocString NAME = "Beache POIs";
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
                    public static LocString NAME = "Mini-Fridge";
                    public static LocString DESC = "A tiny fridge to store a tiny bit of food for the tiny dupes.";
                    public static LocString EFFECT = "TRANSLATION NOT NEEDED - gets copied from regular fridge";
                }

                public class BEACHED_MOSSBED
                {
                    // a bed for moss. it's a wooden frame moss grows on.
                    public static LocString NAME = "Moss Bed";
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

                public class BEACHED_SEASHELLCHIME
                {
                    public static LocString NAME = "Seashell Chime";
                    public static LocString DESC = "Pretty sea shells suspended in air, creating music.";
                    public static LocString EFFECT = "Emits a soothing sound when stimulated by changing air pressure, decreasing Stress of nearby Duplicants.";
                }

                public class BEACHED_SALTLICK
                {
                    // visible in UI or codex 
                    public static LocString NAME = "Critter Lick";
                    // the name that appears on the actual building. ie. Salt-Lick, or Sulfur-Lick
                    public static LocString FORMATTED_NAME = "{Element}-Lick";
                    public static LocString DESC = "A block of lickable material. Delicious!";
                    public static LocString EFFECT = "Allows critters to consume additional materials, boosting production. \n" +
                                                     $"{FormatAsLink("Slickshells", SlickShellConfig.ID)} can be fully sustained on appropiate licks.\n\n" +
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
                    public static LocString NAME = "Wood Carving";
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
                    }
                }
            }

            public class STATUSITEMS
            {
                public class BEACHED_LUBRICATED
                {
                    public static LocString NAME = "Lubricated";

                    public static LocString TOOLTIP =
                        "This machine has been lubricated with Mucus, allowing it to perform better than ever.";
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

        public class COMETS
        {
            public class SHRAPNEL
            {
                public static LocString NAME = "Shrapnel";
                public static LocString DESC = "Small piece of a metal violently flung from an explosion.";
            }
        }

        public class CORALS
        {
            public class BEACHED_LEAFLETCORAL
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

                public class CELLALGAE
                {
                    public static LocString NAME = FormatAsLink("Cell", CellAlgaeConfig.ID);
                    public static LocString DESCRIPTION = "...";
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
                    public static LocString NAME = "StackablePlant"; //Clickety Clack? Clack Cane?
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
                public static LocString GENETIC_TRAIT = "This is a genetic trait always paired with a {0}.";

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
                public static LocString NAME = FormatAsLink("Mucus");
                public static LocString DESC = "TODO";
            }

            public class MUCUSFROZEN
            {
                public static LocString NAME = FormatAsLink("Frozen Mucus", Elements.mucusFrozen.ToString());
                public static LocString DESC = "TODO";
            }

            public class MURKYBRINE
            {
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

            public class RUBBER
            {
                public static LocString NAME = FormatAsLink("Rubber");
                public static LocString DESC = "TODO";
            }

            public class SALTYOXYGEN
            {
                public static LocString NAME = FormatAsLink("Salty Oxygen", Elements.saltyOxygen.ToString());
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
                public static LocString NAME = FormatAsLink("Sulfurous Ice", Elements.sulfurousIce.ToString());
                public static LocString DESC = "TODO";
            }

            public class SULFUROUS_WATER
            {
                public static LocString NAME = FormatAsLink("Sulfurous Water", Elements.sulfurousWater.ToString());
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

                public class BEACHED_EQUIPMENT_MAXIXEPENDANT
                {
                    public static LocString NAME = "Maxixe Pendant";
                    public static LocString GENERICNAME = "Maxixe Pendant";
                    public static LocString DESCRIPTION = "...";
                }

                public class BEACHED_EQUIPMENT_PEARLNECKLACE
                {
                    public static LocString NAME = "Pearl Necklace";
                    public static LocString GENERICNAME = "Pearl Necklace";
                    public static LocString DESCRIPTION = "This pearlescent shine would swoon anyone!";
                }

                public class BEACHED_EQUIPMENT_HADEANZIRCONAMULET
                {
                    public static LocString NAME = "Hadean Zircon Amulet";
                    public static LocString GENERICNAME = "Hadean Zircon Amulet";
                    public static LocString DESCRIPTION = "The bright red sparkle of this gem reminds Duplicants of the importance of working hard.";
                }

                public class BEACHED_EQUIPMENT_HEMATITENECKLACE
                {
                    public static LocString NAME = "Hematite Necklace";
                    public static LocString GENERICNAME = "Hematite Necklace";
                    public static LocString DESCRIPTION = "Heavy and stylish.";
                }

                public class BEACHED_EQUIPMENT_ZEOLITEPENDANT
                {
                    public static LocString NAME = "Zeolite Pendant";
                    public static LocString GENERICNAME = "Zeolite Pendant";
                    public static LocString DESCRIPTION = "This gemstone has a particularly soothing color. When duplicants look at it, they feel at ease.";
                }

                public class BEACHED_EQUIPMENT_STRANGEMATTERAMULET
                {
                    public static LocString NAME = "Strange Amulet";
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
                public class BEACHED_ASPIC_LICE
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

                public class BEACHED_BERRY_JELLY
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

                public class BEACHED_CRABCAKE
                {
                    public static LocString NAME = "Crabcake";
                    public static LocString DESC = "...";
                }

                public class BEACHED_LEGENDARY_STEAK
                {
                    public static LocString NAME = "Legendary Steak";
                    public static LocString DESC = "It is so rare, it has been classified as legendary! A truly wonderful cut of meat that melts in the mouth.";
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
                    public static LocString NAME = FormatAsLink("Mussel Tongue", TongueConfig.ID);
                    public static LocString DESC = "Edible tongue of a Mussel Sprout. It has a somewhat bitter, green flavour, and the consistency of fresh clam.";
                }

                public class BEACHED_GLAZEDDEWNUT
                {
                    public static LocString NAME = "Glazed Dewnut";
                    public static LocString DESC = "...";
                }

                public class BEACHED_SMOKEDMEAT
                {
                    public static LocString NAME = FormatAsLink("Smoked Meat", SmokedMeatConfig.ID);
                    public static LocString DESC = "Meat imbued with the wonderful aroma of smoke.";
                }

                public class BEACHED_SMOKEDFISH
                {
                    public static LocString NAME = "Smoked Fish";
                    public static LocString DESC = "...";
                }

                public class BEACHED_SMOKEDMEALLICE
                {
                    public static LocString NAME = "Smoked Lice";
                    public static LocString DESC = "...";
                }

                public class BEACHED_SMOKEDPLANTMEAT
                {
                    public static LocString NAME = "Smoked PLant Meat";
                    public static LocString DESC = "...";
                }

                public class BEACHED_SMOKED_SNAIL
                {
                    public static LocString NAME = "Smoked Snail";
                    public static LocString DESC = "...";
                }

                public class BEACHED_SMOKED_TOFU
                {
                    public static LocString NAME = "Smoked Tofu";
                    public static LocString DESC = "...";
                }
            }

            public class GEMS
            {
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

                public class FLAWLESS_DIAMOND
                {
                    public static LocString NAME = "Flawless Diamond";
                    public static LocString DESCRIPTION = "...";
                }

                public class HADEAN_ZIRCON
                {
                    public static LocString NAME = "Hadean Zircon";
                    public static LocString DESCRIPTION = "...";
                }

                public class MAXIXE
                {
                    public static LocString NAME = "Maxixe";
                    public static LocString DESCRIPTION = "...";
                }

                public class MOTHER_PEARL
                {
                    public static LocString NAME = "Mother Pearl";
                    public static LocString DESCRIPTION = "An enormous, perfectly round pearl with a pearlescent sheen.";
                }

                public class STRANGE_MATTER
                {
                    public static LocString NAME = "Strange Matter";
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
                public static LocString NAME = "Astropelagos"; // Nautica?
                public static LocString DESCRIPTION = "";
            }
        }

        public class WORLD_TRAITS
        {
            public class EXTRAREEF
            {
                public static LocString NAME = "Coral Reef";
                public static LocString DESCRIPTION = "";
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
