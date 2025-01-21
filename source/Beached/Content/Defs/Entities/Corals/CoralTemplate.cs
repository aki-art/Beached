using Beached.Content.Scripts.Entities;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static SeedProducer;

namespace Beached.Content.Defs.Entities.Corals
{
	public class CoralTemplate
	{
		public static readonly SimHashes[] ALL_WATERS =
		[
			SimHashes.Water,
			SimHashes.SaltWater,
			SimHashes.Brine,
			SimHashes.DirtyWater,
			Elements.murkyBrine
		];

		public GameObject CreatePrefab(string ID, string anim, string initialAnim, int width, int height, EffectorValues decor)
		{
			var name = Strings.Get("STRINGS.CORALS." + ID.ToUpperInvariant() + ".NAME");
			var desc = Strings.Get("STRINGS.CORALS." + ID.ToUpperInvariant() + ".DESCRIPTION");
			var seedName = Strings.Get("STRINGS.CORALS.FRAGS." + ID.ToUpperInvariant() + ".NAME");
			var seedDesc = Strings.Get("STRINGS.CORALS.FRAGS." + ID.ToUpperInvariant() + ".DESCRIPTION");
			var domesticatedDesc = Strings.Get("STRINGS.CORALS." + ID.ToUpperInvariant() + ".DOMESTICATED_DESCRIPTION");

			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				name,
				desc,
				10f,
				Assets.GetAnim(anim),
				initialAnim,
				Grid.SceneLayer.BuildingBack,
				width,
				height,
				decor,
				additionalTags:
				[
					BTags.aquaticPlant,
					BTags.coral,
					BTags.smallAquariumSeed
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
				ID + "Original",
				name);

			prefab.AddOrGet<SubmersionMonitor>();

			prefab.AddOrGet<LoopingSounds>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.DigOnly,
				ID + "Seed",
				seedName,
				seedDesc,
				Assets.GetAnim("beached_leaflet_coral_frag_kanim"),
				additionalTags:
				[
					GameTags.WaterSeed ,
					BTags.coralFrag
				],
				sortOrder: 3,
				domesticatedDescription: domesticatedDesc,
				width: 0.33f,
				height: 0.33f,
				ignoreDefaultSeedTag: true);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				ID + "Preview",
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

			return prefab;
		}

		public static GameObject Create(string id, float mass, string anim, int width, int height, EffectorValues decor, float defaultTemperature = 293f, string initialAnim = "idle_loop", List<Tag> additionalTags = null, SimHashes[] safeElements = null)
		{
			var name = Strings.Get("STRINGS.CORALS." + id.ToUpperInvariant() + ".NAME");
			var desc = Strings.Get("STRINGS.CORALS." + id.ToUpperInvariant() + ".DESC");

			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				name,
				desc,
				mass,
				Assets.GetAnim(anim),
				initialAnim,
				Grid.SceneLayer.BuildingBack,
				width,
				height,
				decor,
				NOISE_POLLUTION.NONE,
				SimHashes.Creature,
				additionalTags,
				defaultTemperature);

			prefab.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers =
			[
				ObjectLayer.Building
			];

			prefab.AddOrGet<EntombVulnerable>();
			//prefab.AddOrGet<DrowningMonitor>().livesUnderWater = true;

			if (safeElements != null)
			{
				prefab.AddOrGet<PressureVulnerable>().Configure(ALL_WATERS);
			}

			prefab.AddOrGet<Prioritizable>();
			prefab.AddOrGet<Uprootable>();

			prefab.AddOrGet<Harvestable>();
			prefab.AddOrGet<HarvestDesignatable>();

			prefab.AddTag(BTags.coral);
			return prefab;
		}

		public static GameObject AddDriedCoral(string id, GameObject coral, string anim, float width, float height,
			SingleEntityReceptacle.ReceptacleDirection direction = SingleEntityReceptacle.ReceptacleDirection.Top)
		{
			var name = Strings.Get("STRINGS.CORALS.SEED." + id.ToUpperInvariant() + ".NAME");
			var desc = Strings.Get("STRINGS.CORALS.SEED." + id.ToUpperInvariant() + ".DESC");
			var domesticatedDesc = Strings.Get("STRINGS.CORALS." + id.ToUpperInvariant() + ".DOMESTICATED_DESC");

			var prefab = EntityTemplates.CreateLooseEntity(
				id,
				name,
				desc,
				1f,
				true,
				Assets.GetAnim(anim),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.CIRCLE,
				width,
				height,
				true,
				SORTORDER.SEEDS);

			prefab.AddOrGet<EntitySplitter>();

			EntityTemplates.CreateAndRegisterCompostableFromPrefab(prefab);

			var plantableSeed = prefab.AddOrGet<PlantableSeed>();
			plantableSeed.PlantID = new Tag(prefab.name);
			plantableSeed.domesticatedDescription = domesticatedDesc;
			plantableSeed.direction = direction;

			var kPrefabId = prefab.AddOrGet<KPrefabID>();
			kPrefabId.AddTag(BTags.coralFrag);
			kPrefabId.AddTag(GameTags.PedestalDisplayable);

			Assets.AddPrefab(kPrefabId);

			coral
				.AddOrGet<SeedProducer>()
				.Configure(id, ProductionType.DigOnly, 1);

			return prefab;
		}

		public static void AddSimpleConverter(GameObject prefab, SimHashes input, float inKgPerSecond, SimHashes output, float outKgPerSecond = -1, float offsetX = 0, float offsetY = 0, float outputMultiplier = 1f, byte disase = byte.MaxValue, int diseaseCount = 0, bool storeOutput = true)
		{
			if (outKgPerSecond == -1)
			{
				outKgPerSecond = inKgPerSecond;
			}

			var elementConverter = prefab.AddComponent<ElementConverter>();
			elementConverter.OutputMultiplier = outputMultiplier;

			elementConverter.consumedElements =
			[
				new ElementConverter.ConsumedElement(input.ToString(), inKgPerSecond)
			];

			elementConverter.outputElements =
			[
				new ElementConverter.OutputElement(outKgPerSecond,
					output,
					0f,
					true,
					storeOutput,
					offsetX,
					offsetY,
					0.75f,
					disase,
					diseaseCount)
			];
		}
	}
}
