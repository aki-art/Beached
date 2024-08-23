using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
	public class LeafletCoralConfig : IEntityConfig
	{
		public const string ID = "Beached_LeafletCoral";
		public const string SEED_ID = "Beached_LeafletCoralSeed";
		public const string BASE_TRAIT_ID = "Beached_LeafletCoraOriginal";
		public const string PREVIEW_ID = "Beached_LeafletCoraPreview";

		public const float CONVERSION_RATE = 0.5f;
		public const float SUCC_RATE = 0.01f;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_LEAFLETCORAL.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_LEAFLETCORAL.DESCRIPTION,
				10f,
				Assets.GetAnim("beached_leaflet_coral_kanim"),
				"idle_grown",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER0,
				additionalTags:
				[
					BTags.aquatic,
					BTags.coral
				]);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				MiscUtil.CelsiusToKelvin(0f),
				MiscUtil.CelsiusToKelvin(5f),
				MiscUtil.CelsiusToKelvin(42f),
				MiscUtil.CelsiusToKelvin(50f),
				[
					SimHashes.Water,
					SimHashes.SaltWater,
					SimHashes.Brine,
					SimHashes.DirtyWater,
					Elements.murkyBrine
				],
				false,
				0f,
				0.15f,
				null,
				false,
				true,
				false,
				true,
				2400f,
				0f,
				7400f,
				BASE_TRAIT_ID,
				STRINGS.CREATURES.SPECIES.BEACHED_LEAFLETCORAL.NAME);

			prefab.AddOrGet<SubmersionMonitor>();

			prefab.AddOrGet<LoopingSounds>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.DigOnly,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_LEAFLETCORAL.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_LEAFLETCORAL.DESC,
				Assets.GetAnim("beached_leaflet_coral_frag_kanim"),
				additionalTags:
				[
					GameTags.WaterSeed,
					BTags.coralFrag,
					BTags.smallAquariumSeed
				],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_LEAFLETCORAL.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f,
				ignoreDefaultSeedTag: true);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_leaflet_coral_kanim"),
				"place",
				1,
				1);

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

		public void OnSpawn(GameObject inst)
		{
			if (inst.TryGetComponent(out ElementConsumer consumer))
			{
				consumer.EnableConsumption(true);
			}
		}
	}
}
