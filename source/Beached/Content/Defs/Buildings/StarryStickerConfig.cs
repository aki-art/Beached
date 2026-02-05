using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class StarryStickerConfig : IBuildingConfig
	{
		public const string ID = "Beached_StarrySticker";
		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_starrysticker_kanim",
				30,
				1f,
				[25f, 5f],
				[BTags.BuildingMaterials.zinc.ToString(), SimHashes.Sulfur.ToString()],
				1600f,
				BuildLocationRule.NotInTiles,
				DECOR.BONUS.TIER1,
				NOISE_POLLUTION.NONE);

			def.Entombable = true;
			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
			def.AudioSize = AUDIO.SIZE.SMALL;
			def.BaseTimeUntilRepair = -1f;
			def.DefaultAnimState = "off";
			def.ObjectLayer = ObjectLayer.Building;
			def.ViewMode = OverlayModes.Light.ID;
			def.SceneLayer = Grid.SceneLayer.InteriorWall;
			def.PermittedRotations = PermittedRotations.R360;
			def.ReplacementLayer = ObjectLayer.ReplacementBackwall;

			def.ReplacementCandidateLayers =
			[
				ObjectLayer.Backwall
			];

			def.ReplacementTags =
			[
				GameTags.Backwall
			];

			def.AddSearchTerms(global::STRINGS.SEARCH_TERMS.TILE);
			def.AddSearchTerms(global::STRINGS.SEARCH_TERMS.DECOR);

			return def;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			var lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 100;
			lightShapePreview.radius = 1;
			lightShapePreview.shape = LightShape.Circle;
			lightShapePreview.offset = CellOffset.none;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			go.AddComponent<ZoneTile>();
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall);
			GeneratedBuildings.RemoveLoopingSounds(go);

			var light2D = go.AddOrGet<Light2D>();
			light2D.Color = new Color(0.96f, 2.55f, 2.05f, 1.0f);
			light2D.Range = 1f;
			light2D.shape = LightShape.Circle;
			light2D.drawOverlay = false;
			light2D.Lux = 100;
			light2D.Offset = new Vector2(0.0f, 0.5f);

			go.AddOrGetDef<CreatureLightToggleController.Def>();

			go.AddOrGetDef<OnlyOperationalAtNightController.Def>();
			go.AddOrGetDef<LightController.Def>();
		}
	}
}
