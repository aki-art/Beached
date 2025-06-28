using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class WaterCupsConfig : IEntityConfig
	{
		public const string ID = "Beached_WaterCups";
		public const string SEED_ID = "Beached_WaterCupsSeed";
		public const string BASETRAIT_ID = "Beached_WaterCupsOriginal";
		public const string PREVIEW_ID = "Beached_WaterCupsPreview";

		public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;
		public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;

		public GameObject CreatePrefab()
		{
			var gameObject = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_WATERCUPS.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_WATERCUPS.DESC,
				100f,
				Assets.GetAnim("beached_watercups_kanim"),
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
				288.15f,
				293.15f,
				323.15f,
				373.15f,
				[
					SimHashes.Oxygen,
					Elements.saltyOxygen,
					SimHashes.ContaminatedOxygen,
					SimHashes.CarbonDioxide
				],
				can_tinker: false,
				baseTraitId: BASETRAIT_ID,
				baseTraitName: STRINGS.CREATURES.SPECIES.BEACHED_WATERCUPS.NAME);

			var decorPlant = gameObject.AddOrGet<PrickleGrass>();
			decorPlant.positive_decor_effect = POSITIVE_DECOR_EFFECT;
			decorPlant.negative_decor_effect = NEGATIVE_DECOR_EFFECT;

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				gameObject,
				this as IHasDlcRestrictions,
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
				Assets.GetAnim("beached_watercups_kanim"),
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
