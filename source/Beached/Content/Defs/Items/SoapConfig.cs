using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class SoapConfig : IEntityConfig
	{
		public const string ID = "Beached_Soap";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.MISC.BEACHED_SOAP.NAME,
				STRINGS.ITEMS.MISC.BEACHED_SOAP.DESC,
				10f,
				false,
				Assets.GetAnim("beached_soap_kanim"),
				"object",
				Grid.SceneLayer.Ore,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.5f,
				0.4f,
				true);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
