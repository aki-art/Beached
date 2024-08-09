using FUtility.FLocalization;

namespace Beached.STRINGS
{
	public class CORALS : StringsBase
	{
		public class BEACHED_CORAL_WASHUSPONGE
		{
			public static LocString NAME = "Washu Sponge";
			public static LocString DESCRIPTION = "Woshu Sponge can sustain itself by consuming germs. This happens to be very useful for those wishing to keep their liquid reservoirs clean.\n" +
				"\n" +
				"Removes germs from liquids, and produces edible frags.";
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
}
