using Klei.CustomSettings;
using ProcGenGame;
using System.Collections.Generic;

namespace Beached.Content.BWorldGen
{
    public class BeachedOfflineWorldGen : KMonoBehaviour
    {
        [MyCmpReq]
        private OfflineWorldGen offlineWorldGen;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        public bool OnDoWorldGenInitialize()
        {
            var clusterLayout = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);

            if (clusterLayout.id == Consts.CLUSTERS.BEACHED)
            {
                InitializeMyWorldGen(clusterLayout);
                return false;
            }

            return true;
        }

        private void InitializeMyWorldGen(SettingLevel clusterLayout)
        {
            Log.Debug("Initializing Beached worldgen");

            var seedSetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
            var clusterId = clusterLayout.id;
            var storyTraits = new List<string>(CustomGameSettings.Instance.GetCurrentStories());
            offlineWorldGen.seed = int.Parse(seedSetting.id);

            Log.Debug($"Set seed to {offlineWorldGen.seed}");

            offlineWorldGen.clusterLayout = new Cluster(
                clusterId,
                offlineWorldGen.seed,
                storyTraits,
                true,
                false);

            offlineWorldGen.clusterLayout.Generate(
                new WorldGen.OfflineCallbackFunction(offlineWorldGen.UpdateProgress),
                offlineWorldGen.OnError,
                offlineWorldGen.seed,
                offlineWorldGen.seed,
                offlineWorldGen.seed,
                offlineWorldGen.seed,
                true,
                true);
        }
    }
}
