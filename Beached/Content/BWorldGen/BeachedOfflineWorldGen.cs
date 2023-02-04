using Klei.CustomSettings;
using ProcGenGame;
using System.Collections.Generic;

namespace Beached.Content.BWorldGen
{
    public class BeachedOfflineWorldGen : KMonoBehaviour
    {
        [MyCmpReq]
        private OfflineWorldGen offlineWorldGen;

        private Cluster cluster;

        public static bool isDoingBeachedWorldgen;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        public bool OnDoWorldGenInitialize()
        {
            return true; 

            var clusterLayout = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);

            if (clusterLayout.id == Consts.CLUSTERS.BEACHED)
            {
                isDoingBeachedWorldgen = true;
                InitializeMyWorldGen(clusterLayout);
                return false;
            }

            isDoingBeachedWorldgen = false;
            return true;
        }

        private void InitializeMyWorldGen(SettingLevel clusterLayout)
        {
            Log.Debug("Initializing Beached worldgen");

            offlineWorldGen.debug = true;

            var seedSetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
            var clusterId = clusterLayout.id;
            offlineWorldGen.seed = 0; //            int.Parse(seedSetting.id);

            Log.Debug($"Set seed to {offlineWorldGen.seed}");

            offlineWorldGen.clusterLayout = cluster = new Cluster(
                clusterId,
                offlineWorldGen.seed,
                new List<string>(),
                true,
                false);

            cluster.Generate(
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
