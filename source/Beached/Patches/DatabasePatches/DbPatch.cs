using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.ModDb;
using Database;
using HarmonyLib;

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

				Log.Debug("PERSONALITIES");
				foreach (var personality in __instance.Personalities.resources)
				{
					Log.Debug($"- {personality.Id} {personality.nameStringKey} {personality.congenitaltrait}");
				}
				BTraits.Register();
				BCritterTraits.Register();
				BAccessories.Register(__instance.Accessories, __instance.AccessorySlots);

				AddMeatsToCarnivore(__instance);
				AddSeedsToGMOOK(__instance);

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
						foodRequirement.fromFoodType.AddRange(
						[
							SmokedMeatConfig.ID,
							SmokedFishConfig.ID,
							HighQualityMeatConfig.ID,
							LegendarySteakConfig.ID,
						]);

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
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, ChimeConfig.ID, "decor", FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, SmallAquariumConfig.ID, "decor", FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.REFINING, MudStomperConfig.ID, FUtility.CONSTS.SUB_BUILD_CATEGORY.Refining.MATERIALS);


				FUtility.BuildingUtil.AddToResearch(ChimeConfig.ID, FUtility.CONSTS.TECH.DECOR.INTERIOR_DECOR);
			}
		}
	}
}
