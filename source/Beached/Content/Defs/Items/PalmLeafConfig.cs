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
				1f,
				false,
				Assets.GetAnim("beached_palmleaf_kanim"),
				"object",
				Grid.SceneLayer.Ore,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true,
				additionalTags: [GameTags.IndustrialIngredient]);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
