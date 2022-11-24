namespace Beached.Content.Scripts
{
    public class BeachedWorldManager : KMonoBehaviour
    {
        public static BeachedWorldManager Instance { get; private set; }

        public void WorldLoaded(string clusterId)
        {
            Log.Debug("WORLD HAS LOADED " + clusterId);

            IsBeachedContentActive = clusterId == "expansion1::clusters/TinyStartCluster";

            Elements.OnWorldReload(IsBeachedContentActive);
            //ElementInteractions.Instance.enabled = IsBeachedContentActive || Mod.Settings.CrossWorld.Elements.ElementInteractions;
            
        }

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
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
