using Beached.Content;

namespace Beached
{
	public partial class STRINGS
	{
		public class ENTITIES
		{
			public class BEACHED_LIMPETROCK
			{
				public static LocString NAME = "Limpet Rock";
				public static LocString DESCRIPTION = "This boulder has been completely overtaken by a colony of Limpets. The inhabitants wil periodically release a small amount of gas saturated with Limpet Eggs.";
			}

			public class BEACHED_CRYSTALS
			{
				public static class BEACHED_ZEOLITECRYSTAL
				{
					public static LocString NAME = "Zeolite Crystal";
					public static LocString DESCRIPTION = $"A large crystal composed of {Link("Zeolite", Elements.zeolite)}.";
				}

				public static class BEACHED_ZEOLITECRYSTALCLUSTER
				{
					public static LocString NAME = "Zeolite Cluster";
					public static LocString DESCRIPTION = $"A small cluster of green opaque crystals.";
				}

				public static class BEACHED_OXYROCKCRYSTAL
				{
					public static LocString NAME = "Oxylite Crystal";
					public static LocString DESCRIPTION = $"A large crystal composed of {Link("Oxylite", SimHashes.OxyRock)}. Slowly releases it's bound Oxygen while exposed to air.";
				}

				public static class BEACHED_OXYROCKCRYSTALCLUSTER
				{
					public static LocString NAME = "Oxylite Cluster";
					public static LocString DESCRIPTION = $"A small cluster ofoxylite crystals.";
				}
			}

			public class GLACIERS
			{
				public static LocString GENERIC_THAW = "Thaw or dig up this feature to retrieve the treasures from the inside.";

				public class BEACHED_GLACIER_MUFFINS
				{
					public static LocString NAME = "Trapped Creatures";
					public static LocString DESCRIPTION = "Two unknown critters are trapped in this ice. Based on their large teeth and claws, if I want to thaw them I should prepare for accomodations fit for a carnivore with a big appetite. It appears these are the only two speciments left in existence.";
				}

				public class BEACHED_GLACIER_SMALL_GENERIC
				{
					public static LocString NAME = "Small Glacier";
					public static LocString DESCRIPTION = "A chunk of ice. There is something inside of it.";
				}
			}

			public class SET_PIECES
			{
				public class BEACHED_BEACHSETPIECE
				{
					public static LocString NAME = "Cave Vista";
					public static LocString DESCRIPTION = "A large empty cave system stretches into the far side of the Asteroid. While looking impressive, it has few valuable resources, and it is far too unstable to build inside. There is a faint but constant stream of Oxygen flowing out.";
				}

				public class BEACHED_ZEOLITESETPIECE
				{
					public static LocString NAME = "Geode Vista";
					public static LocString DESCRIPTION = "A small indent on the wall, filled with green crystals of Zeolite.";
				}
			}

			public class BEACHED_UNCONSCIOUS_CRITTERS
			{
				public static LocString NAME = "Unconscious Critters";
				public static LocString DESCRIPTION = "Two sleeping creatures were thawed from the ice.";
			}

			public class BEACHED_SKELETON_CHAIR
			{
				public static LocString NAME = "Chill Skeleton";
				public static LocString DESCRIPTION = "These bones have the shape and proportions of an average Duplicant... it's probably a coincidence.";
			}
		}
	}
}
