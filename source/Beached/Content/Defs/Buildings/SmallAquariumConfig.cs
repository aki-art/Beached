using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class SmallAquariumConfig : IBuildingConfig
	{
		public const string ID = "Beached_SmallAquarium";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"beached_small_aquarium_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.TRANSPARENTS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.ViewMode = OverlayModes.LiquidConduits.ID;
			def.AudioCategory = AUDIO.CATEGORY.GLASS;
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.InputConduitType = ConduitType.Liquid;

			return def;
		}


		[HarmonyPatch(typeof(PlanterSideScreen), "AdditionalCanDepositTest")]
		public class PlanterSideScreen_AdditionalCanDepositTest_Patch
		{
			public static void Prefix(PlanterSideScreen __instance)
			{
				Log.Debug("selectedDepositObjectTag.IsValid");
				Log.Debug(__instance.selectedDepositObjectTag.IsValid);
				Log.Debug("IsValidPlantableSeed");
				Log.Debug(__instance.selectedDepositObjectTag.ProperNameStripLink());
				Log.Debug(__instance.selectedDepositObjectAdditionalTag.ProperNameStripLink());
				Log.Debug(PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(__instance.selectedDepositObjectTag, __instance.selectedDepositObjectAdditionalTag));
				Log.Debug("IsValidPlantableSeed");
				Log.Debug(__instance.selectedDepositObjectTag.ProperNameStripLink());
				PlantablePlot targetReceptacle = __instance.targetReceptacle as PlantablePlot;
				Log.Debug("ValidPlant");
				Log.Debug(targetReceptacle.ValidPlant);
				WorldContainer myWorld = __instance.targetReceptacle.GetMyWorld();
				Log.Debug("count");
				Log.Debug(myWorld.worldInventory.GetCountWithAdditionalTag(__instance.selectedDepositObjectTag, __instance.selectedDepositObjectAdditionalTag, myWorld.IsModuleInterior));
			}
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			base.ConfigureBuildingTemplate(go, prefab_tag);
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			var storage = go.AddComponent<Storage>();
			storage.capacityKg = 20f;
			storage.storageFilters = STORAGEFILTERS.LIQUIDS;
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);


			var plantablePlot = go.AddOrGet<PlantablePlot>();
			plantablePlot.occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 0.0f);
			plantablePlot.AddDepositTag(BTags.smallAquariumSeed);
			plantablePlot.SetFertilizationFlags(false, true);
			plantablePlot.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);

			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<SmallAquarium>().minimumWaterLevel = 10f;
		}
	}
}
