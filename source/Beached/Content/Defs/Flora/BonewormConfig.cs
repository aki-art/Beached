using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class BonewormConfig : IEntityConfig
	{
		public const string ID = "Beached_Boneworm";
		public const string SEED_ID = "Beached_BonewormSeed";
		public const string BASETRAIT_ID = "Beached_BonewormOriginal";
		public const string PREVIEW_ID = "Beached_BonewormPreview";

		public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;
		public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;

		public GameObject CreatePrefab()
		{
			var gameObject = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_BONEWORM.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_BONEWORM.DESC,
				100f,
				Assets.GetAnim("beached_boneworm_kanim"),
				"idle",
				Grid.SceneLayer.BuildingFront,
				1,
				1,
				POSITIVE_DECOR_EFFECT,
				NOISE_POLLUTION.NONE,
				SimHashes.Creature,
				null,
				298.15f);

			EntityTemplates.ExtendEntityToBasicPlant(
				gameObject,
				MiscUtil.CelsiusToKelvin(18),
				MiscUtil.CelsiusToKelvin(24),
				MiscUtil.CelsiusToKelvin(45),
				MiscUtil.CelsiusToKelvin(52),
				[
					SimHashes.Oxygen,
					Elements.saltyOxygen,
					SimHashes.ContaminatedOxygen,
					SimHashes.CarbonDioxide,
					Elements.ammonia,
					SimHashes.Hydrogen
				],
				can_tinker: false,
				baseTraitId: BASETRAIT_ID,
				baseTraitName: STRINGS.CREATURES.SPECIES.BEACHED_BONEWORM.NAME);

			var decorPlant = gameObject.AddOrGet<PrickleGrass>();
			decorPlant.positive_decor_effect = POSITIVE_DECOR_EFFECT;
			decorPlant.negative_decor_effect = NEGATIVE_DECOR_EFFECT;

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				gameObject,
				this as IHasDlcRestrictions,
				SeedProducer.ProductionType.Hidden,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_BONEWORM.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_BONEWORM.DESC,
				Assets.GetAnim("beached_boneworm_seed_kanim"),
				"object",
				additionalTags: [GameTags.DecorSeed],
				sortOrder: 12,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_BONEWORM.DOMESTICATEDDESC);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_boneworm_kanim"),
				"place",
				1,
				1);

			return gameObject;
		}

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
