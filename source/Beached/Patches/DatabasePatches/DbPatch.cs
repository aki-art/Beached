using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Flora;
using Beached.Content.ModDb;
using Database;
using FUtility;
using HarmonyLib;
using UnityEngine;
using static FUtility.CONSTS;
using static FUtility.CONSTS.SUB_BUILD_CATEGORY;

namespace Beached.Patches.DatabasePatches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				BRoomTypes.ModifyConstraintRules();
				BDuplicants.Register(__instance.Personalities);
				BTraits.Register();
				BCritterTraits.Register();
				BAccessories.Register(__instance.Accessories, __instance.AccessorySlots);

				RegisterBuildings();

				ModAssets.LoadAssets();

				PlanScreen.iconNameMap.Add(HashCache.Get().Add(BDb.poisBuildCategory), ModAssets.Sprites.BUILDCATEGORY_POIS);

				// modify status item of "DryingOut", which is an otherwise unused status with no icon set
				__instance.CreatureStatusItems.DryingOut = new StatusItem(
					"DryingOut",
					"CREATURES",
					ModAssets.Sprites.STATUSITEM_DRIEDOUT,
					StatusItem.IconType.Custom,
					NotificationType.BadMinor,
					false,
					OverlayModes.None.ID)
				{
					resolveStringCallback = (str, data) => str
				};

				BDb.OnDbInit();
				BTags.OnDbInit();
			}

			[HarmonyPriority(Priority.Low)]
			public static void LatePostfix(Db __instance)
			{
				AddMeatsToCarnivore(__instance);
				AddSeedsToGMOOK(__instance);
			}

			private static void AddSeedsToGMOOK(Db instance)
			{
				var achievement = instance.ColonyAchievements.GMOOK;
				achievement.requirementChecklist.Add(new AnalyzeSeed(PoffShroomConfig.ID));
				achievement.requirementChecklist.Add(new AnalyzeSeed(PipTailConfig.ID));
				achievement.requirementChecklist.Add(new AnalyzeSeed(AlgaeCellConfig.ID));
			}

			private static void AddMeatsToCarnivore(Db __instance)
			{
				var items = __instance.ColonyAchievements.EatkCalFromMeatByCycle100.requirementChecklist;

				foreach (var requirement in items)
				{
					if (requirement is EatXCaloriesFromY foodRequirement)
					{
						var meats = Assets.GetPrefabsWithTag(BTags.meat);
						foreach (var item in foodRequirement.fromFoodType)
						{
							// collect meats from other mods
							if (Assets.TryGetPrefab(item) is GameObject prefab)
								prefab.AddTag(BTags.meat);
						}

						foreach (var meat in meats)
						{
							var prefabId = meat.PrefabID().ToString();
							if (foodRequirement.fromFoodType.Contains(prefabId))
								foodRequirement.fromFoodType.Add(prefabId);
						}

						/*
												foodRequirement.fromFoodType.AddRange(
												[
													SmokedMeatConfig.ID,
													SmokedFishConfig.ID,
													HighQualityMeatConfig.ID,
													LegendarySteakConfig.ID,
												]);*/

						break;
					}
				}
			}

			private static void RegisterBuildings()
			{
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.POWER, AmmoniaGeneratorConfig.ID, Power.GENERATORS, MethaneGeneratorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.UTILITIES, MossBedConfig.ID, Utilities.OTHER_UTILITIES, ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, MiniFridgeConfig.ID, Food.STORAGE, ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, DNAInjectorConfig.ID, Food.RANCHING, EggIncubatorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, CollarDispenserConfig.ID, Food.RANCHING, DNAInjectorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.BASE, LaboratoryTileConfig.ID, Base.TILES, PlasticTileConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, WoodCarvingConfig.ID, Furniture.DISPALY, MarbleSculptureConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, SandBoxConfig.ID, Furniture.DISPALY, WoodCarvingConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, ChimeConfig.ID, Furniture.RECREATION, FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, SmallAquariumConfig.ID, Furniture.DISPALY, FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, MudStomperConfig.ID, Refining.MATERIALS);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, SmokingRackConfig.ID, Food.COOKING);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, SpinnerConfig.ID, Refining.MATERIALS, RockCrusherConfig.ID);

				BuildingUtil.AddToResearch(ChimeConfig.ID, TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(SandBoxConfig.ID, TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(SmokingRackConfig.ID, TECH.FOOD.RANCHING);
				BuildingUtil.AddToResearch(SpinnerConfig.ID, TECH.SOLIDS.SMELTING);

			}
		}
	}
}
