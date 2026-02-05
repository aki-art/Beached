using Beached.Content.ModDb;
using Database;

namespace Beached.Utils
{
	public class AchievementBuilder
	{
		private ColonyAchievement achievement;

		public AchievementBuilder(string id, string clusterTag = "BeachedCluster")
		{
			achievement = new ColonyAchievement(
				id,
				"",
				Strings.Get($"STRINGS.COLONY_ACHIEVEMENTS.{id.ToUpperInvariant()}.NAME"),
				Strings.Get($"STRINGS.COLONY_ACHIEVEMENTS.{id.ToUpperInvariant()}.DESCRIPTION"),
				false,
				[],
				clusterTag: clusterTag);
		}

		public AchievementBuilder ClusterTag(Tag tag)
		{
			achievement.clusterTag = tag.ToString();
			return this;
		}

		public AchievementBuilder Requirement(ColonyAchievementRequirement requirement)
		{
			achievement.requirementChecklist.Add(requirement);
			return this;
		}

		public AchievementBuilder VictoryCondition()
		{
			achievement.isVictoryCondition = true;
			return this;
		}

		public AchievementBuilder Icon(string icon)
		{
			achievement.icon = icon;
			return this;
		}

		public AchievementBuilder DLC(string[] dlcIds)
		{
			achievement.requiredDlcIds = dlcIds;
			return this;
		}

		public AchievementBuilder Build(ColonyAchievements colonyAchievements)
		{
			colonyAchievements.Add(achievement);
			BAchievements.beachedAchievements.Add(achievement.Id);
			return this;
		}
	}
}
