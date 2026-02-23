using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	class FiltrationTileConfig : IBuildingConfig
	{
		public const string ID = "Beached_FiltrationTile";

		private static readonly CellOffset[] DELIVERY_OFFSETS =
		[
			new CellOffset(1, 0)
		];

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 2,
				anim: "farmtilerotating_kanim",
				hitpoints: 100,
				construction_time: 30f,
				construction_mass: [400f, 4f],
				construction_materials: [MATERIALS.BUILDABLERAW, BTags.BuildingMaterials.mineralWool.ToString()],
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			def.AudioCategory = "HollowMetal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.PermittedRotations = PermittedRotations.FlipV;
			def.DragBuild = true;

			return def;
		}
		private ElementConverter.OutputElement GetOutputElement(SimHashes element, float kgPerSecond)
		{
			return new ElementConverter.OutputElement(
					kgPerSecond,
					element: element,
					0f,
					true,
					true,
					0,
					0);
		}

		private ElementConverter.ConsumedElement[] InputElement(SimHashes element, float kgPerSecond)
		{
			return
			[
				new ElementConverter.ConsumedElement(tag: element.CreateTag(), kgPerSecond: kgPerSecond)
			];
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			/*
						var simcelloccupier = go.AddOrGet<SimCellOccupier>();
						simcelloccupier.doReplaceElement = true;
						simcelloccupier.notifyOnMelt = true;*/

			var storage = go.AddComponent<Storage>();
			storage.storageFilters = [GameTags.Liquid];
			storage.capacityKg = 10f;

			ElementConsumer elementconsumer = go.AddOrGet<PassiveElementConsumer>();
			elementconsumer.configuration = ElementConsumer.Configuration.AllLiquid;
			elementconsumer.consumptionRate = 100f;
			elementconsumer.consumptionRadius = 1;
			elementconsumer.showInStatusPanel = true;
			elementconsumer.sampleCellOffset = new Vector3(0f, 1f, 0f);
			elementconsumer.isRequired = false;
			elementconsumer.storeOnConsume = true;
			elementconsumer.showDescriptor = false;
			elementconsumer.storage = storage;


			var elementConverter = go.AddComponent<ElementConverter>();
			elementConverter.consumedElements = InputElement(SimHashes.DirtyWater, 100f);
			elementConverter.outputElements =
			[
				GetOutputElement(SimHashes.Water, 70f),
				GetOutputElement(SimHashes.ToxicSand, 30f)
			];

			var elementConverter2 = go.AddComponent<ElementConverter>();
			elementConverter2.consumedElements = InputElement(SimHashes.SaltWater, 100f);
			elementConverter2.outputElements = new ElementConverter.OutputElement[]
			{
				GetOutputElement(SimHashes.Water, 70f),
				GetOutputElement(SimHashes.Salt, 30f)
			};

			var elementConverter3 = go.AddComponent<ElementConverter>();
			elementConverter3.consumedElements = InputElement(SimHashes.Brine, 100f);
			elementConverter3.outputElements = new ElementConverter.OutputElement[]
			{
				GetOutputElement(SimHashes.SaltWater, 50f),
				GetOutputElement(SimHashes.Salt, 50f)
			};
			elementConverter.useGUILayout = false;
			elementConverter2.useGUILayout = false;
			elementConverter3.useGUILayout = false;

			/*				CopyBuildingSettings cbs = go.AddOrGet<CopyBuildingSettings>();
							cbs.copyGroupTag = GameTags.Farm;*/

			//go.AddComponent<FiltrationTileWorkable>();

			go.AddComponent<FiltrationTile>();

			go.AddOrGet<AnimTileable>();
			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var def = go.AddOrGetDef<MakeBaseSolid.Def>();
			def.occupyFoundationLayer = false;
			def.solidOffsets = [CellOffset.none];

			GeneratedBuildings.RemoveLoopingSounds(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles, false);

			if (go.TryGetComponent(out ElementConsumer consumer))
				consumer.EnableConsumption(true);

			/*	var feederBlock = go.AddComponent<CritterFeederStorageBlock>();
				feederBlock.cellOffset = CellOffset.up;
				feederBlock.connectorDefId = FiltrationTileCritterFeederConfig.ID;*/
		}

		public void OnSpawn(GameObject go)
		{
			if (go.TryGetComponent(out ElementConsumer consumer))
				consumer.EnableConsumption(true);
		}
	}
}
