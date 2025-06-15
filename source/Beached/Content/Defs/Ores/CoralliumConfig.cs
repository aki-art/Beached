using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Defs.Ores
{
	public class CoralliumConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.corallium;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);
			prefab.AddOrGet<Corallium>();

			return prefab;
		}
	}
}
