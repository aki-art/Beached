using Beached.Content.Defs.Foods;
using Beached.Content.ModDb.Achievements;
using Database;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class BAchievements
	{
		public static HashSet<string> beachedAchievements = [];

		[DbEntry]
		public static void Register(ColonyAchievements __instance)
		{
			new AchievementBuilder("Beached_TallBamboo")
				.Requirement(new TallBambooAchievementRequirement(128))
				.Build(__instance);

			new AchievementBuilder("Beached_Blinged")
				.Requirement(new RareJewelleryAchievementRequirement())
				.Build(__instance);

			new AchievementBuilder("Beached_LegendarySteak")
				.Requirement(new EatXCaloriesFromY(1, [LegendarySteakConfig.ID]))
				.Build(__instance);
		}
	}
}
