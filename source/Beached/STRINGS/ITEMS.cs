﻿using Beached.Content;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using FUtility.FLocalization;

namespace Beached
{
	public partial class STRINGS
	{
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

				public class BEACHED_DRYAGEDMEAT
				{
					public static LocString NAME = FormatAsLink("Dry Aged Meat", DryAgedMeatConfig.ID);
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

			public class AMBER_INCLUSIONS
			{
				public class FLYINGCENTIPEDE
				{
					public static LocString NAME = "Flying Centipede";
					public static LocString DESCRIPTION = $"And ancient centipede trapped in {ELEMENTS.AMBER.NAME}.";
				}

				public class STRANGEHATCH
				{
					public static LocString NAME = "Strange Hatch";
					public static LocString DESCRIPTION = $"And oddly smooth and tiny hatch trapped in {ELEMENTS.AMBER.NAME}.";
				}

				public class FEATHER
				{
					public static LocString NAME = "Preserved Feather";
					public static LocString DESCRIPTION = $"A single feather trapped in {ELEMENTS.AMBER.NAME}.";
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

	}
}