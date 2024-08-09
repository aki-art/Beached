using Beached.Content.Defs.Comets;
using FUtility.FLocalization;

namespace Beached.STRINGS
{
	public class COMETS : StringsBase
	{
		public class SHRAPNEL
		{
			public static LocString NAME = "Shrapnel";
			public static LocString DESC = "Small piece of a metal violently flung from an explosion.";
		}

		public class BEACHED_DIAMONDCOMET
		{
			public static LocString NAME = FormatAsLink("Diamond Comet", DiamondCometConfig.ID);
			public static LocString DESC = "TODO";
		}

		public class BEACHED_SPARKLINGZIRCONCOMET
		{
			public static LocString NAME = FormatAsLink("Sparkling Zircon Comet", SparklingZirconCometConfig.ID);
			public static LocString DESC = "TODO";
		}

		public class BEACHED_SPARKLINGAQUAMARINECOMET
		{
			public static LocString NAME = FormatAsLink("Sparkling Aquamarine Comet", SparklingAquamarineCometConfig.ID);
			public static LocString DESC = "TODO";
		}

		public class BEACHED_SPARKLINGDIAMONDCOMET
		{
			public static LocString NAME = FormatAsLink("Sparkling Diamond Comet", "");
			public static LocString DESC = "TODO";
		}

		public class BEACHED_SPARKLINGVOIDCOMET
		{
			public static LocString NAME = FormatAsLink("Sparkling Void Comet", SparklingVoidCometConfig.ID);
			public static LocString DESC = "TODO";
		}
	}
}
