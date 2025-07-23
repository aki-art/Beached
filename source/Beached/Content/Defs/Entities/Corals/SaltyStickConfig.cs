using Beached.Content.DefBuilders;
using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
	public class SaltyStickConfig : IEntityConfig
	{
		public const string ID = "Beached_Coral_SaltyStick";
		public const float CONSUMPTION_RATE = 1f;
		public static float MIN_OUTPUT_TEMP = MiscUtil.CelsiusToKelvin(15);

		public GameObject CreatePrefab()
		{
			var prefab = new CoralBuilder(ID, "beached_salty_stick_kanim")
				.Harvestable()
				.InitialAnim("idle_full")
				.Frag("beached_leaflet_coral_frag_kanim")
				.Build().entityPrefab;

			var elementConsumer = prefab.AddComponent<PassiveElementConsumer>();
			elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
			elementConsumer.consumptionRate = 0.5f;
			elementConsumer.storeOnConsume = true;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.consumptionRadius = 1;

			var storage = prefab.AddOrGet<Storage>();
			storage.capacityKg = 1000f;
			storage.showInUI = true;

			var filteringCoral = prefab.AddOrGet<FilteringCoral>();
			filteringCoral.gunkTag = SimHashes.Salt.CreateTag();
			filteringCoral.storage = storage;
			filteringCoral.gunkLimit = 10f;

			var kgPerSecondConverted = 0.3f;

			AddConverter(prefab, SimHashes.SaltWater.CreateTag(), SimHashes.Water, kgPerSecondConverted, storage);
			AddConverter(prefab, SimHashes.Brine.CreateTag(), SimHashes.SaltWater, kgPerSecondConverted, storage);
			AddConverter(prefab, Elements.murkyBrine.CreateTag(), SimHashes.DirtyWater, kgPerSecondConverted, storage);

			// slickshell
			var plantStorageElement = prefab.AddOrGet<DirectlyEdiblePlant_StorageElement>();
			plantStorageElement.tagToConsume = SimHashes.Salt.CreateTag();
			plantStorageElement.rateProducedPerCycle = kgPerSecondConverted * 600f;
			plantStorageElement.storageCapacity = storage.capacityKg;
			plantStorageElement.edibleCellOffsets =
			[
				CellOffset.left,
				CellOffset.right
			];

			return prefab;
		}

		private static void AddConverter(GameObject prefab, Tag from, SimHashes output, float kgPerSecond, Storage storage)
		{
			var converter = prefab.AddComponent<ElementConverter>();
			converter.consumedElements = [new ElementConverter.ConsumedElement(from, CONSUMPTION_RATE)];
			converter.outputElements =
			[
				new ElementConverter.OutputElement(1f - kgPerSecond, output, MIN_OUTPUT_TEMP, outputElementOffsety: 1),
				new ElementConverter.OutputElement(kgPerSecond, SimHashes.Salt, MIN_OUTPUT_TEMP, storeOutput: true),
			];

			converter.ShowInUI = false;
			converter.showDescriptors = false;
			converter.storage = storage;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
