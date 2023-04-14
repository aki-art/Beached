using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
    public class JellyfishStrobilaConfig : IEntityConfig
    {
        public const string ID = "Beached_JellyfishStrobila";

        public GameObject CreatePrefab()
        {
            var anim = Assets.GetAnim("beached_jellyfish_strobila_kanim");

            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                STRINGS.CREATURES.SPECIES.BEACHED_JELLYFISHSTROBILA.NAME,
                STRINGS.CREATURES.SPECIES.BEACHED_JELLYFISHSTROBILA.DESCRIPTION,
                200f,
                anim,
                "idle_loop",
                Grid.SceneLayer.Building,
                1,
                2,
                TUNING.DECOR.BONUS.TIER2,
                default,
                SimHashes.Creature);

            prefab.AddOrGet<EntombVulnerable>();

            var pressureVulnerable = prefab.AddOrGet<PressureVulnerable>();
            pressureVulnerable.Configure(CoralTemplate.ALL_WATERS);

            prefab.AddOrGet<WiltCondition>();
            prefab.AddOrGet<Prioritizable>();
            prefab.AddOrGet<Uprootable>();
            prefab.AddOrGet<UprootedMonitor>();
            prefab.AddOrGet<TemperatureVulnerable>().Configure(
                MiscUtil.CelsiusToKelvin(10),
                MiscUtil.CelsiusToKelvin(0),
                MiscUtil.CelsiusToKelvin(65),
                MiscUtil.CelsiusToKelvin(80));

            prefab.AddOrGet<OccupyArea>().objectLayers = new[]
            {
                ObjectLayer.Building
            };

            prefab.GetComponent<KPrefabID>().prefabInitFn += inst =>
            {
                if (inst.TryGetComponent(out PressureVulnerable pressureVulnerable2))
                {
                    foreach (var safeElement in CoralTemplate.ALL_WATERS)
                        pressureVulnerable2.safe_atmospheres.Add(ElementLoader.FindElementByHash(safeElement));
                }
            };

            prefab.AddOrGet<Strobila>();

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
