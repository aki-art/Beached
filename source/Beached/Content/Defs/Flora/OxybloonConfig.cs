using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.Plant;
using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class OxybloonConfig : IEntityConfig
	{
		public const string ID = "Beached_Oxybloon";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_OXYBLOON.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_OXYBLOON.DESC,
				50f,
				Assets.GetAnim("beached_oxybloon_kanim"),
				"idle",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.PENALTY.TIER1);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];
			prefab.AddOrGet<EntombVulnerable>();
			prefab.AddOrGet<Prioritizable>();

			var uprootable = prefab.AddOrGet<UprootableWithDramaticDeath>();
			uprootable.deathAnimation = "harvest";
			uprootable.multitoolHitEffectTag = "fx_harvest_splash";
			uprootable.multitoolContext = "harvest";

			prefab.AddOrGet<UprootedMonitor>();
			prefab.AddOrGet<Harvestable>();
			prefab.AddOrGet<HarvestDesignatable>();

			var oxybloon = prefab.AddOrGet<Oxybloon>();
			oxybloon.elementId = SimHashes.Oxygen;

			var storage = prefab.AddComponent<Storage>();
			storage.storageFilters = STORAGEFILTERS.GASES;
			storage.showInUI = true;

			prefab.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}