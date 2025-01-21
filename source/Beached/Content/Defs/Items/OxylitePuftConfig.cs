using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class OxylitePuftConfig : IEntityConfig
	{
		public const string ID = "Beached_OxylitePuft";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.MISC.BEACHED_OXYLITEPUFT.NAME,
				STRINGS.ITEMS.MISC.BEACHED_OXYLITEPUFT.DESC,
				400f,
				false,
				Assets.GetAnim("beached_oxylitepuft_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.9f,
				0.9f,
				true,
				0,
				SimHashes.OxyRock,
				[
					GameTags.Organics,
					BTags.BuildingMaterials.chime
				]);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
