using Beached.Content.Defs.Comets;
using Database;
using Klei.AI;
using TUNING;

namespace Beached.Content.ModDb
{
    internal class BGameplayEvents
    {
        public const string DIAMOND_SHOWER = "Beached_ClusterDiamondShowerEvent";
        public static GameplayEvent ClusterDiamondShower;

        public static void Register(GameplayEvents gameplayEvents)
        {
            var fullID = ClusterMapMeteorShowerConfig.GetFullID(DIAMOND_SHOWER);
            ClusterDiamondShower = gameplayEvents.Add(new MeteorShowerEvent(
                DIAMOND_SHOWER,
                300f,
                3.5f,
                METEORS.BOMBARDMENT_OFF.NONE,
                METEORS.BOMBARDMENT_ON.UNLIMITED,
                fullID,
                true)
                .AddMeteor(DiamondCometConfig.ID, 1f));
        }
    }
}
