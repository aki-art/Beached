using Beached.Content.Defs.Foods;
using System.Collections.Generic;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	public class JellyfishTuning
	{
		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE =
		[
			new FertilityMonitor.BreedingChance
			{
				egg = JellyfishConfig.EGG_ID.ToTag(),
				weight = 1f
			}
		];

		public const string ON_DEATH_DROP = JellyConfig.ID;
		public const float MASS = 10f;

		public static float STANDARD_STARVE_CYCLES = 3f;
		public static float STANDARD_CALORIES_PER_CYCLE = 100_000f;
		public static float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;
		public static float GERMS_EATEN_PER_CYCLE = 5_000f;
		public static float SLIME_PER_CYCLE = 200f;
		public static float CONVERSION_RATE = SLIME_PER_CYCLE / GERMS_EATEN_PER_CYCLE;
		public static float CALORIES_PER_GERM = STANDARD_CALORIES_PER_CYCLE / GERMS_EATEN_PER_CYCLE;
	}
}
