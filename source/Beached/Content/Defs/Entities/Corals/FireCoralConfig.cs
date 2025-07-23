using Beached.Content.DefBuilders;
using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
	public class FireCoralConfig : IEntityConfig
	{
		public const string ID = "Beached_FireCoral";
		public const float CONVERSION_RATE = 0.5f;
		public const float SUCC_RATE = 0.01f;

		public GameObject CreatePrefab()
		{
			var prefab = new CoralBuilder(ID, "beached_fire_coral_kanim")
				.Harvestable()
				.DefaultTemperatureCelsius(43)
				.Age(24 * CONSTS.CYCLE_LENGTH)
				.SafeTemperaturesCelsius(20, 25, 82, 94)
				.InitialAnim("idle_grown")
				.SafeIn([SimHashes.Ethanol, SimHashes.Petroleum])
				.Frag("beached_fire_coral_frag_kanim", additionalTags: [GameTags.WaterSeed])
				.Build().entityPrefab;

			var storage = prefab.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.capacityKg = 1f;

			var passiveElementConsumer = prefab.AddOrGet<PassiveElementConsumer>();
			//passiveElementConsumer.elementToConsume = SimHashes.Water;
			passiveElementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
			passiveElementConsumer.consumptionRate = 20f;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;

			var coral = prefab.AddComponent<Coral>();
			coral.emitTag = GameTags.Gas;
			coral.emitMass = 20f * 0.1f;
			coral.initialVelocity = new Vector2f(0, 1);
			coral.consumptionRate = 0.2f;
			coral.filter = GameTags.CombustibleLiquid;

			var elementConverter = prefab.AddComponent<ElementConverter>();
			elementConverter.OutputMultiplier = 0.1f;

			elementConverter.consumedElements =
			[
				new ElementConverter.ConsumedElement(GameTags.CombustibleLiquid, SUCC_RATE)
			];

			elementConverter.outputElements =
			[
				new ElementConverter.OutputElement(
					(float)(SUCC_RATE * CONVERSION_RATE),
					SimHashes.CarbonDioxide,
					MiscUtil.CelsiusToKelvin(75f),
					true,
					true,
					0,
					0.5f,
					0.75f,
					byte.MaxValue,
					0)
			];

			var directVolumeHeater = prefab.AddOrGet<DirectVolumeHeater>();
			directVolumeHeater.DTUs = 2000f;
			directVolumeHeater.width = 3;
			directVolumeHeater.height = 3;
			directVolumeHeater.maximumExternalTemperature = MiscUtil.CelsiusToKelvin(95f);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			var rangeVisualizer = inst.AddOrGet<RangeVisualizer>();
			rangeVisualizer.RangeMax = new Vector2I(1, 1);
			rangeVisualizer.RangeMin = new Vector2I(-1, -1);
			rangeVisualizer.BlockingTileVisible = false;

			inst.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, new CellOffset());
		}
	}
}
