using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.Scripts.ClassExtensions
{
	public class MinionStartingStatsExtension
	{
		public Trait lifeGoalTrait;
		public Dictionary<string, int> lifeGoalAttributes = [];
		public string testValue;
		private MinionStartingStats original;

		public MinionStartingStatsExtension(MinionStartingStats original)
		{
			this.original = original;
		}

		public MinionStartingStatsExtension SetLifeGoal(Trait goal)
		{
			lifeGoalTrait = goal;
			lifeGoalAttributes = [];

			return this;
		}

		public MinionStartingStatsExtension AddLifeGoalReward(string attributeId, int amount)
		{
			lifeGoalAttributes[attributeId] = amount;
			return this;
		}
	}
}
