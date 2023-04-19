using UnityEngine;

namespace Beached.Content.Scripts.Entities.Comets
{
    public class CometTrail : KMonoBehaviour
    {
        [SerializeField] public Color color;

        private ParticleSystem particleSystem;
        private Transform trail;

        public override void OnSpawn()
        {
            base.OnSpawn();
            trail = Instantiate(ModAssets.Prefabs.cometTrailFx).transform;
            trail.localScale = new Vector3(0.4f, 0.4f);
            trail.position = transform.position;
            if(trail.TryGetComponent(out particleSystem))
            {
                var main = particleSystem.main;
                main.startColor = color;
            }
        }

        void Update()
        {
            trail.position = transform.position;
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                //GameScheduler.Instance.Schedule("destroy trail", 3000f, _ => particleSystem.Stop());
            }
        }
    }
}
