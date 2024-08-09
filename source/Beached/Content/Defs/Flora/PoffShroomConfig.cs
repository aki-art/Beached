using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class PoffShroomConfig : IEntityConfig
	{
		public const string ID = "Beached_PoffShroom";
		public const string PREVIEW_ID = "Beached_PoffShroomPreview";
		public const string SEED_ID = "Beached_PoffShroomSeed"; // only for replanting
		public const string BASE_TRAIT_ID = "Beached_PoffShroomOriginal";

		public GameObject CreatePrefab()
		{
			var anim = Assets.GetAnim("beached_poffshroom_kanim");

			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_POFFSHROOM.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_POFFSHROOM.DESC,
				40f,
				anim,
				"idle_full",
				Grid.SceneLayer.Building,
				1,
				1,
				DECOR.NONE);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				crop_id: MushroomConfig.ID, // TODO
				baseTraitId: BASE_TRAIT_ID,
				baseTraitName: STRINGS.CREATURES.SPECIES.BEACHED_POFFSHROOM.NAME);

			prefab.AddOrGet<StandardCropPlant>();

			prefab.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
			prefab.AddOrGet<WiltCondition>();

			var storage = prefab.AddOrGet<Storage>();
			storage.capacityKg = 2f;
			storage.showInUI = true;

			var elementConsumer = prefab.AddOrGet<ElementConsumer>();
			elementConsumer.storeOnConsume = true;
			elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
			elementConsumer.elementToConsume = SimHashes.Vacuum;
			elementConsumer.capacityKG = 2f;
			elementConsumer.consumptionRate = 0.25f;
			elementConsumer.consumptionRadius = 1;
			elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
			elementConsumer.storage = storage;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.showDescriptor = true;

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.DigOnly,
				SEED_ID,
				"Poffshroom Spore",
				"",
				Assets.GetAnim("beached_poffshroom_seed_kanim"),
				additionalTags: new() { GameTags.CropSeed });

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				anim,
				"place",
				1,
				1);

			prefab.AddOrGet<PoffShroom>();

			return prefab;
		}

		public string[] GetDlcIds()
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
