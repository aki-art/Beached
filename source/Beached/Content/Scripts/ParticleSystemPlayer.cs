using System;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class ParticleSystemPlayer : KMonoBehaviour
	{
		[SerializeField] public float duration;

		[MyCmpReq] private ParticleSystem particleSystem;

		public event Action<GameObject> OnDie;

		private float elapsed;
		private bool isPlaying;

		public void Play()
		{
			particleSystem.Play();
			elapsed = 0;
			isPlaying = true;
		}

		public void Update()
		{
			if (!isPlaying)
				return;

			elapsed += Time.deltaTime;

			if (elapsed > duration)
			{
				particleSystem.Stop();
				isPlaying = false;
				OnDie?.Invoke(gameObject);
			}
		}

		public void Hide()
		{
			particleSystem.Clear();
		}
	}
}
