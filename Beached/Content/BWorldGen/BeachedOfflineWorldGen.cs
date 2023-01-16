using HarmonyLib;
using Klei.CustomSettings;

namespace Beached.Content.BWorldGen
{
    public class BeachedOfflineWorldGen : KMonoBehaviour
    {
        [MyCmpReq]
        private OfflineWorldGen offlineWorldGen;

        private AccessTools.FieldRef<OfflineWorldGen, int> seed;

        public int Seed
        {
            get => seed != null ? seed(offlineWorldGen) : 0;
            set => seed(offlineWorldGen) = value;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            seed = AccessTools.FieldRefAccess<OfflineWorldGen, int>("seed");
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
            Seed = int.Parse(seedSetting.id);
        }
    }
}
