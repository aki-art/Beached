using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Scripts.Entities.Plant;
using System;
using System.Linq;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class FilamentConfig : IEntityConfig
	{
		public const string ID = "Beached_Filament";
		public const string SEED_ID = "Beached_FilamentSeed";
		public const string PREVIEW_ID = "Beached_FilamentPreview";
		public const float WATER_RATE = 5f / CONSTS.CYCLE_LENGTH;

		public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;
		public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;


		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_FILAMENT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_FILAMENT.DESC,
				10f,
				Assets.GetAnim("beached_filament_kanim"),
				"idle",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				POSITIVE_DECOR_EFFECT,
				additionalTags:
				[
					BTags.aquaticPlant,
					//BTags.smallAquariumSeed
				],
				defaultTemperature: MiscUtil.CelsiusToKelvin(32f));

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				MiscUtil.CelsiusToKelvin(20f),
				MiscUtil.CelsiusToKelvin(24f),
				MiscUtil.CelsiusToKelvin(34f),
				MiscUtil.CelsiusToKelvin(45f),
				CoralTemplate.ALL_WATERS.Union([SimHashes.Ethanol, SimHashes.Petroleum]).ToArray(),
				false,
				0f,
				40f,
				null,
				false,
				false,
				true,
				true,
				2400f,
				0f,
				7400f,
				ID + "Original",
				STRINGS.CREATURES.SPECIES.BEACHED_FILAMENT.NAME);

			var decorPlant = prefab.AddOrGet<PrickleGrass>();
			decorPlant.positive_decor_effect = POSITIVE_DECOR_EFFECT;
			decorPlant.negative_decor_effect = NEGATIVE_DECOR_EFFECT;

			prefab.AddOrGet<SubmersionMonitor>();
			prefab.AddOrGet<LoopingSounds>();

			var light = prefab.AddOrGet<Light2D>();
			light.Lux = 400;
			light.Color = Util.ColorFromHex("7cd8eb") * 1.1f;
			light.Range = 3;
			light.drawOverlay = true;
			light.shape = LightShape.Circle;
			light.Offset = new Vector2(0f, 0.5f);

			prefab.AddOrGet<Filament>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				this as IHasDlcRestrictions,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_FILAMENT.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_FILAMENT.DESC,
				Assets.GetAnim("beached_small_cell_kanim"),
				additionalTags: [BTags.aquaticSeed],
				sortOrder: 3,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_FILAMENT.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_small_cell_kanim"),
				"place",
				1,
				1);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
