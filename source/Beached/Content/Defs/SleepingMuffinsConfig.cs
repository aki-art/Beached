using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Defs
{
	internal class SleepingMuffinsConfig : IEntityConfig
	{
		public const string ID = "Beached_SleepingMuffings";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.ENTITIES.BEACHED_UNCONSCIOUS_CRITTERS.NAME,
				STRINGS.ENTITIES.BEACHED_UNCONSCIOUS_CRITTERS.DESCRIPTION,
				200f,
				Assets.GetAnim("beached_sleeping_muffins_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				4,
				1,
				TUNING.DECOR.BONUS.TIER1);

			var def = prefab.AddOrGetDef<UnlockableStuff.Def>();
			def.techUnlockIDs =
			[
				CollarDispenserConfig.ID
			];

			def.popUpName = global::STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
			def.animName = "beached_glaciermuffinsunlock_kanim";
			def.spawnPrefabs =
			[
				MuffinConfig.ID,
				MuffinConfig.ID
			];

			prefab.AddOrGet<Prioritizable>();

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
