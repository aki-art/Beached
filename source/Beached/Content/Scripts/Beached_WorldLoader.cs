using Beached.Content.BWorldGen;

namespace Beached.Content.Scripts
{
	public class Beached_WorldLoader : KMonoBehaviour
	{
		public static Beached_WorldLoader Instance;

		public override void OnPrefabInit() => Instance = this;

		public override void OnCleanUp() => Instance = null;

		public bool IsBeachedContentActive { get; private set; } = true;

		public void WorldLoaded(string clusterId)
		{
			IsBeachedContentActive = clusterId == CONSTS.WORLDGEN.CLUSTERS.BEACHED;

			if (IsBeachedContentActive)
				Log.Info("Loaded Astropelagos world, initializing Beached settings.");

			Elements.OnWorldReload(IsBeachedContentActive);
			ZoneTypes.OnWorldLoad();
		}
	}
}
