using Beached.Content.ModDb.Germs;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	// serves as informational placeholder for plankton germ diet
	public class UIOnlyPlanktonConfig : IEntityConfig
	{
		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				PlanktonGerms.ID,
				STRINGS.DUPLICANTS.DISEASES.BEACHED_PLANKTON.NAME,
				STRINGS.DUPLICANTS.DISEASES.BEACHED_PLANKTON.DESCRIPTION,
				1f,
				false,
				Assets.GetAnim("beached_plankton_ui_kanim"),
				"ui",
				Grid.SceneLayer.Creatures,
				additionalTags: [BTags.OniTwitch_surpriseBoxForceDisabled]);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			inst.GetComponent<InfoDescription>().description = "I'm just a humble placeholder prefab, I shouldn't be spawned";

			var testQuad = Object.Instantiate(ModAssets.Prefabs.testQuad);
			testQuad.SetActive(true);
			testQuad.transform.position = inst.transform.position;
			testQuad.transform.parent = inst.transform;
			testQuad.GetComponent<MeshRenderer>().material.renderQueue = RenderQueues.Liquid;
		}
	}
}
