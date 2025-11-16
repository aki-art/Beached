using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class SlickShellShellConfig : IEntityConfig
	{
		public const string ID = "Beached_SlickShellShell";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.MISC.BEACHED_SLICKSHELLSHELL.NAME,
				STRINGS.ITEMS.MISC.BEACHED_SLICKSHELLSHELL.DESC,
				10f,
				true,
				Assets.GetAnim("beached_slickshellshell_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.6f,
				0.4f,
				true,
				0,
				SimHashes.Lime,
				[
					GameTags.Organics,
					BTags.BuildingMaterials.chime
				]);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
