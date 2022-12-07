using Beached.Content.Scripts;
using System.Collections.Generic;
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
            def.LogicOutputPorts = new List<LogicPorts.Port>
            {
                LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 0), global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT, global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_ACTIVE, global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_INACTIVE, false, false)
            };
            def.SceneLayer = Grid.SceneLayer.Building;
            def.ForegroundLayer = Grid.SceneLayer.BuildingBack;

            def.Floodable = false;
            def.ViewMode = OverlayModes.Power.ID;
            def.AudioCategory = AUDIO.CATEGORY.METAL;

            //SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_open", NOISE_POLLUTION.NOISY.TIER1);
            //SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_close", NOISE_POLLUTION.NOISY.TIER1);

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            var storage = go.AddOrGet<Storage>();
            storage.showInUI = true;
            storage.showDescriptor = true;
            storage.storageFilters = STORAGEFILTERS.FOOD;
            storage.allowItemRemoval = true;
            storage.capacityKg = 50f;
            storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
            storage.showCapacityStatusItem = true;

            storage.SetDefaultStoredItemModifiers(new() 
            { 
                Storage.StoredItemModifier.Seal, 
                Storage.StoredItemModifier.Preserve 
            });

            Prioritizable.AddRef(go);

            go.AddOrGet<TreeFilterable>();
            go.AddOrGet<FoodStorage>();
            go.AddOrGet<Refrigerator>();

            var def = go.AddOrGetDef<RefrigeratorController.Def>();
            def.powerSaverEnergyUsage = ENERGY_SAVER_POWER;
            def.coolingHeatKW = 0.375f / 2f;
            def.steadyHeatKW = 0f;

            go.AddOrGet<UserNameable>();
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;
            go.AddOrGetDef<StorageController.Def>();
            go.AddOrGet<MiniFridgeShelfDisplay>();

            SymbolOverrideControllerUtil.AddToPrefab(go);
        }
    }
}
