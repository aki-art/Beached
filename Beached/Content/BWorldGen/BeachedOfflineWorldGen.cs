using HarmonyLib;
using Klei.CustomSettings;

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
            var clusterSetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
            var clusterId = clusterSetting.id;

            if(clusterId == Consts.CLUSTERS.BEACHED)
            {
                InitializeMyWorldGen();

                return false;
            }

            return true;
        }

        private void InitializeMyWorldGen()
        {
            var seedSetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
            offlineWorldGen.seed = int.Parse(seedSetting.id);
        }
    }
}
