using UnityEngine;

namespace Beached.Content.Defs.Ores
{
	public class CrackedNeutroniumConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.crackedNeutronium;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);

			if (prefab.TryGetComponent(out KPrefabID kPrefabID))
				kPrefabID.prefabSpawnFn += Util.KDestroyGameObject;

			return prefab;
		}
	}
}
