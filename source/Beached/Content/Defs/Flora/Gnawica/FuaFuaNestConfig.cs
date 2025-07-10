using Beached.Content.Scripts.Entities.AI.Fua;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Flora.Gnawica
{
	public class FuaFuaNestConfig : IEntityConfig
	{
		public const string ID = "Beached_FuaFuaNest";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_FUAFUANEST.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_FUAFUANEST.DESC,
				30f,
				Assets.GetAnim("beached_fue_nest_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				2,
				2,
				TUNING.DECOR.BONUS.TIER2);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = 1f;

			var nest = prefab.AddComponent<FuaNest>();
			nest.furPerCycle = 5f;

			var diet = new Diet(
			[
					new Diet.Info(
					[Elements.bone.CreateTag()],
					Elements.fuzz.CreateTag(),
					BeeHiveTuning.CALORIES_PER_KG_OF_ORE,
					BeeHiveTuning.POOP_CONVERSTION_RATE)
			]);

			//prefab.AddOrGetDef<BeehiveCalorieMonitor.Def>().diet = diet;

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
