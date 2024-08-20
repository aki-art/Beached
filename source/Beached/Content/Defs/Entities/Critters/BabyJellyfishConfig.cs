using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	[EntityConfigOrder(100)]
	public class BabyJellyfishConfig : IEntityConfig
	{
		public const string ID = "Beached_Jellyfish_Baby";

		public GameObject CreatePrefab()
		{
			var prefab = BaseSnailConfig.CreatePrefab(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_JELLYFISH.BABY_NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_JELLYFISH.BABY_DESC,
				"beached_jellyfish_kanim",
				JellyfishConfig.BASE_TRAIT_ID,
				[]);

			EntityTemplates.ExtendEntityToBeingABaby(prefab, JellyfishConfig.ID, null);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst)
		{
			if (inst.TryGetComponent(out KBatchedAnimController kbac))
			{
				kbac.animScale *= 0.5f; // TODO: custom anim
			}
		}
	}
}
