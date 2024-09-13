using Beached.Content.Scripts.StarmapEntities;
using UnityEngine;

namespace Beached.Content.Defs.StarmapEntities
{
	/// <summary>
	/// This entity is a starmap poi that sits on hexes where the meteor swarm resides on. 
	/// it is either invisible or marks the hex with a warning symbol
	/// rockets that sit on the same tile will receive damage if no shield module is constructed
	/// </summary>
	public class MeteorSwarmDamagePOIConfig : IEntityConfig
	{
		public static string ID = "Beached_ClusterSwarmDamagePOI";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, name: ID);
			prefab.AddOrGet<SaveLoadRoot>();

			var clusterGridEntity = prefab.AddOrGet<InvisibleClusterGridEntity>();
			clusterGridEntity._name = "Meteor Swarm Damage";

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
