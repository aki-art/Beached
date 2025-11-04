using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Flora;
using Beached.Content.ModDb;

namespace Beached
{
	public partial class STRINGS
	{
		public class RESEARCH
		{
			public class TECHITEMS
			{
				public class BEACHED_RUBBER
				{
					public static LocString NAME = Link("Rubber", Elements.rubber);
					public static LocString DESC = $"Tap {Link("Dewpalm Trees", DewPalmConfig.ID)} to produce rubber, or manufacture it in a {Link("Vulcanizer", VulcanizerConfig.ID)}.";
				}
			}
			public class TECHS
			{
				public class BEACHED_CURRENTS
				{
					public static LocString NAME = Link("Kinetic Energy", BTechs.HYDRO_ELECTRONICS);
					public static LocString DESC = "Convert kinetic energy into electrical power.";
				}

				public class BACHED_MATERIALSI
				{
					public static LocString NAME = Link("Materials I", BTechs.MATERIALS1);
					public static LocString DESC = "Basic understanding of material properties, such as elasticity or conduction.";
				}
			}

			public class TREES
			{
				public static LocString TITLE_UNKNOWN = "Unknown";
			}
		}
	}
}
