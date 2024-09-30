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
				"Fue Nest",
				"",
				30f,
				Assets.GetAnim("beached_fue_nest_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				2,
				1,
				DECOR.BONUS.TIER2);

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
