using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class DNAExtractorConfig : IBuildingConfig
	{
		public const string ID = "Beached_DNAExtractor";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				2,
				"craftingStation_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				default);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 120f;
			def.ViewMode = OverlayModes.Power.ID;
			def.PowerInputOffset = new CellOffset(1, 0);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			var storage = go.AddOrGet<Storage>();

			var manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg.SetStorage(storage);
			manualDeliveryKg.RequestedItemTag = BTags.markedForDNAAnalysis;
			manualDeliveryKg.capacity = 300f;
			manualDeliveryKg.refillMass = 60f;
			manualDeliveryKg.MinimumMass = 1f;
			manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.RanchingFetch.IdHash;

			Prioritizable.AddRef(go);

			go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
