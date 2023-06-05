using UnityEngine;

namespace Beached.Content.Scripts.Entities.Comets
{
	public class SparkleComet : Comet
	{
		public override void SpawnCraterPrefabs()
		{
			if (craterPrefabs != null && craterPrefabs.Length != 0)
			{
				var position = transform.position with { z = -19.5f };
				position += Vector3.up;
				var spawn = Util.KInstantiate(Assets.GetPrefab(craterPrefabs.GetRandom()), position);
				spawn.SetActive(true);
			}
		}
	}
}
