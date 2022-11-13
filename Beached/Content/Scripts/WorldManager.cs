namespace Beached.Content.Scripts
{
    public class WorldManager : KMonoBehaviour
    {
        public static WorldManager Instance { get; private set; }

        public bool IsBeached { get; private set; }

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
            IsBeached = true;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
