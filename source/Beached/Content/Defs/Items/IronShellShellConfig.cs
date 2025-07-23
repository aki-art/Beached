using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class IronShellShellConfig : IEntityConfig
	{
		public const string ID = "Beached_IronShellShell";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.MISC.BEACHED_IRONSHELLSHELL.NAME,
				STRINGS.ITEMS.MISC.BEACHED_IRONSHELLSHELL.DESC,
				30f,
				true,
				Assets.GetAnim("beached_ironshellshell_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.6f,
				0.4f,
				true,
				0,
				SimHashes.FoolsGold,
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
