using FUtility.FLocalization;

namespace Beached.STRINGS
{
	public class WORLD_TRAITS : StringsBase
	{
		public class DAMP
		{
			public static LocString NAME = "Damp";
			public static LocString DESCRIPTION = "This world is unusually humid, aiding plant growth and germ .";
		}

		public class ARID
		{
			public static LocString NAME = "Arid";
			public static LocString DESCRIPTION = "This world is unusually dry, plants have difficulty growing in this environment.";
		}

		public class EXTRAREEF
		{
			public static LocString NAME = "Coral Reef";
			public static LocString DESCRIPTION = "A submerged cave of corals, pearls and a plankton providing underwater geyser.";
		}

		public class SULFUROUS_CORE
		{
			public static LocString NAME = "Sulfurous Core";
			public static LocString DESCRIPTION = "This world has a core of molten Sulfur. And snails.";
		}

		public class CRYSTAL_GEODES
		{
			[Note("Caves with crystal blocks, and growing crystal clusters inside.")]
			public static LocString NAME = "Crystal Geodes";
			public static LocString DESCRIPTION = "Geodes of crystal clusters spawn in this world";
		}
	}

}
