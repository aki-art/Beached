using Beached.Content;
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

				__instance.BuildingStatusItems.ColonyLacksRequiredSkillPerk.allowMultiples = true;
				__instance.BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk.allowMultiples = true;

				BDb.OnDbInit(__instance);
				BTags.OnDbInit();

				BionicOilMonitor.LUBRICANT_TYPE_EFFECT[Elements.mucus] = BionicOilMonitor.CreateFreshOilEffectVariation(Elements.mucus.ToString(), -10f / CONSTS.CYCLE_LENGTH, 2f);
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
							if (Assets.TryGetPrefab(item) is GameObject prefab)
								prefab.AddTag(BTags.meat);
						}

						foreach (var meat in meats)
						{
							var prefabId = meat.PrefabID().ToString();
							if (foodRequirement.fromFoodType.Contains(prefabId))
								foodRequirement.fromFoodType.Add(prefabId);
						}

						break;
					}
				}
			}
		}
	}
}
