using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class CrystalGrowerConfig : IBuildingConfig
	{
		public const string ID = "Beached_CrystalGrowerTile";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_aquatic_farmtile_kanim",
				100,
				30f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_METALS,
				1600f,
				BuildLocationRule.Tile,
				BUILDINGS.DECOR.NONE,
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
			def.PermittedRotations = PermittedRotations.R360;
			def.DragBuild = true;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.strengthMultiplier = 3f;

			go.AddOrGet<TileTemperature>();

			var storage = BuildingTemplates.CreateDefaultStorage(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);


			var crystalGrower = go.AddOrGet<CrystalGrower>();
			crystalGrower.occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 0.0f);
			crystalGrower.AddDepositTag(BTags.crystalCluster);

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
			go.AddOrGet<AnimTileable>();
			Prioritizable.AddRef(go);
		}


		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			//go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles);

			go.GetComponent<KPrefabID>().prefabSpawnFn += delegate (GameObject inst)
			{
				var rotatable = inst.GetComponent<Rotatable>();
				var grower = inst.GetComponent<CrystalGrower>();

				switch (rotatable.GetOrientation())
				{
					case Orientation.Neutral:
					case Orientation.FlipH:
						grower.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
						break;
					case Orientation.R180:
					case Orientation.FlipV:
						grower.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Bottom);
						break;
					case Orientation.R90:
					case Orientation.R270:
						grower.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Side);
						break;
					case Orientation.NumRotations:
						break;
				}
			};
		}
	}
}
