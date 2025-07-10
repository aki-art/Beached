using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class BioFuelGeneratorConfig : IBuildingConfig
	{
		public const string ID = "Beached_BioFuelGenerator";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				3,
				"generatormerc_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.GeneratorWattageRating = 400f;
			def.GeneratorBaseCapacity = 1000f;
			def.ExhaustKilowattsWhenActive = 2f;
			def.SelfHeatKilowattsWhenActive = 2f;

			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;

			def.UtilityInputOffset = new CellOffset(0, 1);
			def.InputConduitType = ConduitType.Liquid;

			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = new CellOffset(0, 0);

			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 2));

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();

			var conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Liquid;
			conduitConsumer.consumptionRate = 1f;
			conduitConsumer.capacityTag = Elements.bioFuel.CreateTag();
			conduitConsumer.capacityKG = 2f;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

			var storage = go.AddOrGet<Storage>();
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			storage.showInUI = true;

			go.AddOrGet<LoopingSounds>();

			var energyGenerator = go.AddOrGet<EnergyGenerator>();
			energyGenerator.powerDistributionOrder = 8;
			energyGenerator.hasMeter = true;
			energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(
				Elements.bioFuel.CreateTag(),
				MiscUtil.PerCycle(300),
				720f,
				SimHashes.CarbonDioxide,
				MiscUtil.PerCycle(7.2f),
				false,
				new CellOffset(0, 1),
				383.15f);

			Tinkerable.MakePowerTinkerable(go);
			go.AddOrGetDef<PoweredActiveController.Def>();

			Lubricatable.ConfigurePrefab(go, 10f, 10f / (CONSTS.CYCLE_LENGTH * 3f), true, Lubricatable.BoostType.Generator);
		}
	}
}
