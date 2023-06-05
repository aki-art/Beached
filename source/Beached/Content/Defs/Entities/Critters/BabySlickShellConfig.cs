using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	internal class BabySlickShellConfig : IEntityConfig
	{
		public const string ID = "Beached_SlickShell_Baby";

		public GameObject CreatePrefab()
		{
			var prefab = BaseSnailConfig.CreatePrefab(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_SLICKSHELL.BABY_NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_SLICKSHELL.BABY_DESC,
				"beached_snail_kanim",
				SlickShellConfig.BASE_TRAIT_ID);

			EntityTemplates.ExtendEntityToBeingABaby(prefab, SlickShellConfig.ID, null);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
