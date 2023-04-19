using Beached.Content.Defs.Items;
using Beached.Content.Scripts.Entities.Comets;
using UnityEngine;

namespace Beached.Content.Defs.Comets
{
    internal class SparklingVoidCometConfig : IEntityConfig
    {
        public const string ID = "Beached_SparklingVoidComet";
        public GameObject CreatePrefab()
        {
            var go = BaseCometConfig.BaseComet(
                ID, 
                STRINGS.COMETS.BEACHED_SPARKLINGVOIDCOMET.NAME,
                "beached_meteor_sparkling_void_kanim", 
                Elements.crackedNeutronium, 
                new Vector2(3f, 20f), 
                new Vector2(323.15f, 423.15f), 
                "Meteor_Medium_Impact", 
                1, 
                SimHashes.CarbonDioxide, 
                SpawnFXHashes.MeteorImpactMetal, 
                0.3f);
            
            var comet = go.GetComponent<Comet>();
            comet.explosionOreCount = new Vector2I(0, 0);
            comet.entityDamage = 7;
            comet.totalTileDamage = 0.3f;
            comet.affectedByDifficulty = false;
            comet.craterPrefabs = new[]
            {
                RareGemsConfig.STRANGE_MATTER
            };
            comet.EXHAUST_RATE = 10f;

            var trail = go.AddOrGet<CometTrail>();
            trail.color = Util.ColorFromHex("7441d2");

            return go;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) {  }

        public void OnSpawn(GameObject inst) { }
    }
}
