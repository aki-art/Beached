using Beached.Content.ModDb;
using UnityEngine;

namespace Beached
{
	public class CONSTS
	{
		public const float CYCLE_LENGTH = 600f;
		public const float ANIM_SCALE = 0.005f;

		public const string DOT_PREFIX = "\n    • ";
		public const string BEACHED_CLUSTER_SETTING_ID = "clusters/AstropelagosMoonlets";

		public static class CRITTER_LOAD_ORDER
		{
			public const int
				BABY = 100,
				ADULT = 0;
		}

		// referencing the classes static will load some stuff early, this is safer
		public static class PREFAB_IDS
		{
			public const string
				MICROBE_MUSHER = "MicrobeMusher",
				DEEP_FRIER = "Deepfryer",
				GRILL = "CookingStation",
				GAS_RANGE = "GourmetCookingStation",
				DEHYDRATOR = "FoodDehydrator",
				MOO = "Moo";
		}

		// weirdly off named things
		public static class PLANTS
		{
			public static readonly string
				MEALWOOD = BasicSingleHarvestPlantConfig.ID,
				THIMBLE_REED = BasicFabricMaterialPlantConfig.ID,
				MUCKROOT = BasicForagePlantPlantedConfig.ID,
				HEXALENT = ForestForagePlantPlantedConfig.ID;
		}

		public static class DUPLICANTS
		{
			public const float EASILY_TRIGGERED_THRESHOLD = 0.9f;

			public static class LIFEGOALS
			{
				public const int MORALBONUS = 2;
			}

			public const string HISTAMINE_SUPPRESSION = "HistamineSuppression";

			public static class GENDER
			{
				public const string
					FEMALE = "Female",
					MALE = "Male",
					NONBINARY = "Nb"; // displays as X on UI
			}

			public static class JOY_TRAITS
			{
				public const string
					BALLOONARTIST = "BalloonArtist",
					SPARKLESTREAKER = "SparkleStreaker",
					STICKERBOMBER = "StickerBomber",
					SUPERPRODUCTIVE = "SuperProductive",
					HAPPYSINGER = "HappySinger",

					// Bionic
					DATA_RAINER = "DataRainer",
					ROBO_DANCER = "RoboDancer",

					// BEACHED
					PLUSHIEMAKER = BTraits.PLUSHIE_MAKER;
			}

			public static class STRESS_TRAIT
			{
				public const string
					AGGRESSIVE = "Aggressive",
					STRESSVOMITER = "StressVomiter",
					UGLYCRIER = "UglyCrier",
					BINGEEATER = "BingeEater",

					// Bionic
					STRESSSHOCKER = "StressShocker",

					// Beached
					SIREN = BTraits.SIREN;
			}

			public static class BIONIC_BUGS
			{
				public const string
					RIGIDTHINKING_LEARNING_STRENGTH = "BionicBug1",
					DISSOCIATIVE_RANCHING_CARING = "BionicBug2",
					ALLTHUMBS_DIGGING_MACHINERY = "BionicBug3",
					OVERENGINEERED_CONSTRUCTION_ART = "BionicBug4",
					LATEBLOOMER_ATHLETICS_BOTANIST = "BionicBug5",
					URBANITE_COOKING_BOTANIST = "BionicBug6",
					ERRORPRONE_CARING_LEARNING = "BionicBug7";
			}
		}

		public static class ACID_VULNERABILITY
		{
			public const float IMMUNE = 0;
			public const float MILDLY_ANNOYING = 0.01f;
			public const float BAD = 0.05f;
			public const float PRETTY_BAD = 0.1f;
			public const float EXTREME = 0.2f;
		}

		public static class CORROSION_VULNERABILITY
		{
			public const float NONREACTIVE = 0f;
			public const float LITTLE_REACTIVE = 0.01f;
			public const float MEDIUM = 0.05f;
			public const float REACTIVE = 0.33f;
			public const float VERY_REACTIVE = 0.5f;
			public const float INSTANTLY_MELT = 1f;
		}

		/// <see cref="Patches.MinionConfigPatch.MinionConfig_CreatePrefab_Patch />
		public static class SNAPONS
		{
			public const string CAP = "Beached_SnapOn_Cap";
			public const string RUBBER_BOOTS = "Beached_SnapOn_RubberBoots";

			public static class JEWELLERIES
			{
				public const string MAXIXE = "Beached_SnapOn_Maxixe";
				public const string ZIRCON = "Beached_SnapOn_HadeanZircon";
				public const string PEARL = "Beached_SnapOn_Pearl";
				public const string HEMATITE = "Beached_SnapOn_Hematite";
				public const string ZEOLITE = "Beached_SnapOn_Zeolite";
				public const string STRANGE_MATTER = "Beached_SnapOn_StrangeMatter";
				public const string FLAWLESS_DIAMOND = "Beached_SnapOn_FlawlessDiamond"; // TODO
			}
		}

		public class BATCH_TAGS
		{
			public const int SWAPS = -77805842;
			public const int INTERACTS = -1371425853;
		}

		public class COLORS
		{
			public static readonly Color
				KLEI_PINK = new Color32(127, 61, 94, 255),
				KLEI_BLUE = new Color32(62, 67, 87, 255);
		}

		// TUNING is missing half of these
		public static class AUDIO_CATEGORY
		{
			public const string
				METAL = "Metal",
				GLASS = "Glass",
				HOLLOWMETAL = "HollowMetal",
				PLASTIC = "Plastic",
				SOLIDMETAL = "SolidMetal";
		}

		public static class NAV_GRID
		{
			public const string
				WALKER_BABY = "WalkerBabyNavGrid",
				WALKER_1X1 = "WalkerNavGrid1x1",
				WALKER_1X2 = "WalkerNavGrid1x2",
				MINION = "MinionNavGrid",
				ROBOT = "RobotNavGrid",
				DIGGER = "DiggerNavGrid",
				DRECKO = "DreckoNavGrid",
				DRECKO_BABY = "DreckoBabyNavGrid",
				FLYER_1X1 = "FlyerNavGrid1x1",
				FLYER_1X2 = "FlyerNavGrid1x2",
				FLYER_2X2 = "FlyerNavGrid2x2",
				SLICKSTER = "FloaterNavGrid",
				SWIMMER = "SwimmerNavGrid",
				PIP = "SquirrelNavGrid";
		}

		public static class WORLDGEN
		{
			public static class CLUSTERS
			{
				public const string BEACHED = "clusters/AstropelagosMoonlets";
			}
		}
	}
}
