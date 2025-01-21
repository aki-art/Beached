using Beached.Content.Defs.Entities.Critters.Karacoos;
using UnityEngine;

namespace Beached.Content.Defs.Foods
{
	public class InfertileEggConfig : IEntityConfig
	{
		public const string ID = "Beached_InfertileEgg";
		public const float DEFAULT_MASS = 1f;

		public GameObject CreatePrefab()
		{
			var prefab = EggConfig.CreateEgg(
				ID,
				STRINGS.ITEMS.FOOD.BEACHED_INFERTILEEGG.NAME,
				STRINGS.ITEMS.FOOD.BEACHED_INFERTILEEGG.DESC,
				KaracooConfig.ID,
				"beached_infertile_egg_kanim",
				DEFAULT_MASS,
				9999,
				0,
				DlcManager.AVAILABLE_ALL_VERSIONS);

			prefab.RemoveTag(GameTags.IncubatableEgg);
			prefab.RemoveTag(GameTags.Egg);
			prefab.AddTag(BTags.karacooSittable);
			prefab.AddTag(GameTags.Organics);

			SymbolOverrideControllerUtil.AddToPrefab(prefab);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			DiscoveredResources.Instance.Discover(ID.ToTag(), DiscoveredResources.GetCategoryForTags(inst.GetComponent<KPrefabID>().Tags));
		}
	}
}
