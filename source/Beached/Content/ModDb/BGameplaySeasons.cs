using Database;
using Klei.AI;

namespace Beached.Content.ModDb
{
	internal class BGameplaySeasons
	{
		public const string ASTROPELAGOS = "Beached_AstropelagosMoonletMeteorShowers";
		public static GameplaySeason astropelagosMoonletMeteorShowers;

		[DbEntry]
		public static void Register(GameplaySeasons __instance)
		{
			astropelagosMoonletMeteorShowers = __instance.Add(new MeteorShowerSeason(
				ASTROPELAGOS,
				GameplaySeason.Type.World,
				DlcManager.EXPANSION1_ID,
				20f,
				false,
				-1f,
				true,
				-1,
				0f,
				float.PositiveInfinity,
				1,
				true,
				300f)
				.AddEvent(BGameplayEvents.ClusterDiamondShower));
			//.AddEvent(BGameplayEvents.ClusterAbyssaliteShower);
		}
	}
}
