using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Equipment;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class SkeletonChairConfig : IEntityConfig
	{
		public const string ID = "Beached_SkeletonChair";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.ENTITIES.BEACHED_SKELETON_CHAIR.NAME,
				STRINGS.ENTITIES.BEACHED_SKELETON_CHAIR.DESCRIPTION,
				400 + 30 + 2,
				Assets.GetAnim("beached_skeletonchair_kanim"),
				"idle",
				Grid.SceneLayer.Building,
				2,
				3,
				TUNING.DECOR.NONE,
				defaultTemperature: MiscUtil.CelsiusToKelvin(30));

			prefab.AddOrGet<Demolishable>();
			//prefab.AddOrGet<POITechItemUnlockWorkable>().workTime = 5f;

			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];

			var unlockWorkable = prefab.AddOrGet<GenericUnlockablePOIWorkable>();
			unlockWorkable.workTime = 5f;
			unlockWorkable.overrideAnims = [Assets.GetAnim("anim_interacts_clothingfactory_kanim")];
			unlockWorkable.synchronizeAnims = false;

			var unlocks = prefab.AddOrGetDef<GenericUnlockablePOI.Def>();
			unlocks.techUnlockIDs =
				[
					AquaticFarmTileConfig.ID,
					WaterCoolerConfig.ID,
					BeachChairConfig.ID
				];

			unlocks.animName = "beached_skeletonunlock_kanim";
			unlocks.popUpName = STRINGS.BEACHED.UI.SKELETON_POI_POPUP.TITLE;
			unlocks.messageBody = STRINGS.BEACHED.UI.SKELETON_POI_POPUP.BODY;
			unlocks.spawnPrefabs = [BeachShirtConfig.ID, Elements.bone.ToString()];
			unlocks.onSpawnFn += spawnedGO =>
			{
				if (spawnedGO.IsPrefabID(BeachShirtConfig.ID) && spawnedGO.TryGetComponent(out Equippable equippable))
					EquippableFacade.AddFacadeToEquippable(equippable, BEquippableFacades.BEACHSHIRTS.GREEN);
			};

			prefab.AddOrGet<Prioritizable>();

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst) { }
	}
}
