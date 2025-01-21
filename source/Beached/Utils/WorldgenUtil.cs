using Beached.Content.BWorldGen;
using Klei.CustomSettings;
using ProcGen;

namespace Beached.Utils
{
	public class WorldgenUtil
	{
		public static bool IsCurrentBeachedWorld()
		{
			var currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);

			if (SettingsCache.clusterLayouts == null)
			{
				Log.Warning("Trying to fetch cluster data but clusters are not loaded yet!");
				return currentQualitySetting.id.Contains(CONSTS.BEACHED_CLUSTER_SETTING_ID);
			}

			var clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
			if (clusterData == null)
				return false;

			return clusterData.clusterTags.Contains(BWorldGenTags.BeachedCluster.ToString());
		}

		public static bool IsBeachedWorld(string clusterId)
		{
			if (SettingsCache.clusterLayouts == null)
			{
				Log.Warning("Trying to fetch cluster data but clusters are not loaded yet!");
				return clusterId.Contains(CONSTS.BEACHED_CLUSTER_SETTING_ID);
			}

			var clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
			if (clusterData == null)
				return false;

			return clusterData.clusterTags.Contains(BWorldGenTags.BeachedCluster.ToString());
		}
	}
}
