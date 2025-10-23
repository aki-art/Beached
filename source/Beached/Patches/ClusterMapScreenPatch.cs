using Beached.Content.Defs.StarmapEntities;
using Beached.Content.Scripts.Entities;

namespace Beached.Patches
{
	public class ClusterMapScreenPatch
	{
		//[HarmonyPatch(typeof(ClusterMapScreen), "SetupVisGameObjects")]
		public class ClusterMapScreen_SetupVisGameObjects_Patch
		{
			public static void Postfix(ClusterMapScreen __instance)
			{
				foreach (var vis in __instance.m_gridEntityVis)
				{
					if (vis.Key.IsPrefabID(MeteorSwarmVisualPOIConfig.ID))
					{
						Log.Debug("ADDED ASTEROID BELT VIS");
						vis.Value.gameObject.AddOrGet<AsteroidBeltVisualizer>();
					}
				}
			}
		}
	}
}
