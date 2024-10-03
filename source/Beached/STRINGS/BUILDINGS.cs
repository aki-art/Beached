using Beached.Content.Defs.Buildings;
using FUtility.FLocalization;
using KLEISTRINGS = STRINGS;

namespace Beached
{
	public partial class STRINGS
	{
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
			public class PREFABS
			{
				public class BEACHED_AMMONIAGENERATOR
				{
					[Note("Generates power from ammonia")]
					public static LocString NAME = Link("Ammonia Generator", AmmoniaGeneratorConfig.ID);
					public static LocString DESC = "Ammonia generators are not very efficient, and emit a lot of waste materials.";
					public static LocString EFFECT = $"Converts {ELEMENTS.AMMONIA.NAME} into electrical {Link("Power", "POWER")}, " +
													 $"{KLEISTRINGS.ELEMENTS.WATER.NAME} and {ELEMENTS.NITROGEN.NAME}.";
				}

				public class BEACHED_ATMOSPHERICFORCEFIELDGENERATOR
				{
					public static LocString NAME = Link("Atmospheric Forcefield Generator", ForceFieldGeneratorConfig.ID);
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

				public class BEACHED_CHIME
				{
					public static LocString NAME = Link("Wind Chime", ChimeConfig.ID);
					public static LocString DESC = "Pretty things suspended in air, creating music.";
					public static LocString EFFECT = "Emits a soothing sound when stimulated by changing air pressure, decreasing Stress of nearby Duplicants.";
				}

				public class BEACHED_CONDUITAQUARIUM
				{
					public static LocString NAME = "Conduit Coral Bed";
					public static LocString DESC = "...";
					public static LocString EFFECT = "Houses a single coral, connected to a liquid conduit.";
				}

				public class BEACHED_DNAINJECTOR
				{
					public static LocString NAME = Link("DNA Injector", ForceFieldGeneratorConfig.ID);
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
					public static LocString NAME = Link("Mini-Fridge", MiniFridgeConfig.ID);
					public static LocString DESC = "A tiny fridge to store a tiny bit of food for the tiny dupes.";
					public static LocString EFFECT = "TRANSLATION NOT NEEDED - gets copied from regular fridge";
				}

				public class BEACHED_MOSSBED
				{
					public static LocString NAME = Link("Moss Frame", MossBedConfig.ID);
					public static LocString DESC = "Grows a single tile of moss over a period of time. Requires a once time delivery of water; once grown the moss is converted to a natural tile.";
					public static LocString EFFECT = "Natural tiles can be used as walls and floors or for wild planting.";
				}

				public class BEACHED_MUDSTOMER
				{
					public static LocString NAME = Link("Stomper", MudStomperConfig.ID);
					public static LocString DESC = "By repeated vertical motion of external duplicant peripherals it is possible to process several materials into one unified mush.";
					public static LocString EFFECT = "Mixes granular and liquid materials into compound results.";
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

						// Aladdin sand tiger head
						// some Terraria or Calamity sand boi
					}
				}

				public class BEACHED_SMALLAQUARIUM
				{
					public static LocString NAME = "Small Aquarium";
					public static LocString DESC = "";

					public static LocString EFFECT =
						"Houses a single aquatic plant.";
				}

				public class BEACHED_SMOKINGRACK
				{
					public static LocString NAME = "Smoking Rack";
					public static LocString DESC = "";

					public static LocString EFFECT =
						"Uses Salt and Carbon Dioxide to smoke food, extending their shelf life and improving quality.\n\n" +
						$"Can also use ambient Carbon Dioxide if it'a already hot enough (at least {GameUtil.GetFormattedTemperature(343.15f)}).";
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

					public static LocString TOOLTIP = "A {0} has been placed on this bed.\n\n<b>Effects</b>";
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