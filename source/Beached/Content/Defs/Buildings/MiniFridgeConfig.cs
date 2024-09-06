using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class MiniFridgeConfig : IBuildingConfig
	{
		public const string ID = "Beached_MiniFridge";

		private const int ENERGY_SAVER_POWER = 20 / 2;

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_minifridge_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER2,
				BuildLocationRule.OnFloor,
				BUILDINGS.DECOR.BONUS.TIER1,
				NOISE_POLLUTION.NONE);

			def.RequiresPowerInput = true;
			def.AddLogicPowerPort = false;
			def.EnergyConsumptionWhenActive = 120f;
			def.SelfHeatKilowattsWhenActive = 0.125f;
			def.ExhaustKilowattsWhenActive = 0f;
			def.LogicOutputPorts =
			[
				LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 0), global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT, global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_ACTIVE, global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_INACTIVE, false, false)
			];
			def.SceneLayer = Grid.SceneLayer.Building;
			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;

			def.Floodable = false;
			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = AUDIO.CATEGORY.METAL;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			base.ConfigureBuildingTemplate(go, prefab_tag);
			//LoreBearerUtil.AddLoreTo(go, LoreBearerUtil.UnlockSpecificEntry("pod_evacuation", "Inspect"));
			var storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.showDescriptor = true;
			storage.storageFilters = STORAGEFILTERS.FOOD;
			storage.allowItemRemoval = true;
			storage.capacityKg = 50f;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			storage.showCapacityStatusItem = true;

			Prioritizable.AddRef(go);

			go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = global::STRINGS.UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON_EDIBLES;
			go.AddOrGet<FoodStorage>();
			go.AddOrGet<Refrigerator>();
			go.AddOrGet<MiniFridge>();

			var def = go.AddOrGetDef<RefrigeratorController.Def>();
			def.powerSaverEnergyUsage = ENERGY_SAVER_POWER;
			def.coolingHeatKW = 0.375f / 2f;
			def.steadyHeatKW = 0f;

			go.AddOrGet<UserNameable>();
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;

		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<MiniFridgeShelfDisplay>();
			go.AddOrGetDef<StorageController.Def>();
			SymbolOverrideControllerUtil.AddToPrefab(go);
		}
	}
}
