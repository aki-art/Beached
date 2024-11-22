using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class SkeletonChairConfig : IEntityConfig
	{
		public const string ID = "Beached_SkeletonChair";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.ENTITIES.BEACHED_SKELETON_CHAIR.NAME,
				STRINGS.ENTITIES.BEACHED_SKELETON_CHAIR.DESCRIPTION,
				400 + 30 + 2,
				Assets.GetAnim("beached_skeletonchair_kanim"),
				"idle",
				Grid.SceneLayer.Building,
				2,
				3,
				TUNING.DECOR.NONE,
				defaultTemperature: MiscUtil.CelsiusToKelvin(30));

			prefab.AddOrGet<Demolishable>();
			prefab.AddOrGet<POITechItemUnlockWorkable>().workTime = 5f;

			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];

			var unlocks = prefab.AddOrGetDef<POITechItemUnlocks.Def>();
			unlocks.POITechUnlockIDs = [
				IceMachineConfig.ID,
				BeachChairConfig.ID,
				IceCooledFanConfig.ID ];
			unlocks.PopUpName = STRINGS.UI.BEACHED_MISC.POI_UNLOCK_TITLE;
			unlocks.animName = "ceres_remote_archive_kanim";

			prefab.AddOrGet<Prioritizable>();

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst) { }
	}
}
