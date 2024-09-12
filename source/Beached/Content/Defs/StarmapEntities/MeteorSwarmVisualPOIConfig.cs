using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beached.Content.Defs.StarmapEntities
{
    /// <summary>
    /// This entity handles the actual animation of the meteor swarm, it is not selectable on the starmap
    /// </summary>
    internal class MeteorSwarmVisualPOIConfig : IEntityConfig
    {
        public static string ID = "Beached_ClusterSwarmVisualPOI";
        public GameObject CreatePrefab()
        {
            GameObject entity = EntityTemplates.CreateEntity(ID, name: ID);
            entity.AddOrGet<SaveLoadRoot>();
            var clusterGridEntity = entity.AddOrGet<InvisibleClusterGridEntity>();
            clusterGridEntity._name = "Meteor Swarm Visualizer";
            return entity;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
