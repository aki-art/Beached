using UnityEngine;
using TUNING;
using Beached.Content.Scripts;

namespace Beached.Content.Defs.Entities.Plants
{
    public class PoffShroomConfig : IEntityConfig
    {
        public const string ID = "Beached_PoffShroom";
        public const string BASE_TRAIT_ID = "Beached_PoffShroomOriginal";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                STRINGS.CREATURES.SPECIES.POFFSHROOM.NAME,
                STRINGS.CREATURES.SPECIES.POFFSHROOM.DESC,
                40f,
                Assets.GetAnim("beached_poffshroom_kanim"),
                "idle_full",
                Grid.SceneLayer.Building,
                1,
                1,
                DECOR.NONE);

            EntityTemplates.ExtendEntityToBasicPlant(
                prefab,
                baseTraitId: BASE_TRAIT_ID,
                baseTraitName: STRINGS.CREATURES.SPECIES.POFFSHROOM.NAME);

            //prefab.AddOrGet<StandardCropPlant>();

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
            //EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "MushroomPlant_preview", Assets.GetAnim("fungusplant_kanim"), "place", 1, 2);

            prefab.AddOrGet<PoffShroom>();

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
