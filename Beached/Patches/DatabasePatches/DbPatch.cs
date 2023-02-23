using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Sicknesses;
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
                // do not reorder randomly, loading order matters!
                BAttributes.Register(__instance.Attributes);
                BAmounts.Register(__instance.Amounts);
                BAssignableSlots.Register(__instance.AssignableSlots);
                BChoreGroups.Register(__instance.ChoreGroups);
                BStatusItems.Register();
                BSkillGroups.Register(__instance.SkillGroups);
                BSkillPerks.Register(__instance.SkillPerks);
                BSkills.Register(__instance.Skills);
                BSicknesses.Register(__instance.Sicknesses);
                BTraits.Register();
                BRoomTypes.Register(__instance.RoomTypes);
                BRoomTypes.ModifyConstraintRules();

                RegisterBuildings();

                ModAssets.LoadAssets();
                
                PlanScreen.iconNameMap.Add(HashCache.Get().Add(BDb.poisBuildCategory), ModAssets.Sprites.BUILDCATEGORY_POIS);
            }

            private static void RegisterBuildings()
            {
                ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.POWER, AmmoniaGeneratorConfig.ID, "Default", MethaneGeneratorConfig.ID);
                ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.UTILITIES, MossBedConfig.ID, "Default", ExteriorWallConfig.ID);
                ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FOOD, MiniFridgeConfig.ID, "Default", ExteriorWallConfig.ID);
                ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, LaboratoryTileConfig.ID, "Tiles", PlasticTileConfig.ID);
            }
        }
    }
}
