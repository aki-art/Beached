using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class AmmoniaGeneratorConfig : IBuildingConfig
	{
		public const string ID = "Beached_AmmoniaGenerator";

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
			go.AddOrGet<Storage>();

			var conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.consumptionRate = 1f;
			conduitConsumer.capacityTag = Elements.ammonia.CreateTag();
			conduitConsumer.capacityKG = 2f;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

			var dispenser = go.AddOrGet<ConduitDispenser>();
			dispenser.conduitType = ConduitType.Liquid;

			// TODO: configure proper values
			var energyGenerator = go.AddOrGet<EnergyGenerator>();
			energyGenerator.formula = new EnergyGenerator.Formula
			{
				inputs =
				[
					new EnergyGenerator.InputItem(Elements.ammonia.CreateTag(), 0.09f, 0.90000004f)
				],
				outputs =
				[
					new EnergyGenerator.OutputItem(SimHashes.Water, 0.0675f, true, new CellOffset(1, 1), 313.15f),
					new EnergyGenerator.OutputItem(Elements.nitrogen, 0.0225f, false, new CellOffset(0, 2), 383.15f)
				]
			};

			energyGenerator.powerDistributionOrder = 8;
			energyGenerator.ignoreBatteryRefillPercent = true;
			energyGenerator.meterOffset = Meter.Offset.Behind;

			Tinkerable.MakePowerTinkerable(go);
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
