namespace Beached.Content.Scripts
{
    public class BeachedWorldLoader : KMonoBehaviour
    {
        public static BeachedWorldLoader Instance;

        public override void OnPrefabInit() => Instance = this;

        public override void OnCleanUp() => Instance = null;

        public bool IsBeachedContentActive { get; private set; } = true;

        public void WorldLoaded(string clusterId)
        {
            IsBeachedContentActive = clusterId == "expansion1::clusters/TinyStartCluster";
            Elements.OnWorldReload(IsBeachedContentActive);
        }
    }
}
