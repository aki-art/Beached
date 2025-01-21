using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class DeconstructableRocketTileConfig : IBuildingConfig
	{
		public const string ID = "Beached_DeconstructableRocketTile";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"floor_rocket_kanim",
				1000,
				60f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				[SimHashes.Steel.ToString()],
				800f,
				BuildLocationRule.Tile,
				BUILDINGS.DECOR.BONUS.TIER0,
				NOISE_POLLUTION.NONE);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = AUDIO.CATEGORY.METAL;
			def.AudioSize = AUDIO.SIZE.SMALL;
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.isKAnimTile = true;
			def.BlockTileAtlas = Assets.GetTextureAtlas("tiles_rocket_wall_int");
			def.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_rocket_wall_int_place");
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			def.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_rocket_wall_ext_decor_info");
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_rocket_wall_ext_place_decor_info");
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.strengthMultiplier = 10f;
			simCellOccupier.notifyOnMelt = true;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = RocketWallTileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);

			var kprefabId = go.GetComponent<KPrefabID>();
			kprefabId.AddTag(GameTags.Bunker);
			kprefabId.AddTag(GameTags.FloorTiles);
			// kprefabId.AddTag(GameTags.RocketEnvelopeTile); // restricts what can be building on this

			go.GetComponent<Deconstructable>().allowDeconstruction = false;
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
