using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class PurpleHangerConfig : IEntityConfig
	{
		public const string ID = "Beached_PurpleHanger";
		public const string SEED_ID = "Beached_PurpleHangerSeed";
		public const string BASETRAIT_ID = "Beached_PurpleHangerOriginal";
		public const string PREVIEW_ID = "Beached_PurpleHangerPreview";

		public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;
		public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_PURPLEHANGER.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_PURPLEHANGER.DESC,
				100f,
				Assets.GetAnim("beached_purplehanger2_kanim"),
				"idle_loop",
				Grid.SceneLayer.BuildingBack,
				1,
				3,
				DECOR.BONUS.TIER1);

			EntityTemplates.MakeHangingOffsets(prefab, 1, 3);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				288.15f,
				293.15f,
				323.15f,
				373.15f,
				[
					SimHashes.Oxygen,
					Elements.saltyOxygen,
					SimHashes.ContaminatedOxygen,
					SimHashes.CarbonDioxide,
					Elements.ammonia,
					SimHashes.Hydrogen,
					SimHashes.Helium
				],
				can_tinker: false,
				baseTraitId: BASETRAIT_ID,
				baseTraitName: STRINGS.CREATURES.SPECIES.BEACHED_PURPLEHANGER.NAME);

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				null,
				SeedProducer.ProductionType.Hidden,
				SEED_ID,
				STRINGS.CREATURES.SPECIES.SEEDS.BEACHED_PURPLEHANGER.NAME,
				STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.DESC,
				Assets.GetAnim("beached_watercups_seed_kanim"),
				"object",
				additionalTags: [GameTags.DecorSeed, BTags.decorSeedHanging],
				sortOrder: 12,
				domesticatedDescription: STRINGS.CREATURES.SPECIES.BEACHED_WATERCUPS.DOMESTICATEDDESC);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				PREVIEW_ID,
				Assets.GetAnim("beached_watercups_kanim"),
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
