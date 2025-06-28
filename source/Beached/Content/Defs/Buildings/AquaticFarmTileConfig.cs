using STRINGS;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class AquaticFarmTileConfig : IBuildingConfig
	{
		public const string ID = "Beached_AquaticFarmTile";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_aquatic_farmtile_kanim",
				100,
				30f,
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.FARMABLE,
				1600f,
				BuildLocationRule.Tile,
				TUNING.BUILDINGS.DECOR.NONE,
				NOISE_POLLUTION.NONE);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.AudioSize = AUDIO.SIZE.SMALL;
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.PermittedRotations = PermittedRotations.FlipV;
			def.DragBuild = true;
			def.Breakable = false;

			def.POIUnlockable = true;

			def.AddSearchTerms(SEARCH_TERMS.FOOD);
			def.AddSearchTerms(SEARCH_TERMS.FARM);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.strengthMultiplier = 20f;

			go.AddOrGet<TileTemperature>();

			var storage = BuildingTemplates.CreateDefaultStorage(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

			var plantablePlot = go.AddOrGet<PlantablePlot>();
			plantablePlot.occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 0.0f);
			plantablePlot.AddDepositTag(BTags.aquaticSeed);
			plantablePlot.AddDepositTag(BTags.coralFrag);
			plantablePlot.AddDepositTag(GameTags.WaterSeed);
			plantablePlot.SetFertilizationFlags(true, false);

			/*			var elementConsumer = go.AddOrGet<PassiveElementConsumer>();
						elementConsumer.elementToConsume = SimHashes.Water; // TODO other liquids
						elementConsumer.consumptionRate = 5f / CONSTS.CYCLE_LENGTH;
						elementConsumer.capacityKG = 10f;
						elementConsumer.consumptionRadius = 3;
						elementConsumer.showInStatusPanel = true;
						elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
						elementConsumer.isRequired = false;
						elementConsumer.storeOnConsume = true;
						elementConsumer.showDescriptor = true;
						elementConsumer.ignoreActiveChanged = true;

						go.GetComponent<KPrefabID>().prefabSpawnFn += go =>
						{
							go.GetComponent<PassiveElementConsumer>().EnableConsumption(true);
						};

						elementConsumer.EnableConsumption(true);
			*/
			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
			go.AddOrGet<AnimTileable>();
			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles);
			FarmTileConfig.SetUpFarmPlotTags(go);


			Tinkerable.MakePowerTinkerable(go);
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
