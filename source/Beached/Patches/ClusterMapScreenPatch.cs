using Beached.Content.Defs.StarmapEntities;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.StarmapEntities;
using HarmonyLib;
using System.Linq;
using static LogicGateVisualizer;

namespace Beached.Patches
{
	public class ClusterMapScreenPatch
	{
		[HarmonyPatch(typeof(ClusterMapScreen), "SetupVisGameObjects")]
		public class ClusterMapScreen_SetupVisGameObjects_Patch
		{
			[HarmonyPrepare]
			public static bool Prepare()=> DlcManager.IsExpansion1Active();

			public static void Postfix(ClusterMapScreen __instance)
			{
				foreach(var entityGroup in ClusterGrid.Instance.cellContents)
				{
					foreach(var entity in entityGroup.Value)
					{
						if(entity.TryGetComponent<SwarmVisualizer>(out var visualizer))
						{
							visualizer.SpawnVisualizers();
							return;
						}
					}
				}

				foreach (var vis in __instance.m_gridEntityVis)
				{
					//note: this is only found if the MeteorSwarmVisualPOI is uncovered!
					if (vis.Key.IsPrefabID(MeteorSwarmVisualPOIConfig.ID))
					{
						Log.Debug("ADDED ASTEROID BELT VIS");
						//vis.Value.gameObject.AddOrGet<AsteroidBeltVisualizer>();
					}
				}
			}
		}
	}
}
