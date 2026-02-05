using Beached.Content;
using Beached.Content.Defs;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.Defs.Medicines;
using FUtility.FLocalization;

namespace Beached
{
	public partial class STRINGS
	{
		public class ITEMS
		{
			public class BIONIC_BOOSTERS
			{
				public class BEACHED_BOOSTER_PRECISION1
				{
					public static LocString NAME = Link("Precision Booster", ExtraBionicUpgradeComponentConfig.PRECISION1_ID);
					public static LocString DESC = "Grants a Bionic Duplicant the skill required to carefully extract items from dug materials.";
				}

				public class BEACHED_BOOSTER_PRECISION2
				{
					public static LocString NAME = Link("Extreme Precision Booster", ExtraBionicUpgradeComponentConfig.PRECISION2_ID);
					public static LocString DESC = "Grants a Bionic Duplicant the precision skill to handle crystals and gemstones.";
				}
			}

			public class MISC
			{
				public class BEACHED_SOAP
				{
					public static LocString NAME = Link("Soap", SoapConfig.ID);
					public static LocString DESC = $"An aromatic block of waxy soap.\n\nCan be delivered to a {Link("Shower", ShowerConfig.ID)} to increase the morale bonus from using it.";
				}

				public class BEACHED_SULFURGLAND
				{
					public static LocString NAME = Link("Stinky Gland", SulfurGlandConfig.ID);
					public static LocString DESC = $"Parts of digestive glands of Limpets. It reeks of {Link("Sulfur", SimHashes.Sulfur)}, and has applications in Vulcanization processes.";
				}

				public class BEACHED_OXYLITEPUFT
				{
					public static LocString NAME = Link("Oxylite Buddy", OxylitePuftConfig.ID);
					public static LocString DESC = "A sculpture made in the image of a Dense Puft. Unfortunately it is made out of Oxylite and will slowly sublimate away.";
				}

				public class BEACHED_GENETIC_SAMPLE
				{
					public static LocString NAME = "{0} Genetic Sample";
					public static LocString DESC = "...";
				}

				public class BEACHED_PALMLEAF
				{
					public static LocString NAME = Link("Palm Leaf", PalmLeafConfig.ID);
					public static LocString DESC = "The lead of a Dew Palm tree, it is very energy dense and makes for an excellent Bio-Fuel source.";
				}

				public class BEACHED_PALATECLEANSERFOOD
				{
					public static LocString NAME = "Palate Cleanser";
					public static LocString DESC = "...";
				}

				public class BEACHED_SLICKSHELLSHELL
				{
					public static LocString NAME = Link("Spiral Shell", SlickShellShellConfig.ID);
					public static LocString DESC = "Lime rich empty shell left behind by a Slickshell.";
				}

				public class BEACHED_IRONSHELLSHELL
				{
					public static LocString NAME = Link("Gilded Spiral Shell", IronShellShellConfig.ID);
					public static LocString DESC = "Pyrite lined empty shell left behind by a Slickshell.";
				}

				public class BEACHED_SEASHELL
				{
					public static LocString NAME = Link("Seashell", SeaShellConfig.ID);
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

			public class PILLS
			{
				public class BEACHED_SUPERALLERGYMEDICATION
				{
					public static LocString NAME = Link("Sneeze Begone", SuperAllergyMedicationConfig.ID);
					public static LocString DESC = "Suppresses and prevents allergic reactions.";
					public static LocString RECIPEDESC = $"An even stronger antihistamine Duplicants can take to alleviate allergic reactions. Lasts 7 cycles.";
				}
			}

			public class FOOD
			{
				public class BEACHED_ASPICLICE
				{
					public static LocString NAME = Link("Aspic Lice", AspicLiceConfig.ID);
					public static LocString DESC = $"Some {Link("Meal Lice", BasicPlantFoodConfig.ID)} suspended in jello.";
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
					public static LocString NAME = Link("Berry-Jelly", BerryJellyConfig.ID);
					public static LocString DESC = "A jiggly treat hiding tasty berries within.";
				}

				public class BEACHED_CRABCAKES
				{
					public static LocString NAME = Link("Crab Cakes", CrabCakesConfig.ID);
					public static LocString DESC = "Crunchy crabby bites.";
				}

				public class BEACHED_GLAZEDDEWNUT
				{
					public static LocString NAME = Link("Glazed Dewnut", GlazedDewnutConfig.ID);
					public static LocString DESC = "Baking the Dewnut causes it to form bubbles of air inside, giving it an unexpectedly soft texture. Topped with colored sugar frosting and sprinkles, this is delightfully sweet treat.";
				}

				public class BEACHED_MEATPLATTER
				{
					public static LocString NAME = Link("Meatlover's Platter", MeatPlatterConfig.ID);
					public static LocString DESC = "An assortment of high quality meats, fit for a feast.";
				}

				public class BEACHED_SUFFEDSNAILS
				{
					public static LocString NAME = Link("Stuffed Snails", StuffedSnailsConfig.ID);
					public static LocString DESC = "Pieces of snail cooked tender, drenched in butter, stuffed into small decorative edible snail shells.";
				}

				public class BEACHED_HARDBOILEDEGG
				{
					public static LocString NAME = Link("Hard Boiled Egg", HardBoiledEggConfig.ID);
					public static LocString DESC = "An unlucky egg that will never hatch.";
				}

				public class BEACHED_RAWSNAIL
				{
					public static LocString NAME = Link("Slimy Meat", RawSnailConfig.ID);
					public static LocString DESC = "Chewy meat of a creature dripping with mucus.";
				}

				public class BEACHED_COOKEDSNAIL
				{
					public static LocString NAME = Link("Seared Slimy Meat", SlickShellConfig.ID);
					public static LocString DESC = "A still chewy slab of meat, with a somewhat pleasant crispy outside.";
				}

				public class BEACHED_INFERTILEEGG
				{
					public static LocString NAME = Link("Egg", InfertileEggConfig.ID);
					public static LocString DESC = "This egg is infertile and will never hatch. Perfect for an omelette!";
				}

				public class BEACHED_SCRAMBLEDSNAIL
				{
					public static LocString NAME = Link("Slimy Scramble", SlickShellConfig.ID);
					public static LocString DESC = "A still chewy slab of meat, with a somewhat pleasant crispy outside.";
				}

				public class BEACHED_STUFFEDSNAILS
				{
					public static LocString NAME = Link("Stuffed Snail", StuffedSnailsConfig.ID);
					public static LocString DESC = $"Chewy meat jammed into the shells of {Link("Slickshells", SlickShellShellConfig.ID)}. The crunch is part of the experience.";
				}


				public class BEACHED_CRACKLINGS
				{
					public static LocString NAME = Link("Cracklings", CracklingsConfig.ID);
					public static LocString DESC = "Extremely crunchy bites, with fatty juices flowing out when bitten into. Has a distinctly metallic aftertaste.";
				}

				public class BEACHED_NUTTYDELIGHT
				{
					public static LocString NAME = "Nutty Delight";
					public static LocString DESC = "...";
				}

				public class BEACHED_SPONGECAKE
				{
					public static LocString NAME = Link("Sponge Cake", SpongeCakeConfig.ID);
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
					public static LocString DESC = $"Edible tongue of a {Link("Mussel Sprout", MusselSproutConfig.ID)}. It has a somewhat bitter, green flavour, and the consistency of fresh clam.";
				}

				public class BEACHED_VEGGIEBURGER
				{
					public static LocString NAME = Link("Veggie Burger", VeggieBurgerConfig.ID);
					public static LocString DESC = $"{Link("Fried Mushrooms", FriedMushroomConfig.ID)} and {Link("Lettuce", LettuceConfig.ID)} on a chilled {Link("Frost Bun", ColdWheatBreadConfig.ID)}.\n\nIt's the only other burger best served cold.";
					public static LocString RECIPEDESC = ($"{Link("Fried Mushrooms", FriedMushroomConfig.ID)} and {Link("Lettuce", LettuceConfig.ID)} on a chilled {Link("Frost Bun", ColdWheatBreadConfig.ID)}.");
				}

				public class BEACHED_HIGHQUALITYMEAT
				{
					public static LocString NAME = Link("High Quality Meat", HighQualityMeatConfig.ID);
					public static LocString DESC = $"Soft fatty meat of an unfortunate creature. \n\nExclusively dropped by normally {Link("Meat", MeatConfig.ID)} dropping critters who have the {Link("Meaty genetic trait", "GENETICS")}.";
				}

				public class BEACHED_FOULPOFF
				{
					public static LocString NAME = Link("Foul Poff", FoulPoffConfig.ID);
					public static LocString DESC = "An inedible chunk of fungal material. It has an offensive smell of mould and ash.";
				}

				public class BEACHED_SMOKEDSNAIL
				{
					public static LocString NAME = Link("Smoked Snail", SmokedSnailConfig.ID);
					public static LocString DESC = ".";
				}

				public class BEACHED_SPICYCRACKLINGS
				{
					public static LocString NAME = Link("Spicy Cracklings", SpicyCracklingsConfig.ID);
					public static LocString DESC = ".";
				}

				public class BEACHEDDEWNUT
				{
					public static LocString NAME = Link("Dew Nut", DewNutConfig.ID);
					public static LocString DESC = "An oddly round seed of Dew Palms.";
				}

				public class BEACHED_SCRAMBLEDSNAILS
				{
					public static LocString NAME = Link("Snail Scramble", ScrambledSnailsConfig.ID);
					public static LocString DESC = "The yolk was left extra runny to match the slimy texture of Slickshell Meat.";
				}

				public class BEACHED_DRYAGEDMEAT
				{
					public static LocString NAME = Link("Dry-Aged Meat", DryAgedMeatConfig.ID);
					public static LocString DESC = "Fatty Meat encrusted in thick Salt.";
				}

				public class BEACHED_DRYNOODLES
				{
					public static LocString NAME = Link("Dry Noodles", DryNoodlesConfig.ID);
					public static LocString DESC = ".. they don't taste very good. Unfortunately there is no flavor packet included.";
				}

				public class BEACHED_GNAWICABERRY
				{
					public static LocString NAME = Link("Gnawberry", GnawicaBerryConfig.ID);
					public static LocString DESC = "Fruit with a hard chewy skin that pops when punctured, spilling out the sweet fluid inside. At its center a hard seed floats.";
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

				public class BEACHED_BEACHED_NITROGEN_POFF_RAW
				{
					public static LocString NAME = Link("White Poff", PoffConfig.GetRawId(Elements.nitrogen));
					public static LocString DESC = "So bland, it sucks the taste out of the tongue.";
				}

				public class BEACHED_BEACHED_NITROGEN_POFF_COOKED
				{
					public static LocString NAME = Link("Cooked White Poff", PoffConfig.GetCookedId(Elements.nitrogen));
					public static LocString DESC = "Still has an astonishing lack of flavor, but now it's also mushy.";
				}

				public class BEACHED_BEACHED_SALTYOXYGEN_POFF_RAW
				{
					public static LocString NAME = Link("Pale Poff", PoffConfig.GetRawId(Elements.saltyOxygen));
					public static LocString DESC = "Tastes like an eraser.";
				}

				public class BEACHED_BEACHED_SALTYOXYGEN_POFF_COOKED
				{
					public static LocString NAME = Link("Cooked Pale Poff", PoffConfig.GetCookedId(Elements.saltyOxygen));
					public static LocString DESC = "Pleasantly chewy, with a strong flavor of salt.";
				}

				public class BEACHED_BEACHED_AMMONIA_POFF_RAW
				{
					public static LocString NAME = Link("Purple Poff", PoffConfig.poffLookup[Elements.ammonia].raw);
					public static LocString DESC = "Leaves a stinging feel in the mouth.";
				}

				public class BEACHED_BEACHED_AMMONIA_POFF_COOKED
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

				public class FLAWLESS_DIAMOND
				{
					public static LocString NAME = Link("Flawless Diamond", RareGemsConfig.FLAWLESS_DIAMOND);
					public static LocString DESCRIPTION = "A perfect Diamond sparling in every color of the rainbow.";
				}

				public class HADEAN_ZIRCON
				{
					[Note("Hadean Zircon is a specific type of real life Zircon, from the Hadean period from 4 billion years ago.")]
					public static LocString NAME = Link("Hadean Zircon", RareGemsConfig.HADEAN_ZIRCON);
					public static LocString DESCRIPTION = "A 4 billion years old piece of pristine Zircon. \n\nA long time ago a Hadean Zircon of this size and purity would have sent the global scientific community into an upheaval, shaking the very foundation of humanity's understanding of geology and the Earth's formation. Nowadays, it makes for a neat necklace.";
				}

				public class MAXIXE
				{
					[Note("Maxixe beryl is a real gemstone, but probably has little direct translations. \"Blue Beryl\" works too. https://en.wikipedia.org/wiki/Beryl")]
					public static LocString NAME = Link("Maxixe", RareGemsConfig.MAXIXE);
					public static LocString DESCRIPTION = "A beryl gemstone with a captivatingly dark blue shade.";
				}

				public class MOTHER_PEARL
				{
					public static LocString NAME = Link("Mother Pearl", RareGemsConfig.MOTHER_PEARL);
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
					public static LocString NAME = Link("Flying Centipede", AmberInclusionsConfig.FLYING_CENTIPEDE);
					public static LocString DESCRIPTION = $"And ancient centipede trapped in {Link("Amber", "BEACHEDAMBER")}.";
				}

				public class STRANGEHATCH
				{
					public static LocString NAME = Link("Strange Hatch", AmberInclusionsConfig.STRANGE_HATCH);
					public static LocString DESCRIPTION = $"And oddly smooth and tiny hatch trapped in {Link("Amber", "BEACHEDAMBER")}.";
				}

				public class FEATHER
				{
					public static LocString NAME = Link("Preserved Feather", AmberInclusionsConfig.FEATHER);
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