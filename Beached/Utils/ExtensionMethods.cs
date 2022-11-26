using Beached.Content.Scripts.ClassExtensions;
using Klei.AI;
using System.Collections.Generic;

namespace Beached.Utils
{
    public static class ExtensionMethods
    {
        public static Dictionary<MinionStartingStats, MinionStartingStatsExtension> minionStartingStatsExtensions = new();

        public static MinionStartingStatsExtension GetExtension(this MinionStartingStats stats)
        {
            if(!minionStartingStatsExtensions.ContainsKey(stats))
            {
                minionStartingStatsExtensions.Add(stats, new MinionStartingStatsExtension(stats));
            }

            return minionStartingStatsExtensions[stats];
        }

        public static Trait GetLifeGoalTrait(this MinionStartingStats stats) => GetExtension(stats)?.lifeGoalTrait;

        public static Dictionary<string, int> GetLifeGoalAttributes(this MinionStartingStats stats) => GetExtension(stats).lifeGoalAttributes;
    }
}
