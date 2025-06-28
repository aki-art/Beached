using Beached.Content.Defs.Equipment;
using FUtility.FLocalization;

namespace Beached
{
	public partial class STRINGS
	{
		public class EQUIPMENT
		{
			public class PREFABS
			{
				public class BEACHED_BEACHSHIRT
				{
					public static LocString NAME = Link("Beach Shirt", BeachShirtConfig.ID);
					public static LocString GENERICNAME = "Beach Shirts";
					public static LocString DESCRIPTION = "A clothing item that is cool in both ways.";

					public static class FACADES
					{
						public static LocString GREEN = "Green Beach Shirt";
						public static LocString GREEN_DESCRIPTION = "desc";
					}
				}

				public class BEACHED_EQUIPMENT_RUBBERBOOTS
				{
					public static LocString NAME = Link("Rubber Boots", RubberBootsConfig.ID);
					public static LocString GENERICNAME = "Rubber Boots";
					public static LocString DESCRIPTION = "Protects the feet of Duplicants from being wet.";
				}

				public class BEACHED_EQUIPMENT_MAXIXEPENDANT
				{
					public static LocString NAME = Link("Maxixe Pendant", MaxixePendantConfig.ID);
					public static LocString GENERICNAME = "Maxixe Pendant";
					public static LocString DESCRIPTION = "This pendant has a blue color reminiscent of the deep sea.";
				}

				public class BEACHED_EQUIPMENT_PEARLNECKLACE
				{
					public static LocString NAME = Link("Pearl Necklace", PearlNecklaceConfig.ID);
					public static LocString GENERICNAME = "Pearl Necklace";
					public static LocString DESCRIPTION = "This pearlescent shine would swoon anyone!";
				}

				public class BEACHED_EQUIPMENT_HADEANZIRCONAMULET
				{
					public static LocString NAME = Link("Hadean Zircon Amulet", HadeanZirconAmuletConfig.ID);
					public static LocString GENERICNAME = "Hadean Zircon Amulet";
					public static LocString DESCRIPTION = "The bright red sparkle of this gem reminds Duplicants of the importance of working hard.";
				}

				public class BEACHED_EQUIPMENT_HEMATITENECKLACE
				{
					public static LocString NAME = Link("Hematite Necklace", HematiteNecklaceConfig.ID);
					public static LocString GENERICNAME = "Hematite Necklace";
					public static LocString DESCRIPTION = "Heavy and stylish.";
				}

				public class BEACHED_EQUIPMENT_ZEOLITEPENDANT
				{
					public static LocString NAME = Link("Zeolite Pendant", ZeolitePendantConfig.ID);
					public static LocString GENERICNAME = "Zeolite Pendant";
					public static LocString DESCRIPTION = "This gemstone has a particularly soothing color. When duplicants look at it, they feel at ease.";
				}

				public class BEACHED_EQUIPMENT_STRANGEMATTERAMULET
				{
					[Note("https://en.wikipedia.org/wiki/Strange_matter")]
					public static LocString NAME = Link("Strange Amulet", StrangeMatterAmuletConfig.ID);
					public static LocString GENERICNAME = "Strange Amulet";
					public static LocString DESCRIPTION = "A piece of material with Universe-defying qualities; safely contained.";
				}
			}
		}
	}
}
