using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class ShutterTileConfig : IBuildingConfig
	{

		public const string ID = "Beached_ShutterTile";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_shutters_kanim",
				100,
				30f,
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_METALS,
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

			def.LogicInputPorts = [
				LogicPorts.Port.InputPort(
					LogicOperationalController.PORT_ID,
					CellOffset.none,
					global::STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN,
					global::STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_ACTIVE,
					global::STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_INACTIVE)
			];

			def.AddSearchTerms(global::STRINGS.SEARCH_TERMS.TILE);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			//GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			go.AddOrGet<Prioritizable>();
			Prioritizable.AddRef(go);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			go.AddOrGet<Operational>();
			go.AddOrGet<LogicPorts>();
			go.AddOrGet<LogicOperationalController>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var door = go.AddOrGet<ShutterTile>();
			//go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
			go.AddOrGet<ZoneTile>();
			go.AddOrGet<KBoxCollider2D>();
			Prioritizable.AddRef(go);
			Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddTag(GameTags.NoCreatureIdling);
		}
	}
}
