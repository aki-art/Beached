using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class MossBedConfig : IBuildingConfig
	{
		public const string ID = "Beached_MossBed";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_mossframe_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				new[]
				{
					200f,
					100f
				},
				new[]
				{
					GameTags.Farmable.ToString(),
					Elements.moss.ToString()
				},
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.NotInTiles,
				DECOR.NONE,
				NOISE_POLLUTION.NONE);

			def.Floodable = true;
			def.Overheatable = false;
			def.AudioCategory = "Metal";
			def.BaseTimeUntilRepair = -1f;
			def.ViewMode = OverlayModes.Temperature.ID;
			def.DefaultAnimState = "empty";
			def.ObjectLayer = ObjectLayer.Backwall;
			def.SceneLayer = Grid.SceneLayer.Backwall;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			var storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.capacityKg = MossBed.WATER_REQUIREMENT_KG;
			storage.storageFilters = STORAGEFILTERS.LIQUIDS;

			var delivery = go.AddComponent<ManualDeliveryKG>();
			delivery.SetStorage(storage);
			delivery.RequestedItemTag = GameTags.Water;
			delivery.capacity = MossBed.WATER_REQUIREMENT_KG;
			delivery.refillMass = MossBed.WATER_REQUIREMENT_KG;
			delivery.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

			// leaving output on null on purpose
			var converter = go.AddComponent<ElementConverter>();
			converter.consumedElements = new[]
			{
				new ElementConverter.ConsumedElement(GameTags.Water, MossBed.WATER_REQUIREMENT_KG / MossBed.GROWTH_TIME_SECONDS)
			};

			// empty list so this really just acts like a consumer
			converter.outputElements = new ElementConverter.OutputElement[] { };
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<MossBed>();
		}
	}
}
