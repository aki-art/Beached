using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class GlowyPlantConfig : IEntityConfig
	{
		public const string ID = "Beached_GlowyPlant";
		public const string SEED_ID = "Beached_GlowyPlantSeed";
		public const string PREVIEW_ID = "Beached_GlowyPlantPreview";
		public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;
		public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_GLOWYPLANT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_GLOWYPLANT.DESC,
				100f,
				Assets.GetAnim("beached_glowyplant_kanim"),
				"night_idle_full",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER2,
				defaultTemperature: CREATURES.TEMPERATURE.HOT_1);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				CREATURES.TEMPERATURE.FREEZING_10,
				CREATURES.TEMPERATURE.FREEZING_9,
				CREATURES.TEMPERATURE.HOT_2,
				CREATURES.TEMPERATURE.HOT_3,
				null,
				false,
				0f,
				0.15f,
				"PlantMeat",
				true,
				true,
				true,
				false,
				2400f,
				0f,
				2200f,
				ID + "Original",
				STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.NAME);

			var decorPlant = prefab.AddOrGet<PrickleGrass>();
			decorPlant.positive_decor_effect = POSITIVE_DECOR_EFFECT;
			decorPlant.negative_decor_effect = NEGATIVE_DECOR_EFFECT;

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Hidden,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.DESC,
				Assets.GetAnim("beached_watercups_seed_kanim"),
				"object",
				additionalTags: [GameTags.DecorSeed],
				sortOrder: 12,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_WATERCUPS.DOMESTICATEDDESC);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_glowyplant_kanim"),
				"place",
				1,
				1);

			var light = prefab.AddComponent<Light2D>();
			light.Range = 2f;
			light.shape = LightShape.Circle;
			light.Color = new Color(0, 1f, 0.6f);
			light.drawOverlay = false;
			light.Lux = 400;
			light.Offset = new Vector2(0, 0.3f);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			inst.GetComponent<KBatchedAnimController>().animScale *= 1.25f;
		}
	}
}
