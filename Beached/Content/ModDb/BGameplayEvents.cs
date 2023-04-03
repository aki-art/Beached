using Beached.Content.Defs.Comets;
using Database;
using Klei.AI;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.ModDb
{
    internal class BGameplayEvents
    {
        public const string DIAMOND_SHOWER = "Beached_ClusterDiamondShowerEvent";
        public static GameplayEvent ClusterDiamondShower;
        public const string ABYSSALITE_SHOWER = "Beached_ClusterAbyssaliteShowerEvent";
        public static GameplayEvent ClusterAbyssaliteShower;

        public static void Register(GameplayEvents gameplayEvents)
        {
            ClusterDiamondShower = gameplayEvents.Add(new MeteorShowerEvent(
                DIAMOND_SHOWER,
                300f,
                3.5f,
                METEORS.BOMBARDMENT_OFF.NONE,
                METEORS.BOMBARDMENT_ON.UNLIMITED,
                ClusterMapMeteorShowerConfig.GetFullID(DIAMOND_SHOWER),
                true)
                .AddMeteor(DiamondCometConfig.ID, 1f)
                .AddMeteor(SparklingZirconCometConfig.ID, 0.001f)
                .AddMeteor(SparklingAquamarineCometConfig.ID, 0.001f)
                .AddMeteor(SparklingVoidCometConfig.ID, 0.001f));

            ClusterDiamondShower.tags.Add(BTags.wishingStars);

            ClusterAbyssaliteShower = gameplayEvents.Add(new MeteorShowerEvent(
                ABYSSALITE_SHOWER,
                300f,
                3.5f,
                METEORS.BOMBARDMENT_OFF.NONE,
                METEORS.BOMBARDMENT_ON.UNLIMITED,
                ClusterMapMeteorShowerConfig.GetFullID(ABYSSALITE_SHOWER),
                true)
                .AddMeteor(AbyssaliteCometConfig.ID, 1f));

        }
    }
}
