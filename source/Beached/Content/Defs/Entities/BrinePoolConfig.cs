using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class BrinePoolConfig : IEntityConfig
	{
		public const string ID = "Beached_BrinePool";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.OTHERS.BEACHED_BRINE_POOL.NAME,
				STRINGS.CREATURES.SPECIES.OTHERS.BEACHED_BRINE_POOL.DESC,
				100f,
				Assets.GetAnim("beached_brinepool_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				3,
				1,
				DECOR.NONE);

			var brinePool = prefab.AddComponent<BrinePool>();
			brinePool.saltingCooldownS = 30;
			brinePool.saltingFrequencyS = new(1, 30);
			brinePool.cellOffset = new CellOffset(0, 0);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
