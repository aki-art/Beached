/*using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class BioFuelGeneratorConfig : IBuildingConfig
	{
// todo: add lubricatable
		public const string ID = "Beached_BioFuelGenerator";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				3,
				"beached_ammoniagenerator_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.GeneratorWattageRating = 240f;
			def.GeneratorBaseCapacity = 1000f;
			def.ExhaustKilowattsWhenActive = 2f;
			def.SelfHeatKilowattsWhenActive = 2f;

			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.UtilityInputOffset = new CellOffset(0, 2);
			def.UtilityOutputOffset = new CellOffset(0, 0);
			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = new CellOffset(0, 0);
			def.InputConduitType = ConduitType.Gas;
			def.OutputConduitType = ConduitType.Liquid;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 2));

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();

			var storage = go.AddOrGet<Storage>();
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			storage.showInUI = true;

			go.AddOrGet<LoopingSounds>();

			var manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg.SetStorage(storage);
			manualDeliveryKg.RequestedItemTag = SimHashes.WoodLog.CreateTag();
			manualDeliveryKg.capacity = 360f;
			manualDeliveryKg.refillMass = 180f;
			manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

			var energyGenerator = go.AddOrGet<EnergyGenerator>();
			energyGenerator.powerDistributionOrder = 8;
			energyGenerator.hasMeter = true;
			energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(
				SimHashes.WoodLog.CreateTag(),
				1.2f,
				720f,
				SimHashes.CarbonDioxide,
				0.17f,
				false,
				new CellOffset(0, 1),
				383.15f);

			Tinkerable.MakePowerTinkerable(go);
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
*/