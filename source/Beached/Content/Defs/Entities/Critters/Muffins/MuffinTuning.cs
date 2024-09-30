using System.Collections.Generic;

namespace Beached.Content.Defs.Entities.Critters.Muffins
{
	public class MuffinTuning
	{
		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE =
		[
			new FertilityMonitor.BreedingChance
			{
				egg = MuffinConfig.EGG_ID.ToTag(),
				weight = 1f
			}
		];

		public const string ON_DEATH_DROP = MeatConfig.ID;
		public const float MASS = 100f;
		public const float SPEED = 1.25f;
		public const float KCAL_PER_CYCLE = 200_000; // 1/4 meat

	}
}
