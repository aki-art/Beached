using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class SealedStorageConfig : IBuildingConfig
	{
		public const string ID = "Beached_SealedStorage";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"beached_sealed_storage_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				[200f, 25f],
				[MATERIALS.BUILDABLERAW, BTags.BuildingMaterials.rubber.ToString()],
				BUILDINGS.MELTING_POINT_KELVIN.TIER2,
				BuildLocationRule.OnFloor,
				BUILDINGS.DECOR.PENALTY.TIER0,
				NOISE_POLLUTION.NONE);

			def.Floodable = false;
			def.AudioCategory = AUDIO.CATEGORY.METAL;
			def.Overheatable = false;
			def.AddSearchTerms(global::STRINGS.SEARCH_TERMS.STORAGE);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);

			var storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = true;
			storage.showDescriptor = true;
			storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			storage.showCapacityStatusItem = true;
			storage.showCapacityAsMainStatus = true;
			storage.capacityKg = 15_000f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
			go.AddOrGet<StorageLocker>();
			go.AddOrGet<UserNameable>();
			go.AddOrGetDef<RocketUsageRestriction.Def>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
		}
	}
}
