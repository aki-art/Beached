using Database;
using Klei.AI;

namespace Beached.Content.ModDb
{
    internal class BGameplaySeasons
    {
        public const string ASTROPELAGOS = "Beached_AstropelagosMoonletMeteorShowers";
        public static GameplaySeason astropelagosMoonletMeteorShowers;

        public static void Register(GameplaySeasons gameplaySeasons)
        {
            astropelagosMoonletMeteorShowers = gameplaySeasons.Add(new MeteorShowerSeason(
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
                600f)
                //.AddEvent(Db.Get().GameplayEvents.ClusterIceShower)
                .AddEvent(BGameplayEvents.ClusterDiamondShower));
                //.AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
        }
    }
}
