using UnityEngine;

namespace Beached.Content.Defs.Items
{
	internal class PalmLeafConfig : IEntityConfig
	{
		public const string ID = "Beached_PalmLeaf";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.MISC.BEACHED_PALMLEAF.NAME,
				STRINGS.ITEMS.MISC.BEACHED_PALMLEAF.DESC,
				6f,
				false,
				Assets.GetAnim("beached_palmleaf_kanim"),
				"object",
				Grid.SceneLayer.Ore,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
