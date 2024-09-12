using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beached.Content.Scripts.StarmapEntities;
using UnityEngine;
using static KAnim;
using static Klei.ClusterLayoutSave;
using static ResearchTypes;

namespace Beached.Content.Defs.StarmapEntities
{
    /// <summary>
    /// This entity is a starmap poi that sits on hexes where the meteor swarm resides on. 
    /// it is either invisible or marks the hex with a warning symbol
    /// rockets that sit on the same tile will receive damage if no shield module is constructed
    /// </summary>
    internal class MeteorSwarmDamagePOIConfig : IEntityConfig
    {
        public static string ID = "Beached_ClusterSwarmDamagePOI";

        public GameObject CreatePrefab()
        {
            GameObject entity = EntityTemplates.CreateEntity(ID, name: ID);
            entity.AddOrGet<SaveLoadRoot>();
            var clusterGridEntity = entity.AddOrGet<InvisibleClusterGridEntity>();
            clusterGridEntity._name = "Meteor Swarm Damage";
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
