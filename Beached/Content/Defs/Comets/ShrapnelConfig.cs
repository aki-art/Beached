using Beached.Content.Scripts.Entities;
using Beached.Utils;
using UnityEngine;

namespace Beached.Content.Defs.Comets
{
    public class ShrapnelConfig : IEntityConfig
    {
        public const string ID = "Beached_ShrapnelComet";

        public GameObject CreatePrefab()
        {
            var go = EntityTemplates.CreateEntity(ID, STRINGS.COMETS.SHRAPNEL.NAME, true);

            go.AddOrGet<SaveLoadRoot>();
            go.AddOrGet<LoopingSounds>();

            var shrapnel = go.AddOrGet<Shrapnel>();
            shrapnel.entityDamage = 2;
            shrapnel.totalTileDamage = 0.15f;
            shrapnel.splashRadius = 0;
            shrapnel.impactSound = "Meteor_Small_Impact";
            shrapnel.flyingSoundID = 0;
            shrapnel.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
            shrapnel.explosionOreCount = new Vector2I(1, 2);

            var primaryElement = go.AddOrGet<PrimaryElement>();
            primaryElement.SetElement(SimHashes.Iron);
            primaryElement.Temperature = MiscUtil.CelsiusToKelvin(500);

            var kbac = go.AddOrGet<KBatchedAnimController>();
            kbac.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim("meteor_rock_kanim")
            };

            kbac.isMovable = true;
            kbac.initialAnim = "fall_loop";
            kbac.initialMode = KAnim.PlayMode.Loop;
            kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

            var collider = go.AddOrGet<KCircleCollider2D>();
            collider.radius = 0.5f;

            go.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            go.AddTag(GameTags.Comet);

            return go;
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
