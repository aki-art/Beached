using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class GlowCapConfig : IEntityConfig
	{
		public const string ID = "Beached_GlowCap";
		public const string BASE_TRAIT_ID = "Beached_PoffShroomOriginal";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_GLOWCAP.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_GLOWCAP.DESC,
				40f,
				Assets.GetAnim("beached_glowcap_kanim"),
				"idle_empty",
				Grid.SceneLayer.Building,
				1,
				2,
				DECOR.NONE);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				baseTraitId: BASE_TRAIT_ID,
				baseTraitName: STRINGS.CREATURES.SPECIES.BEACHED_POFFSHROOM.NAME);

			//prefab.AddOrGet<StandardCropPlant>();

			// TODO: prefer moderately dark
			//prefab.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
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
			//EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "MushroomPlant_preview", Assets.GetAnim("fungusplant_kanim"), "place", 1, 2);

			var light = prefab.AddOrGet<Light2D>();
			light.Color = new Color(0, 0, 3f);
			light.Lux = 100;
			light.Range = 2f;
			light.shape = LightShape.Circle;

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
