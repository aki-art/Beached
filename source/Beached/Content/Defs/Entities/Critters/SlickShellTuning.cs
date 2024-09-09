using System.Collections.Generic;
using TUNING;

namespace Beached.Content.Defs.Entities.Critters
{
	public class SlickShellTuning
	{
		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE =
		[
			new FertilityMonitor.BreedingChance
			{
				egg = SlickShellConfig.EGG_ID.ToTag(),
				weight = 1f
			}
		];

		public const string ON_DEATH_DROP = CrabShellConfig.ID;
		public const float MASS = 50f;
		public static float STANDARD_CALORIES_PER_CYCLE = 700000f;
		public static float STANDARD_STARVE_CYCLES = 10f;
		public static float STANDARD_STOMACH_SIZE = HatchTuning.STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;
		public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
		public static float EGG_MASS = 2f;
		public static float KG_ORE_EATEN_PER_CYCLE = 140f;
		public static float CALORIES_PER_KG_OF_ORE = STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;
		public static float MIN_POOP_SIZE_IN_KG = 25f;
	}
}
