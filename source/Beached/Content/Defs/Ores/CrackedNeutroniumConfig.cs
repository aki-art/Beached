using UnityEngine;

namespace Beached.Content.Defs.Ores
{
#if ELEMENTS
	public class CrackedNeutroniumConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.crackedNeutronium;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);

			if (prefab.TryGetComponent(out KPrefabID kPrefabID))
			{
				kPrefabID.prefabSpawnFn += Util.KDestroyGameObject;
				// TODO: If rocketry expanded is here, convert self into Dust instead
			}

			return prefab;
		}
	}
#endif
}
