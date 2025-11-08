using UnityEngine;

namespace Beached.Content.Defs.Ores
{
	public class FuzzConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.fuzz;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID, [
				GameTags.BuildingFiber,
				GameTags.IndustrialIngredient
				]);

			Log.Debug($"CREATED FUZZ PREFAB: {prefab.PrefabID()}");
			return prefab;
		}
	}
}
