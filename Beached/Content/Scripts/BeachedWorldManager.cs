using System;

namespace Beached.Content.Scripts
{
    public class BeachedWorldManager : KMonoBehaviour
    {
        public static BeachedWorldManager Instance { get; private set; }

        // TODO: mod settings content enable || beached world loaded
        public bool IsBeachedContentActive { get; private set; } = true;

        private bool wasOnBeachedWorld;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Game.Instance.Subscribe((int)GameHashes.SaveGameReady, OnSaveGameReady);
        }

        private void OnSaveGameReady(object obj)
        {
            IsBeachedContentActive = true; // TODO: actually detect world type

            if(IsBeachedContentActive && !wasOnBeachedWorld)
            {
                SetDbEntries(true);
                wasOnBeachedWorld = true;
            }

            Debug.Log("SAVEGAME READY");
        }

        private void SetDbEntries(bool isBeached)
        {
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
