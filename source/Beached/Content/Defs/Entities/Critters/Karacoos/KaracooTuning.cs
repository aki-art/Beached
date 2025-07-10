namespace Beached.Content.Defs.Entities.Critters.Karacoos
{
	public static class KaracooTuning
	{
		public static float STANDARD_STARVE_CYCLES = 3f;
		public static float STANDARD_CALORIES_PER_CYCLE = 100_000f;
		public static float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;
		public static float KG_ORE_EATEN_PER_CYCLE = 100f;
		public static float CALORIES_PER_KG_OF_ORE = STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE; // 3333.33
		public static float MIN_POOP_SIZE_IN_KG = 5f;
	}
}
