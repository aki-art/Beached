using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class SmallAquariumConfig : IBuildingConfig
	{
		public const string ID = "Beached_SmallAquarium";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_small_aquarium_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.TRANSPARENTS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				NOISE_POLLUTION.NOISY.TIER1);

			def.ViewMode = OverlayModes.LiquidConduits.ID;
			def.AudioCategory = AUDIO.CATEGORY.GLASS;
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.InputConduitType = ConduitType.Liquid;
			def.SceneLayer = Grid.SceneLayer.BuildingBack;
			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			base.ConfigureBuildingTemplate(go, prefab_tag);
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			var storage = go.AddComponent<Storage>();
			storage.capacityKg = 20f;
			storage.storageFilters = STORAGEFILTERS.LIQUIDS;
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

			var plantablePlot = go.AddOrGet<SmallAquariumPlot>();
			plantablePlot.occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 0.0f);
			plantablePlot.occupyingObjectVisualOffset = new Vector3(0.0f, -0.15f, 0.0f);
			plantablePlot.plantLayer = Grid.SceneLayer.Building;
			plantablePlot.AddDepositTag(BTags.smallAquariumSeed);
			plantablePlot.SetFertilizationFlags(false, true);
			plantablePlot.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
			plantablePlot.tagOnPlanted = GameTags.PlantedOnFloorVessel;
			plantablePlot.liquidStorage = storage;

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = BTags.aquaticFarm;

			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var aquarium = go.AddOrGet<SmallAquarium>();
			aquarium.minimumWaterLevel = 10f;
			aquarium.storage = go.GetComponent<SmallAquariumPlot>().liquidStorage;
		}
	}
}
