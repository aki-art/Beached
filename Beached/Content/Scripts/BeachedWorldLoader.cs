using Beached.Content.BWorldGen;
using Klei.CustomSettings;
using Neutronium.PostProcessing.LUT;
using System.Collections.Generic;
using System.IO;
using static Beached.ModAssets;

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
            IsBeachedContentActive = clusterId == CONSTS.WORLDGEN.CLUSTERS.BEACHED;

            if (IsBeachedContentActive)
            {
                Log.Info("Loaded Astropelagos world, initializing Beached settings.");
            }

            Elements.OnWorldReload(IsBeachedContentActive);
        }
    }
}
