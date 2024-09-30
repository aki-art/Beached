using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Flora;
using Beached.Content.ModDb;
using Database;
using HarmonyLib;
using UnityEngine;

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

				var effect = __instance.effects.Get("MechanicalSurfboard");
				Log.Debug("effect: " + effect.Id);
				Log.Debug($"duration: " + effect.duration);
				Log.Debug($"emote: " + effect.emote?.Id);
				Log.Debug($"emote cooldown: " + effect.emoteCooldown);
				Log.Debug($"stompGroup: " + effect.stompGroup);
				Log.Debug($"stompPriority: " + effect.stompPriority);
				Log.Debug($"maxInitialDelay: " + effect.maxInitialDelay);

				var effect2 = __instance.effects.Get("RecentlyMechanicalSurfboard");
				Log.Debug("effect2: " + effect2.Id);
				Log.Debug($"duration: " + effect2.duration);
				Log.Debug($"emote: " + effect2.emote?.Id);
				Log.Debug($"emote cooldown: " + effect2.emoteCooldown);
				Log.Debug($"stompGroup: " + effect2.stompGroup);
				Log.Debug($"stompPriority: " + effect2.stompPriority);
				Log.Debug($"maxInitialDelay: " + effect2.maxInitialDelay);

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
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.POWER, AmmoniaGeneratorConfig.ID, "Default", MethaneGeneratorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.UTILITIES, MossBedConfig.ID, "Default", ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, MiniFridgeConfig.ID, "Default", ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, DNAInjectorConfig.ID, "Default", EggIncubatorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, CollarDispenserConfig.ID, "Default", DNAInjectorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, LaboratoryTileConfig.ID, "Tiles", PlasticTileConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, WoodCarvingConfig.ID, "decor", MarbleSculptureConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, SandBoxConfig.ID, "decor", WoodCarvingConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, ChimeConfig.ID, "decor", FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, SmallAquariumConfig.ID, "decor", FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.REFINING, MudStomperConfig.ID, FUtility.CONSTS.SUB_BUILD_CATEGORY.Refining.MATERIALS);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, SmokingRackConfig.ID, FUtility.CONSTS.SUB_BUILD_CATEGORY.Food.COOKING);

				FUtility.BuildingUtil.AddToResearch(ChimeConfig.ID, FUtility.CONSTS.TECH.DECOR.INTERIOR_DECOR);
				FUtility.BuildingUtil.AddToResearch(SandBoxConfig.ID, FUtility.CONSTS.TECH.DECOR.INTERIOR_DECOR);
				FUtility.BuildingUtil.AddToResearch(SmokingRackConfig.ID, FUtility.CONSTS.TECH.FOOD.RANCHING);
			}
		}
	}
}
