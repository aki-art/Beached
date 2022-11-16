using System;

namespace Beached.Content.Scripts
{
    public class GrowingCrystal : KMonoBehaviour
    {
        [MyCmpReq]
        private CrystalFoundationMonitor crystalFoundationMonitor;

        [MyCmpReq]
        private KSelectable kSelectable;

        private static StatusItem test = new StatusItem("Beached_Growing", "Growing", "", StatusItem.IconType.Info, NotificationType.Good, false, new HashedString(129022));

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Subscribe((int)GameHashes.FoundationChanged, OnFoundationChanged);

            //Log.Debug("HASH " + HashCache.Get().Get(new HashedString(129022)).ToString());
        }

        private void OnFoundationChanged(object obj)
        {
            if(!crystalFoundationMonitor.hasFoundation)
            {
                // shatter
                Util.KDestroyGameObject(this);
            }

            kSelectable.ToggleStatusItem(test, crystalFoundationMonitor.hasMatchingFoundation);
        }
    }
}
