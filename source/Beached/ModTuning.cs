using System.Collections.Generic;

namespace Beached
{
	public class ModTuning
	{
		public static int standardLubricantUses = 36;

		public static class Karacoo
		{
			public static float STANDARD_STARVE_CYCLES = 3f;
			public static float STANDARD_CALORIES_PER_CYCLE = 100_000f;
			public static float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;
			public static float KG_ORE_EATEN_PER_CYCLE = 100f;
			public static float CALORIES_PER_KG_OF_ORE = STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE; // 3333.33
			public static float MIN_POOP_SIZE_IN_KG = 5f;
		}

		public static class DewPalm
		{
			// must not overlap main body, 0-1, 0-2, 0-3
			public static Dictionary<string, CellOffset> Leafs() => new()
			{
				{"a", new CellOffset(-1, +3) },
				{"b", new CellOffset(-1, +4) },
				{"c", new CellOffset(0, +4) },
				{"d", new CellOffset(1, +3) },
				{"e", new CellOffset(1, +2) },
				{"f", new CellOffset(1, +1) },
			};
		}
	}
}
