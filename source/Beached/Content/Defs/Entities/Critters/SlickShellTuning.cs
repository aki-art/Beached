using System.Collections.Generic;

namespace Beached.Content.Defs.Entities.Critters
{
	public class SlickShellTuning
	{
		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new()
		{
			new FertilityMonitor.BreedingChance
			{
				egg = SlickShellConfig.EGG_ID.ToTag(),
				weight = 1f
			}
		};

		public const string ON_DEATH_DROP = CrabShellConfig.ID;
		public const float MASS = 50f;
	}
}
