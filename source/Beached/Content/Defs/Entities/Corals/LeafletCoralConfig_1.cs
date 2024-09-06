using Beached.Content.DefBuilders;
using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
	public class LeafletCoralConfig : IEntityConfig
	{
		public const string ID = "Beached_LeafletCoral";
		public const float CONVERSION_RATE = 0.5f;
		public const float SUCC_RATE = 0.01f;

		public GameObject CreatePrefab()
		{
			var prefab = new CoralBuilder(ID, "beached_leaflet_coral_kanim")
				.Harvestable()
				.Age(6 * CONSTS.CYCLE_LENGTH)
				.SafeTemperaturesCelsius(0, 5, 42, 50)
				.InitialAnim("idle_grown")
				.Frag("beached_leaflet_coral_frag_kanim")
				.Build().entityPrefab;

			var storage = prefab.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.capacityKg = 1f;

			var passiveElementConsumer = prefab.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.Water;
			passiveElementConsumer.consumptionRate = 0.2f;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;

			var coral = prefab.AddComponent<Coral>();
			coral.emitTag = GameTags.Gas;
			coral.emitMass = 0.05f;
			coral.initialVelocity = new Vector2f(0, 1);
			coral.consumptionRate = 0.2f;

			CoralTemplate.AddSimpleConverter(prefab, SimHashes.Water, SUCC_RATE, SimHashes.Oxygen, SUCC_RATE * CONVERSION_RATE);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
