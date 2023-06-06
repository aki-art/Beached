using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Foods;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Sicknesses;
using Database;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches.DatabasePatches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				Log.Debug("DB POSTFIX ------------------------------------");

				// do not reorder randomly, loading order matters!
				BAttributes.Register(__instance.Attributes);
				BAccessories.Register(__instance.Accessories, __instance.AccessorySlots);
				BAmounts.Register(__instance.Amounts);
				BAssignableSlots.Register(__instance.AssignableSlots);
				BChoreGroups.Register(__instance.ChoreGroups);
				BStatusItems.Register(__instance);
				BSkillGroups.Register(__instance.SkillGroups);
				BSkillPerks.Register(__instance.SkillPerks);
				BSkills.Register(__instance.Skills);
				BSicknesses.Register(__instance.Sicknesses);
				BTraits.Register();
				BCritterTraits.Register();
				BRoomTypes.Register(__instance.RoomTypes);
				BRoomTypes.ModifyConstraintRules();
				BGameplayEvents.Register(__instance.GameplayEvents);
				BGameplaySeasons.Register(__instance.GameplaySeasons);
				BDuplicants.Register(__instance.Personalities);
				// TechTreeTitles has to be patched on class
				//BTechs.Register(__instance.Techs);

				var items = __instance.ColonyAchievements.EatkCalFromMeatByCycle100.requirementChecklist;

				foreach (var requirement in items)
				{
					if (requirement is EatXCaloriesFromY foodRequirement)
					{
						foodRequirement.fromFoodType.AddRange(new List<string>()
						{
							SmokedMeatConfig.ID,
							SmokedFishConfig.ID,
							HighQualityMeatConfig.ID,
							LegendarySteakConfig.ID,
						});

						break;
					}
				}

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
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Low)]
			public static void LatePostfix()
			{
			}

			private static void RegisterBuildings()
			{
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.POWER, AmmoniaGeneratorConfig.ID, "Default", MethaneGeneratorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.UTILITIES, MossBedConfig.ID, "Default", ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, MiniFridgeConfig.ID, "Default", ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, DNAInjectorConfig.ID, "Default", EggIncubatorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, LaboratoryTileConfig.ID, "Tiles", PlasticTileConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, WoodCarvingConfig.ID, "decor", MarbleSculptureConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, ChimeConfig.ID, "decor", FlowerVaseConfig.ID);
			}
		}
	}
}
