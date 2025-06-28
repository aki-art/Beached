using Beached.Content.ModDb.Germs;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	// serves as informational placeholder for limpet shearing codex entries
	public class UIOnlyLimpetConfig : IEntityConfig
	{
		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				LimpetEggGerms.ID,
				STRINGS.DUPLICANTS.DISEASES.BEACHED_LIMPETEGG.NAME,
				STRINGS.DUPLICANTS.DISEASES.BEACHED_LIMPETEGG.DESC,
				1f,
				false,
				Assets.GetAnim("beached_limpetegg_ui_kanim"),
				"ui",
				Grid.SceneLayer.Creatures,
				additionalTags: [BTags.OniTwitch_surpriseBoxForceDisabled]);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			inst.GetComponent<InfoDescription>().description = "I'm just a humble placeholder prefab, I shouldn't be spawned";
		}
	}
}
