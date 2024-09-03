using Beached.Content.Scripts;
using Database;

namespace Beached.Content.ModDb.Achievements
{
	public class TallBambooAchievementRequirement(int minHeight) : ColonyAchievementRequirement
	{
		private readonly int minHeight = minHeight;

		public override string GetProgress(bool complete) => $"{Beached_Mod.Instance.tallestBambooGrown}/{minHeight}";

		public override bool Success() => Beached_Mod.Instance.tallestBambooGrown >= minHeight;
	}
}
