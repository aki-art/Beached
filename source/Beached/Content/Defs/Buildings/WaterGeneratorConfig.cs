using Beached.Content.Overlays;
using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class WaterGeneratorConfig : IBuildingConfig
	{
		public const string ID = "Beached_WaterGenerator";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_watermill_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Anywhere,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.GeneratorWattageRating = 90f;
			def.GeneratorBaseCapacity = 1000f;
			def.ExhaustKilowattsWhenActive = 2f;
			def.SelfHeatKilowattsWhenActive = 2f;

			def.ViewMode = Beached_FlowOverlayMode.ID;
			def.Floodable = false;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;

			// conduit
			def.InputConduitType = ConduitType.Liquid;
			def.UtilityInputOffset = CellOffset.none;

			// power
			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = CellOffset.none;

			// automation
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 2));

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			// LiquidGunk?
			var tag = GameTags.LubricatingOil;
			var snailsToSustainOne = 3.0f;
			var amount = snailsToSustainOne / CONSTS.CYCLE_LENGTH;
			var capacity = 10.0f;

			go.AddOrGet<LogicOperationalController>();

			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();

			var storage = go.AddComponent<Storage>();
			storage.capacityKg = capacity;
			storage.showInUI = true;
			storage.allowItemRemoval = false;
			//storage.storageFilters = [tag];

			var generator = go.AddOrGet<WaterGenerator>();
			generator.windDownSpeed = (10.0f / 90f);
			generator.formula = EnergyGenerator.CreateSimpleFormula(tag, amount, 10.0f);
			generator.storage = storage;

			var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKG.SetStorage(storage);
			manualDeliveryKG.requestedItemTag = tag;
			manualDeliveryKG.capacity = capacity;
			manualDeliveryKG.refillMass = 1.5f;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			manualDeliveryKG.operationalRequirement = Operational.State.Functional;
			manualDeliveryKG.allowPause = true;

			// too complicated both to design and for users to set
			// part 1/2
			/*			var passiveElementConsumer = go.AddOrGet<PassiveElementConsumer>();
						passiveElementConsumer.consumptionRate = amount;
						passiveElementConsumer.capacityKG = 10f;
						passiveElementConsumer.consumptionRadius = 1;
						passiveElementConsumer.showInStatusPanel = true;
						passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
						passiveElementConsumer.isRequired = false;
						passiveElementConsumer.storeOnConsume = true;
						passiveElementConsumer.showDescriptor = false;

						passiveElementConsumer.EnableConsumption(false);

						var filter = go.AddComponent<FilterElementConsumer>();
						filter.targetTag = tag;
						filter.targetStorage = storage;
						filter.consumptionEnabled = true;

						go.GetComponent<KPrefabID>().prefabSpawnFn += OnSpawn; */

			var delivery = go.AddOrGet<HybridDelivery>();
			delivery.consumedPerSecond = amount;
			delivery.consumedTag = tag;
			delivery.storage = storage;

			var conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.capacityTag = GameTags.LubricatingOil;
			conduitConsumer.capacityKG = capacity;
			conduitConsumer.storage = storage;
			conduitConsumer.alwaysConsume = true;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

			Tinkerable.MakePowerTinkerable(go);
			go.AddOrGetDef<PoweredActiveController.Def>();

			go.GetComponent<RequireInputs>().SetRequirements(false, false);

		}

		/*		private void OnSpawn(GameObject go)
				{
					go.GetComponent<FilterElementConsumer>().SetStorage(go.GetComponent<WaterGenerator>().storage);
				}*/
	}
}
