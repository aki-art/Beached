using FUtility.FLocalization;

namespace Beached.STRINGS
{
	public class WORLDS : StringsBase
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
			[Note("Based on Pelagic layer naming system of planets. Astropelagos = Ocean layer of the stars.")]
			public static LocString NAME = "Astropelagos";
			public static LocString DESCRIPTION = "";
		}
	}

}
