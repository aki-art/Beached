using Beached.Content.Defs.Foods;
using System.Collections.Generic;

namespace Beached.Content.Defs.Entities.Critters
{
	public class JellyfishTuning
	{
		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new()
		{
			new FertilityMonitor.BreedingChance
			{
				egg = JellyfishConfig.EGG_ID.ToTag(),
				weight = 1f
			}
		};

		public const string ON_DEATH_DROP = JellyConfig.ID;
		public const float MASS = 10f;
	}
}
