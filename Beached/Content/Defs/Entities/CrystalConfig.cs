using Beached.Content.Scripts;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
    public class CrystalConfig : IEntityConfig
    {
        public const string ID = "Beached_Crystal";

        public GameObject CreatePrefab()
        {
            var anim = Assets.GetAnim("test_crystal_kanim");

            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                "Crystal",
                "Placeholder",
                100f,
                anim,
                "1",
                Grid.SceneLayer.Building,
                1,
                1,
                TUNING.DECOR.BONUS.TIER4,
                default,
                Elements.Selenite);

            prefab.AddOrGet<GeoFormation>().allowDiagonalGrowth = true;
            //prefab.AddOrGet<CrystalFoundationMonitor>().needsFoundation = false;
            //prefab.AddOrGet<GrowingCrystal>();

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            inst.GetComponent<GeoFormation>();
        }
    }
}
