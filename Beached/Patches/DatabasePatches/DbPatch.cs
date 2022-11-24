using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Sicknesses;
using HarmonyLib;
using Klei.AI;
using System;

namespace Beached.Patches.DatabasePatches
{
    public class DbPatch
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix(Db __instance)
            {
                BAttributes.Register(__instance.Attributes);
                BChoreGroups.Register(__instance.ChoreGroups);
                BSkillGroups.Register(__instance.SkillGroups);
                BSkillPerks.Register(__instance.SkillPerks);
                BSkills.Register(__instance.Skills);
                BSicknesses.Register(__instance.Sicknesses);

                RegisterBuildings();

                ModAssets.LoadAssets();
            }

            private static void RegisterBuildings()
            {
                ModUtil.AddBuildingToPlanScreen("Power", AmmoniaGeneratorConfig.ID);
            }
        }
    }
}
