using Beached.Content.Scripts.Entities.AI.Fua;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Fuas
{
	public class FueNestConfig : IEntityConfig
	{
		public const string ID = "Beached_FueNest";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Fua Nest",
				"<b>WORK IN PROGRESS</b>\n" +
				"\n" +
				"For now this will periodically provide some fiber, in a future update please look forward to a precious new creature to inhabit this nest.",
				30f,
				Assets.GetAnim("beached_fue_nest_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				2,
				1,
				DECOR.BONUS.TIER2);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = 1f;

			var nest = prefab.AddComponent<FuaNest>();
			nest.furPerCycle = 5f;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
