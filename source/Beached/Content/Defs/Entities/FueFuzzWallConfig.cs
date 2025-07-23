using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class FueFuzzWallConfig : IEntityConfig
	{
		public const string ID = "Beached_FueFuzzWall";

		public GameObject CreatePrefab()
		{
			return EntityTemplates.CreatePlacedEntity(
				ID,
				"Fue Fuzz Wall",
				"",
				10,
				Assets.GetAnim("beached_fue_fuzz_wall_kanim"),
				"idle",
				Grid.SceneLayer.Backwall,
				1,
				1,
				DECOR.BONUS.TIER0);
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
