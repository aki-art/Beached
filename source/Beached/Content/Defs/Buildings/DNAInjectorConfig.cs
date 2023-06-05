using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class DNAInjectorConfig : IBuildingConfig
	{
		public const string ID = "Beached_DNAInjector";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				3,
				"beached_dnainjector_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.REFINED_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				NOISE_POLLUTION.NONE);

			def.AudioCategory = AUDIO.CATEGORY.METAL;
			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 120f;
			def.ExhaustKilowattsWhenActive = 0.25f;
			def.SelfHeatKilowattsWhenActive = 2f;
			def.OverheatTemperature = 363.15f;
			def.SceneLayer = Grid.SceneLayer.Building;
			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var treeFilterable = go.AddOrGet<TreeFilterable>();
			treeFilterable.uiHeight = TreeFilterable.UISideScreenHeight.Short;

			var flatFilterable = go.AddOrGet<SimpleFlatFilterable>();
			flatFilterable.headerText = "test";

			var eggStorage = go.AddComponent<Storage>();
			eggStorage.showInUI = true;
			eggStorage.showDescriptor = true;
			eggStorage.storageFilters = new List<Tag>()
			{
				GameTags.Egg
			};
			eggStorage.allowItemRemoval = false;
			eggStorage.capacityKg = 1f;
			eggStorage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			eggStorage.fetchCategory = Storage.FetchCategory.Building;
			eggStorage.showCapacityStatusItem = true;
			eggStorage.allowSettingOnlyFetchMarkedItems = false;
			eggStorage.showSideScreenTitleBar = true;

			var sampleStorage = go.AddComponent<Storage>();
			sampleStorage.showInUI = true;
			sampleStorage.showDescriptor = true;
			sampleStorage.dropOnLoad = false;
			sampleStorage.capacityKg = 1f;
			sampleStorage.allowItemRemoval = false;
			sampleStorage.fetchCategory = Storage.FetchCategory.Building;
			sampleStorage.showCapacityStatusItem = true;
			sampleStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

			var sampleDelivery = go.AddOrGet<ManualDeliveryKG>();
			sampleDelivery.SetStorage(sampleStorage);
			sampleDelivery.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
			sampleDelivery.allowPause = false;
			sampleDelivery.refillMass = 0.5f;
			sampleDelivery.MinimumMass = 1f;
			sampleDelivery.capacity = 1f;
			sampleDelivery.RequestedItemTag = Tag.Invalid;
			sampleDelivery.operationalRequirement = Operational.State.None;

			go.AddOrGet<DNAInjector>();
			go.AddOrGet<DNAInjectorWorkable>();
		}
	}
}
