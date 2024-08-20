using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Entities.Critters;
using Beached.Content.Scripts;
using System.Collections.Generic;
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

			prefab.AddOrGet<GenericUnlockablePOIWorkable>().workTime = 5f;
			var def = prefab.AddOrGetDef<GenericUnlockablePOI.Def>();
			def.techUnlockIDs =
			[
				CollarDispenserConfig.ID
			];

			def.popUpName = global::STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
			def.animName = "ceres_remote_archive_kanim";
			def.loreUnlockId = "notes_welcometoceres";
			def.spawnPrefabs =
			[
				MuffinConfig.ID,
				MuffinConfig.ID
			];

			prefab.AddOrGet<Prioritizable>();

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
