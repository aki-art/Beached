using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class WreckageNoseConfig : IEntityConfig
	{
		public const string ID = "Beached_WreackageNose";
		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Wreckage",
				"...",
				400,
				Assets.GetAnim("beached_wreckage_kanim"),
				"cone",
				Grid.SceneLayer.BuildingBack,
				3,
				4,
				DECOR.NONE,
				NOISE_POLLUTION.NONE,
				SimHashes.Rust,

				null,
				MiscUtil.CelsiusToKelvin(30));

			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];

			return prefab;
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
