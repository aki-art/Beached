using UnityEngine;

namespace Beached.Content.Scripts
{
    public class AcidVulnerable : KMonoBehaviour, ISim1000ms//, IGameObjectEffectDescriptor//, IWiltCause
    {
        private int acidIdx;

        [MyCmpGet]
        private Health health;

        [MyCmpGet]
        private KBatchedAnimController kbac;

        [SerializeField]
        public float acidDamage;

        private bool isHurtByAcid;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            acidIdx = Elements.SulfurousWater.Get().idx;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            /* if(!BeachedWorldManager.Instance.IsBeachedContentActive)
             {
                 isHurtByAcid = false;
                 return;
             }*/

            isHurtByAcid = acidDamage > 0;
        }

        public void Sim1000ms(float dt)
        {
            if (!isHurtByAcid)
            {
                return;
            }

            var pos = Grid.PosToCell(transform.position);
            if (Grid.ElementIdx[pos] == acidIdx)
            {
                health.Damage(acidDamage);
                // TODO: sizzle
                // todo: acid resistance
            }
        }
    }
}
