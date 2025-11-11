using Beached.Content.DefBuilders;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.Plant;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
	public class LeafletCoralConfig : IEntityConfig
	{
		public const string ID = "Beached_LeafletCoral";
		public const string SEED_ID = "Beached_LeafletCoralSeed";
		public const float CONVERSION_RATE = 0.5f;
		public const float SUCC_RATE = 0.05f;
		public const float RATE_MULT = 2f;

		public GameObject CreatePrefab()
		{
			var prefab = new CoralBuilder(ID, "beached_leaflet_coral_kanim")
				.Harvestable()
				.Age(6 * CONSTS.CYCLE_LENGTH)
				.SafeTemperaturesCelsius(30, 35, 62, 75)
				.DefaultTemperatureCelsius(42)
				.InitialAnim("idle_grown")
				.Frag("beached_leaflet_coral_frag_kanim", additionalTags: [GameTags.WaterSeed])
				.Build().entityPrefab;

			var storage = prefab.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.capacityKg = 1f;

			var passiveElementConsumer = prefab.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.Water;
			passiveElementConsumer.consumptionRate = 0.05f * RATE_MULT;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;

			var coral = prefab.AddComponent<Coral>();
			coral.emitTag = GameTags.Gas;
			coral.emitMass = 0.2f;
			coral.initialVelocity = new Vector2f(0, 1);
			coral.consumptionRate = 0.05f * RATE_MULT;
			coral.spawnFX = SpawnFXHashes.OxygenEmissionBubbles;

			EntityTemplates.ExtendPlantToFertilizable(prefab,
			[
				new PlantElementAbsorber.ConsumeInfo()
				{
					tag = SimHashes.Lime.CreateTag(),
					massConsumptionRate = MiscUtil.PerCycle(4.0f)
			  }
			]);

			prefab.AddComponent<LeafletCoral>();

			CoralTemplate.AddSimpleConverter(prefab, SimHashes.Water, SUCC_RATE, SimHashes.Oxygen, SUCC_RATE * CONVERSION_RATE);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
