using System.Collections.Generic;

namespace Beached
{
	public class ModTuning
	{
		public static int standardLubricantUses = 36;

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
