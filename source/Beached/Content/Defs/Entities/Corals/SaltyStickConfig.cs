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

			AddConverter(prefab, SimHashes.SaltWater.CreateTag(), SimHashes.Water, 0.3f, storage);
			AddConverter(prefab, SimHashes.Brine.CreateTag(), SimHashes.SaltWater, 0.3f, storage);
			AddConverter(prefab, Elements.murkyBrine.Tag, SimHashes.DirtyWater, 0.3f, storage);

			return prefab;
		}

		private static void AddConverter(GameObject prefab, Tag from, SimHashes output, float saltContent, Storage storage)
		{
			var converter = prefab.AddComponent<ElementConverter>();
			converter.consumedElements = new[] { new ElementConverter.ConsumedElement(from, CONSUMPTION_RATE) };
			converter.outputElements = new[]
			{
				new ElementConverter.OutputElement(1f - saltContent, output, MIN_OUTPUT_TEMP, outputElementOffsety: 1),
				new ElementConverter.OutputElement(saltContent, SimHashes.Salt, MIN_OUTPUT_TEMP, storeOutput: true),
			};

			converter.ShowInUI = false;
			converter.showDescriptors = false;
			converter.storage = storage;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			if (inst.TryGetComponent(out PassiveElementConsumer consumer))
				consumer.EnableConsumption(true);
		}
	}
}
