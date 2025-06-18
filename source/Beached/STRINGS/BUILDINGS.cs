using Beached.Content;
using Beached.Content.Defs.Buildings;
using FUtility.FLocalization;
using KLEISTRINGS = STRINGS;

namespace Beached
{
	public partial class STRINGS
	{
		public static string Link(string text, SimHashes element) => Link(text, element.CreateTag().ToString());

		public static string Link(string text, string id = null)
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

		public class BUILDINGS
		{
			public class ATTRIBUTES
			{
				public class BEACHED_BUILDING_OPERATINGSPEED
				{
					public static LocString NAME = "Operating Speed";
				}

				public class BEACHED_BUILDING_DOOROPENINGSPEED
				{
					public static LocString NAME = "Door Opening Speed";
				}
			}

			public class PREFABS
			{
				public class BEACHED_AMMONIAGENERATOR
				{
					[Note("Generates power from ammonia")]
					public static LocString NAME = Link("Ammonia Generator", AmmoniaGeneratorConfig.ID);
					public static LocString DESC = "Ammonia generators are not very efficient, and emit a lot of waste materials.";
					public static LocString EFFECT = $"Converts {Link("Ammonia", "BEACHEDAMMONIA")} into electrical {Link("Power", "POWER")}, {Link("Water", "WATER")} and {Link("Nitrogen", "BEACHEDNITROGEN")}.";
				}

				public class BEACHED_AQUATICFARMTILE
				{
					public static LocString NAME = Link("Aquatic Farm Tile", AquaticFarmTileConfig.ID);
					public static LocString DESC = "Duplicants can deliver fertilizer to farm aquatic tiles, accelerating plant growth. Required liwuids will be soaked up by the tile as needed.";
					public static LocString EFFECT = $"Grows one {Link("Plant", "PLANTS")} from a {Link("Seed", "PLANTS")}.\n" +
						$"\n" +
						$"Can be used as floor tile and rotated before construction.";
				}

				public class BEACHED_ATMOSPHERICFORCEFIELDGENERATOR
				{
					public static LocString NAME = Link("Atmospheric Forcefield Generator", ForceFieldGeneratorConfig.ID);
					public static LocString DESC = "The air feels heavy and electrifying near this machine.";
					public static LocString EFFECT = "Shields the surface of an asteroid from incoming Meteors. Does not allow rockets to pass.";
				}

				public class BEACHED_BIOFUELGENERATOR
				{
					public static LocString NAME = Link("Bio-Fuel Generator", BioFuelGeneratorConfig.ID);
					public static LocString DESC = "Generates a small amount of power by burning Bio-Fuel.";
					public static LocString EFFECT = $"Burns {Link("Bio-Fuel", Elements.bioFuel)}, producing electrical {Link("Power", "POWER")} and {Link("Nitrogen", "BEACHEDNITROGEN")}.";
				}

				public class BEACHED_BIOFUELMAKER
				{
					public static LocString NAME = Link("Bio-Fuel Reactor", BioFuelMakerConfig.ID);
					public static LocString DESC = "Processes organic material into Bio-Fuel.";
					public static LocString EFFECT = $"Converts organics into {Link("Bio-Fuel", Elements.bioFuel)}, which can be burnt in a {Link("Bio-Fuel Generator", BioFuelGeneratorConfig.ID)}.";
				}

				public class BEACHED_CRYSTALSYNTHETIZER
				{
					public static LocString NAME = Link("Crystal Synthetizer", CrystalSynthetizerConfig.ID);
					public static LocString DESC = "Allows fabrication of Crystal Clusters, which can be placed in a Crystal Grower for renewable resources.";
					public static LocString EFFECTS = "A Duplicant with the Crystallograpy skill is required to operte this building.";
				}


				public class BEACHED_COLLARDISPENSER
				{
					public static LocString NAME = Link("Collar Rack", CollarDispenserConfig.ID);
					public static LocString DESC = "Dispenses Collars for Muffins to wear, and provides the interface to set instructions for tamed Muffins.";
					public static LocString EFFECTS = "A trained duplicant can equip Collars to Muffins.";
				}

				public class BEACHED_GEMCUTTER
				{
					public static LocString NAME = Link("Gem Cutter", GemCutterConfig.ID);
					public static LocString DESC = "Cuts and shapes gemstones and other materials into jewelry.";
					public static LocString EFFECTS = "Fabricates equippable jewelry from raw materials.";
				}

				public class BEACHED_DECONSTRUCTABLEROCKETTILE
				{
					public static LocString NAME = Link("Rocket Panel", DeconstructableRocketTileConfig.ID);
				}

				public class BEACHED_SHELLDOOR
				{
					public static LocString NAME = Link("Shell Curtain", "Beached_ShellCurtain");
					public static LocString DESC = "A curtain made of beads and shells. Duplicants can pass through without stopping.";
					public static LocString EFFECTS = "Separates rooms without obstructing gas or liquid flow.";
				}

				public class BEACHED_DNAANALYZER
				{
					public static LocString NAME = Link("DNA Analyzer", "Beached_DNAAnalyzer");
					public static LocString DESC = "Requires a Duplicant with Geneticist skill to operate.";
					public static LocString EFFECTS = "Extracts Genetic Samples from various objects, eggs or critters.";
				}

				public class BEACHED_HOLOGRAPHICPROJECTOR
				{
					public static LocString NAME = Link("Holographic Projector", "Beached_HolographicProjector");
					public static LocString DESC = "Projects images to thin layers of transparent materials, creating an illusion of 3-dimensional artwork.";
					public static LocString EFFECTS = $"Majorly increases {Link("Decor", "DECOR")}, contributing to {Link("Morale", "MORALE")}." +
						$"\n" +
						$"\nMust be configured by a Duplicant.";

					public class FACADES
					{
						public class SCIENTIST
						{
							public static LocString NAME = "Old Scientist";
							public static LocString DESC = "A glowing presentation of an accomplished scientist.";
						}

						public class CAT
						{
							public static LocString NAME = "Cat Cat Cat";
							public static LocString DESC = "Cat Cat Cat Cat Cat Cat Cat Cat Cat Cat Cat Cat";
						}

						public class ORB
						{
							public static LocString NAME = "Orb";
							public static LocString DESC = "It's actually a dodecahedron.";
						}

						public class SPIGOTRIDER
						{
							public static LocString NAME = "Spigot Rider";
							public static LocString DESC = "A heroic Duplicant riding a large Spigot Seal.";
						}

						public class DRAEDON
						{
							[Note("Terraria: Calamity reference")]
							public static LocString NAME = "Draedon";
							public static LocString DESC = "To any personnel engaged in the laboratories.Please wear your steel engraved ID badge at all times.It is the easiest method to discern your body if any accidents do occur.";
						}
					}
				}

				public class BEACHED_JUMBOGELBATTERY
				{
					public static LocString NAME = Link("Jumbo Gel Battery", "Beached_JumboGelBattery");
					public static LocString DESC = "A tiny round fishbowl for corals.";
					public static LocString EFFECT = "Houses a single Coral for decorative purposes.";
				}
				public class BEACHED_SMALLAQUARIUM
				{
					public static LocString NAME = Link("Glass Bowl", SmallAquariumConfig.ID);
					public static LocString DESC = "A tiny round fishbowl for corals.";
					public static LocString EFFECT = "Houses a single Coral for decorative purposes.";
				}

				public class BEACHED_LARGEAQUARIUM
				{
					public static LocString NAME = Link("Aquarium", "Beached_LargeAquarium");
					public static LocString DESC = "A large container suitable for housing aquatic or terrestrial life.";
					public static LocString EFFECT = "Houses several critters, plants or corals for decorative purposes.";
				}


				public class BEACHED_BAMBOOPLATFORM
				{
					public static LocString NAME = Link("Bamboo Walkway", "Beached_BambooWalkway");
					public static LocString DESC = "A narrow rickety walkway made of Bamboo stems.";
					public static LocString EFFECT = "Provides floor for Duplicants to walk on. Does not prevent gas or liquid flow.";
				}

				public class BEACHED_CHIME
				{
					public static LocString NAME = Link("Wind Chime", ChimeConfig.ID);
					public static LocString DESC = "Pretty things suspended in air, creating music.";
					public static LocString EFFECT = $"Emits a soothing sound when stimulated by changing air pressure, decreasing {Link("Stress", "STRESS")} of nearby Duplicants.";
				}

				public class BEACHED_CONDUITAQUARIUM
				{
					public static LocString NAME = Link("Conduit Coral Bed", "Beached_CoralConduit");
					public static LocString DESC = "A small Coral just barely fits into this.";
					public static LocString EFFECT = "Houses a single coral, connected to a liquid conduit input, with a gas and a liquid output.";
				}

				public class BEACHED_DNAINJECTOR
				{
					public static LocString NAME = Link("DNA Injector", DNAInjectorConfig.ID);
					public static LocString DESC = "The Power to alter Nature.";
					public static LocString EFFECTS = "Allows administering DNA samples to Eggs or Critters, applying traits to newborn critters.";
				}

				public class BEACHED_INTERPLANETARYPOWEROUTLET
				{
					public static LocString NAME = Link("Interplanetary Power Outlet", "Beached_InterplanetaryPowerOutlet");
					public static LocString DESC = "Ranged power!";
					public static LocString EFFECT = "Provides wireless power transmission between two remote locations. The outlet is highly volatile" +
													 "and will electrute and super-heat it's nearby area.";
				}

				public class BEACHED_INTERPLANETARYPOWERINLET
				{
					public static LocString NAME = Link("Interplanetary Power Inlet", "Beached_InterplanetaryPowerInlet");
					public static LocString DESC = "Ranged power!";
					public static LocString EFFECT = "Receives wireless power transmission between two remote locations. The inlet is highly volatile" +
													 "and will electrute and super-heat it's nearby area.";
				}

				public class BEACHED_LABORATORYTILES
				{
					public static LocString NAME = Link("Laboratory Tile", LaboratoryTileConfig.ID);
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_MAKIHUT
				{
					public static LocString NAME = Link("Maki Hut", "Beached_MakiHut");
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_MAKITRAININGGROUND
				{
					public static LocString NAME = Link("Maki Training Ground", "Beached_MakiTrainingGround");
					public static LocString DESC = "...";
					public static LocString EFFECT = "...";
				}

				public class BEACHED_MINIFRIDGE
				{
					public static LocString NAME = Link("Mini-Fridge", MiniFridgeConfig.ID);
					public static LocString DESC = "A tiny fridge to store a tiny bit of food for the tiny dupes.";
					public static LocString EFFECT = "TRANSLATION NOT NEEDED - gets copied from regular fridge";
				}

				public class BEACHED_MIRROR
				{
					public static LocString NAME = Link("Mirror", MirrorConfig.ID);
					public static LocString DESC = "A reflective surface with a quant frame. Tricks most critters into thinking their available space is larger.";
					public static LocString EFFECT = "Halves the space requirement of critters. Only one mirror can affect critters, the effect does not stack.";
				}

				public class BEACHED_MOSSBED
				{
					public static LocString NAME = Link("Moss Frame", MossBedConfig.ID);
					public static LocString DESC = "Grows a single tile of moss over a period of time. Requires a once time delivery of water; once grown the moss is converted to a natural tile.";
					public static LocString EFFECT = "Natural tiles can be used as walls and floors or for wild planting.";
				}

				public class BEACHED_MUDSTOMPER
				{
					public static LocString NAME = Link("Stomper", MudStomperConfig.ID);
					public static LocString DESC = "By repeated vertical motion of external duplicant peripherals it is possible to process several materials into one unified mush.";
					public static LocString EFFECT = "Mixes granular and liquid materials into compound results.";
				}

				public class BEACHED_SALTLICK
				{
					[Note("Visible in UI or codex.")]
					public static LocString NAME = Link("Critter Lick", "Beached_CritterLick");
					[Note("The name that appears on the actual building. ie. Salt-Lick, or Sulfur-Lick")]
					public static LocString FORMATTED_NAME = Link("{Element}-Lick", "Beached_CritterLick");
					public static LocString DESC = "A block of lickable material. Delicious!";
					public static LocString EFFECT = "Allows critters to consume additional materials, boosting production. \n" +
													 $"{CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES} can be fully sustained on appropiate licks.\n\n" +
													 "Requires refilling once depleted.";
				}

				public class BEACHED_SANDBOX
				{
					public static LocString NAME = Link("Sand Pile", SandBoxConfig.ID);
					public static LocString DESC = "A Duplicant can express themselves and build a sand castle... or sand cat, or a sand crab.";
					public static LocString EFFECT = $"Does not require art skills. Duplicants will use Sand Piles during their downtime. The Sand Pile will crumble and reset to a pile of sand in a cycle, ready to be used again. Until then it will provide additional {Link("Decor")}.";

					public class FACADES
					{
						public class CASTLE
						{
							public static LocString NAME = "Qaint Castle";
							public static LocString DESC = "A classic Sand Castle by a classy dupe.";
						}

						public class TIGER_SHARK
						{
							public static LocString NAME = "Terrifying Tiger Shark";
							public static LocString DESC = "A menacing creature, based on a game the dupe saw on the Arcade Machine.";
						}

						public class CASTLE2
						{
							public static LocString NAME = "Whimsical Castle";
							public static LocString DESC = "A castle straight from fairy tales.";
						}

						public class CRAB
						{
							public static LocString NAME = "Another Dupe's Treasure";
							public static LocString DESC = "The real treasure were the crabs we made along the way.";
						}

						public class CAVETIGER
						{
							[Note("Aladdin reference")]
							public static LocString NAME = "Wondrous Cave";
							public static LocString DESC = "A diamond in the rough.";
						}
					}
				}


				public class BEACHED_SMOKINGRACK
				{
					public static LocString NAME = Link("Smoking Rack", SmokingRackConfig.ID);
					public static LocString DESC = "Help imbuing various meals with the flavor of smoke.";

					public static LocString EFFECT =
						"Uses Carbon Dioxide from Smokers to smoke food, extending their shelf life and improving quality.\n\n" +
						$"Can also use ambient Carbon Dioxide if it's hot enough.";
				}

				public class BEACHED_SPINNER
				{
					[Note("\"Centrifuge\" or synonims also work")]
					public static LocString NAME = Link("Spinner", SpinnerConfig.ID);
					public static LocString DESC = $"Spinners use centrifugal force to spin various materials into fibers.";
					public static LocString EFFECT = "Produces fibers from solids." +
						"\n" +
						"\nDuplicants will not fabricate items unless recipes are queued.";
				}

				public class BEACHED_VULCANIZER
				{
					public static LocString NAME = Link("Vulcanizer", VulcanizerConfig.ID);
					public static LocString DESC = $"Hardens elastomers into {Link("Rubber", Elements.rubber)} by process of vulcanization..";
					public static LocString EFFECT = "Produces Rubber." +
						"\n" +
						"\nDuplicants will not fabricate items unless recipes are queued.";
				}

				public class BEACHED_WOODCARVING
				{
					public static LocString NAME = Link("Wood Carving", WoodCarvingConfig.ID);
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
					public static LocString TOOLTIP = "This building is operating slippery smooth.\n" +
						"\n" +
						"Remaining: {0}";
				}

				public class BEACHED_COLLECTINGRUBBER
				{
					public static LocString NAME = "Collecting Rubber";
					public static LocString TOOLTIP = "This building has been tapped, and Rubber has accumulated {0} Rubber in the attached storage.";
				}

				public class BEACHED_COLLECTINGRUBBERHALTED
				{
					public static LocString NAME = "Rubber Collection Halted";
					public static LocString TOOLTIP = "This tree cannot produce more rubber under the currect circumstances.";
				}

				public class BEACHED_COLLECTINGRUBBERFULL
				{
					public static LocString NAME = "Rubber Bucket Full";
					public static LocString TOOLTIP = "This tree has been fully tapped for now.";
				}

				public class BEACHED_PROJECTINGFORCEFIELD1
				{
					public static LocString NAME = "Projecting Forcefield";
					public static LocString TOOLTIP = "This shield generator is projecting a planet wide force field. It will break and destroy any object that enters the atmosphere.";
				}

				public class BEACHED_PLUSHED
				{
					public static LocString NAME = "{0} Plush";

					public static LocString TOOLTIP = "A {0} has been placed on this bed.\n\n<b>Effects</b>";
				}

				public class BEACHED_SANDBOXCRUMBLE
				{
					public static LocString NAME = "Temporary Artwork";

					public static LocString TOOLTIP = "This piece will become a blank canvas again in {0}.";
				}
			}
		}
	}
}