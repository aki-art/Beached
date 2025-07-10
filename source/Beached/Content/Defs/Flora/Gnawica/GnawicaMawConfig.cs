using System;
using UnityEngine;

namespace Beached.Content.Defs.Flora.Gnawica
{
	public class GnawicaMawConfig : IEntityConfig
	{
		public const string ID = "Beached_GnawicaMaw";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_GNAWICAMAW.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_GNAWICAMAW.DESC,
				30f,
				Assets.GetAnim("beached_gnawica_maw_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				2,
				2,
				TUNING.DECOR.BONUS.TIER2);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = 1f;

			//var manualDeliveryKg = prefab.AddOrGet<ManualDeliveryKG>();


			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
