using Beached.Content;
using Beached.Content.Defs.Entities.Critters.SlickShells;
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

			public class INDUSTRIAL_INGREDIENTS
			{
				public class BEACHED_SLAGWOOL
				{
					public static LocString NAME = Link("Slag Wool", SlagWoolConfig.ID);
					public static LocString DESC = $"Fibrous material spun from Slag, used for insulation and filtration.";
				}
			}

			public class FOOD
			{
				public class BEACHED_ASPICLICE
				{
					public static LocString NAME = Link("Aspic Lice", JellyConfig.ID);
					public static LocString DESC = "Lice suspended in jello.";
				}

				public class BEACHED_ASTROBAR
				{
					public static LocString NAME = Link("Astrobar", AstrobarConfig.ID);
					public static LocString DESC = "Delicious and nutritious candy bar, with a sticky and gooey filling that " +
												   "sticks to the roof of the mouth.";
				}

				public class BEACHED_COTTONCANDY
				{
					public static LocString NAME = Link("Cotton Candy", CottonCandyConfig.ID);
					public static LocString DESC = "Fruit flavored confection spun from sugars.\n" +
						"\n" +
						"The fluffy texture makes for a perfect treat.";

					public static LocString RECIPEDESC = "Fruit flavored confection spun from sugars.";
				}

				public class BEACHED_LEGENDARYSTEAK
				{
					public static LocString NAME = Link("Legendary Steak", LegendarySteakConfig.ID);
					public static LocString DESC = "Truly, the rarest steak of them all. A wonderful cut of meat that melts in the mouth.";
				}

				public class BEACHED_JELLYBAR
				{
					public static LocString NAME = Link("Jelly Brick", JellyBarConfig.ID);
					public static LocString DESC = "A solid slab of flavored Jelly.";
				}

				public class BEACHED_BERRYJELLY
				{
					public static LocString NAME = Link("Berry-Jelly", JellyConfig.ID);
					public static LocString DESC = "A jiggly treat hiding tasty berries within.";
				}

				public class BEACHED_RAWSNAIL
				{
					public static LocString NAME = Link("Raw Snail", RawSnailConfig.ID);
					public static LocString DESC = "Slimy chewy meat of a Slickshell.";
				}

				public class BEACHED_COOKEDSNAIL
				{
					public static LocString NAME = Link("Seared Snail", SlickShellConfig.ID);
					public static LocString DESC = "A still chewy slab of meat, with a somewhat pleasant crispy outside.";
				}

				public class BEACHED_INFERTILEEGG
				{
					public static LocString NAME = Link("Egg", InfertileEggConfig.ID);
					public static LocString DESC = "This egg is infertile and will never hatch. Perfect for an omelette!";
				}

				public class BEACHED_SCRAMBLEDSNAIL
				{
					public static LocString NAME = Link("Snail Scramble", SlickShellConfig.ID);
					public static LocString DESC = "A still chewy slab of meat, with a somewhat pleasant crispy outside.";
				}

				public class BEACHED_STUFFEDSNAIL
				{
					public static LocString NAME = "Stuffed Snail";
					public static LocString DESC = "...";
				}


				public class BEACHED_CRACKLINGS
				{
					public static LocString NAME = Link("Cracklings", CracklingsConfig.ID);
					public static LocString DESC = "Extremely crunchy.";
				}

				public class BEACHED_NUTTYDELIGHT
				{
					public static LocString NAME = "Nutty Delight";
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
					public static LocString NAME = Link("Jelly", JellyConfig.ID);
					public static LocString DESC = "See-through edible blob. It tastes like solidified water.";
				}

				public class BEACHED_MUSSELTONGUE
				{
					public static LocString NAME = Link("Mussel Tongue", MusselTongueConfig.ID);
					public static LocString DESC = "Edible tongue of a Mussel Sprout. It has a somewhat bitter, green flavour, and the consistency of fresh clam.";
				}

				public class BEACHED_GLAZEDDEWNUT
				{
					public static LocString NAME = "Glazed Dewnut";
					public static LocString DESC = "...";
				}

				public class BEACHED_HIGHQUALITYMEAT
				{
					public static LocString NAME = Link("High Quality Meat", HighQualityMeatConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_DRYAGEDMEAT
				{
					public static LocString NAME = Link("Dry Aged Meat", DryAgedMeatConfig.ID);
					public static LocString DESC = "Soft fatty meat of an unfortunate creature.";
				}

				public class BEACHED_DRYNOODLES
				{
					public static LocString NAME = Link("Dry Noodles", DryNoodlesConfig.ID);
					public static LocString DESC = ".. they don't taste very good. Unfortunately there is no flavor packet included.";
				}

				public class BEACHED_SALTRUBBEDJELLY
				{
					public static LocString NAME = Link("Salt Rubbed Jelly", SaltRubbedJellyConfig.ID);
					public static LocString DESC = "A bit of salt greatly elevates raw jelly.";
				}

				public class BEACHED_SEAFOODPASTA
				{
					public static LocString NAME = Link("Seafood Pasta", SeafoodPastaConfig.ID);
					public static LocString DESC = "TODO.";
				}

				public class BEACHED_SPAGHETTI
				{
					public static LocString NAME = Link("Spaghetti", SpaghettiConfig.ID);
					public static LocString DESC = "The favorite meal of many duplicants and programmers alike.";
				}

				public class BEACHED_SMOKEDMEAT
				{
					public static LocString NAME = Link("Smoked Meat", SmokedMeatConfig.ID);
					public static LocString DESC = "Meat imbued with the wonderful aroma of smoke.";
				}

				public class BEACHED_SMOKEDFISH
				{
					public static LocString NAME = Link("Smoked Fish", SmokedFishConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_SMOKEDLICE
				{
					public static LocString NAME = Link("Smoked Lice", SmokedLiceConfig.ID);
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
					public static LocString NAME = Link("Smoked Tofu", SmokedTofuConfig.ID);
					public static LocString DESC = "...";
				}

				public class BEACHED_OXYGEN_POFF_RAW
				{
					public static LocString NAME = Link("Blue Poff", PoffConfig.GetRawId(SimHashes.Oxygen));
					public static LocString DESC = "...";
				}

				public class BEACHED_OXYGEN_POFF_COOKED
				{
					public static LocString NAME = Link("Cooked Blue Poff", PoffConfig.GetCookedId(SimHashes.Oxygen));
					public static LocString DESC = "...";
				}

				public class BEACHED_NITROGEN_POFF_RAW
				{
					public static LocString NAME = Link("White Poff", PoffConfig.GetRawId(Elements.nitrogen));
					public static LocString DESC = "So bland, it sucks the taste out of the tongue.";
				}

				public class BEACHED_NITROGEN_POFF_COOKED
				{
					public static LocString NAME = Link("Cooked White Poff", PoffConfig.GetCookedId(Elements.nitrogen));
					public static LocString DESC = "Still has an astonishing lack of flavor, but now it's also mushy.";
				}

				public class BEACHED_SALTYOXYGEN_POFF_RAW
				{
					public static LocString NAME = Link("Pale Poff", PoffConfig.GetRawId(Elements.saltyOxygen));
					public static LocString DESC = "Tastes like an eraser.";
				}

				public class BEACHED_SALTYOXYGEN_POFF_COOKED
				{
					public static LocString NAME = Link("Cooked Pale Poff", PoffConfig.GetCookedId(Elements.saltyOxygen));
					public static LocString DESC = "Pleasantly chewy, with a strong flavor of salt.";
				}

				public class BEACHED_AMMONIA_POFF_RAW
				{
					public static LocString NAME = Link("Purple Poff", PoffConfig.poffLookup[Elements.ammonia].raw);
					public static LocString DESC = "Leaves a stinging feel in the mouth.";
				}

				public class BEACHED_AMMONIA_POFF_COOKED
				{
					public static LocString NAME = Link("Cooked Purple Poff", PoffConfig.poffLookup[Elements.ammonia].cooked);
					public static LocString DESC = "Tastes alright, as long as you pinch your nose and don't smell it.";
				}

				public class BEACHED_RAWKELP
				{
					public static LocString NAME = Link("Raw Kelp", RawKelpConfig.ID);
					public static LocString DESC = "Slimy and salty leaf of a Kelp plant.";
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
					public static LocString DESCRIPTION = "A perfect Diamond sparling in every color of the rainbow.";
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
					public static LocString NAME = Link("Strange Matter", RareGemsConfig.STRANGE_MATTER);
					public static LocString DESCRIPTION = "A droplet of strange quark matter, safely contained.";
				}
			}

			public class AMBER_INCLUSIONS
			{
				public class FLYINGCENTIPEDE
				{
					public static LocString NAME = "Flying Centipede";
					public static LocString DESCRIPTION = $"And ancient centipede trapped in {Link("Amber", "BEACHEDAMBER")}.";
				}

				public class STRANGEHATCH
				{
					public static LocString NAME = "Strange Hatch";
					public static LocString DESCRIPTION = $"And oddly smooth and tiny hatch trapped in {Link("Amber", "BEACHEDAMBER")}.";
				}

				public class FEATHER
				{
					public static LocString NAME = "Preserved Feather";
					public static LocString DESCRIPTION = $"A single feather trapped in {Link("Amber", "BEACHEDAMBER")}.";
				}
			}

			public class STATUSITEMS
			{
				public class BEACHED_MEAT
				{
					public static LocString NAME = "Meat";

					public static LocString TOOLTIP = "This food is appropiate for Carnivorous diets, and makes vegetarian Duplicants sad.";
				}

				public class BEACHED_NONVEGA
				{
					public static LocString NAME = "Non-Vega";

					public static LocString TOOLTIP = "This food is not eligible for Carnivorous diets, but vegetarians still find it unsavory.";
				}

				public class BEACHED_VEGETARIAN
				{
					public static LocString NAME = "Vegetarian";

					public static LocString TOOLTIP = "This food is favored by Vegetarians. It does not count for Carnivorous diets .";
				}
			}

			public class INDUSTRIAL_PRODUCTS
			{
				public class BEACHED_SLICKSHELLSHELL
				{
					public static LocString NAME = "Slickshell House";
					public static LocString DESC = "No one lives here anymore.";
				}
			}
		}

	}
}