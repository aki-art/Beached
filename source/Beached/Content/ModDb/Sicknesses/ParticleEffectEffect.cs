using Klei.AI;
using UnityEngine;

namespace Beached.Content.ModDb.Sicknesses
{
	public class ParticleEffectEffect : Sickness.SicknessComponent
	{
		private readonly GameObject prefab;
		private readonly Vector3 offset;

		public ParticleEffectEffect(GameObject prefab, Vector3 offset)
		{
			this.prefab = prefab;
			this.offset = offset;
		}

		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			if (prefab == null)
			{
				Log.Warning("Particle prefab is null");
				return null;
			}

			var effect = Object.Instantiate(prefab);
			effect.transform.position = go.transform.position + offset;
			effect.transform.SetParent(go.transform);
			effect.SetActive(true);

			effect.TryGetComponent(out ParticleSystem particleSystem);
			particleSystem.Play();

			return particleSystem;
		}

		public override void OnCure(GameObject go, object data)
		{
			if (data is ParticleSystem particles)
			{
				// this allows the last particles to gracefully disappear
				var emission = particles.emission;
				emission.rateOverTime = 0;

				GameScheduler.Instance.Schedule("remove particles", 5f, _ => particles.Stop());
			}
		}
	}
}
