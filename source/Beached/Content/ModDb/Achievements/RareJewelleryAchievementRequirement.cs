using Beached.Content.Scripts;
using Database;

namespace Beached.Content.ModDb.Achievements
{
	public class RareJewelleryAchievementRequirement() : ColonyAchievementRequirement
	{
		public override bool Success() => Beached_Mod.Instance.rareJewelleryObjectiveComplete;
	}
}
