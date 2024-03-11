using UnityEngine;

namespace Beached.Utils
{
	public class SpawnUtil
	{
		public readonly struct Spawnable(Tag prefabId, float mass = -1, float temperature = -1, int count = 1)
		{
			public readonly Tag prefabId = prefabId;
			public readonly float mass = mass;
			public readonly float temperature = temperature;
			public readonly int count = count;

			public GameObject Spawn(Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures)
			{
				var result = MiscUtil.Spawn(prefabId, position, sceneLayer);
				if (result != null && result.TryGetComponent(out PrimaryElement primaryElement))
				{
					if (mass != -1)
						primaryElement.SetMass(mass);

					if (temperature != -1)
						primaryElement.SetTemperature(temperature);
				}

				return result;
			}


			public GameObject Spawn(int cell, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures)
			{
				return Spawn(Grid.CellToPosCCC(cell, sceneLayer), sceneLayer);
			}
		}
	}
}
