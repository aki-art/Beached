using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
    internal class WaterCupsConfig : IEntityConfig
    {
        public const string ID = "Beached_WaterCups";
        public const string SEED_ID = "Beached_WaterCupsSeed";
        public const string PREVIEW_ID = "Beached_WaterCupsPreview";

        public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;
        public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;

        public GameObject CreatePrefab()
        {
            var gameObject = EntityTemplates.CreatePlacedEntity(
                ID,
                STRINGS.CREATURES.SPECIES.WATERCUPS.NAME,
                STRINGS.CREATURES.SPECIES.WATERCUPS.DESC,
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

            EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288.15f, 293.15f, 323.15f, 373.15f, new SimHashes[]
            {
                SimHashes.Oxygen,
                Elements.saltyOxygen,
                SimHashes.ContaminatedOxygen,
                SimHashes.CarbonDioxide
            }, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CylindricaOriginal", STRINGS.CREATURES.SPECIES.WATERCUPS.NAME);

            var prickleGrass = gameObject.AddOrGet<PrickleGrass>();
            prickleGrass.positive_decor_effect = POSITIVE_DECOR_EFFECT;
            prickleGrass.negative_decor_effect = NEGATIVE_DECOR_EFFECT;

            EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "CylindricaSeed", STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.NAME, STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.DESC, Assets.GetAnim("seed_potted_cylindricafan_kanim"), "object", 1, new List<Tag>
            {
                GameTags.DecorSeed
            }, SingleEntityReceptacle.ReceptacleDirection.Top, default, 12, STRINGS.CREATURES.SPECIES.WATERCUPS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), PREVIEW_ID, Assets.GetAnim("beached_watercups_kanim"), "place", 1, 1);

            return gameObject;
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
