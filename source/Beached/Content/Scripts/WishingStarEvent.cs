using System.Collections.Generic;

namespace Beached.Content.Scripts
{
    public class WishingStarEvent : KMonoBehaviour
    {
        public HashSet<int> activeWorlds = new();

        public bool DoesWorldHaveShooties(int worldId) => activeWorlds.Contains(worldId);

        public override void OnSpawn()
        {
            base.OnSpawn();
            Game.Instance.Subscribe((int)GameHashes.MeteorShowerBombardStateBegins, OnBombardmentStart);
            Game.Instance.Subscribe((int)GameHashes.MeteorShowerBombardStateEnds, OnBombardmentEnd);

            foreach (var activeEvent in GameplayEventManager.Instance.activeEvents)
            {
                if (activeEvent.tags.Contains(BTags.wishingStars))
                {
                    activeWorlds.Add(activeEvent.worldId);
                }
            }
        }

        private void OnBombardmentStart(object data)
        {
            if (data is int worldId)
            {
                RunIfWishingStarsActive(id => activeWorlds.Add(id), worldId);
                Trigger(ModHashes.wishingStarEvent, worldId);
            }
        }

        private void OnBombardmentEnd(object data)
        {
            if (data is int worldId)
            {
                RunIfWishingStarsActive(id => activeWorlds.Remove(id), worldId);
                Trigger(ModHashes.wishingStarEvent, worldId);
            }
        }

        public static void RunIfWishingStarsActive(System.Action<int> fn, int worldId)
        {
            foreach (var activeEvent in GameplayEventManager.Instance.activeEvents)
            {
                if (activeEvent.worldId == worldId && activeEvent.tags.Contains(BTags.wishingStars))
                {
                    fn(worldId);
                    return;
                }
            }
        }
    }
}
