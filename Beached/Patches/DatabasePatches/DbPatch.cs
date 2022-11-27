using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Sicknesses;
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
                // do not reorder, loading order matters!
                BAttributes.Register(__instance.Attributes);
                BAssignableSlots.Register(__instance.AssignableSlots);
                BChoreGroups.Register(__instance.ChoreGroups);
                BSkillGroups.Register(__instance.SkillGroups);
                BSkillPerks.Register(__instance.SkillPerks);
                BSkills.Register(__instance.Skills);
                BSicknesses.Register(__instance.Sicknesses);
                BTraits.Register();

                RegisterBuildings();

                ModAssets.LoadAssets();

                RoomConstraints.BED_SINGLE.stomp_in_conflict ??= new List<RoomConstraints.Constraint>();
                RoomConstraints.BED_SINGLE.stomp_in_conflict.Add(RoomConstraints.REC_BUILDING);
                RoomConstraints.LUXURY_BED_SINGLE.stomp_in_conflict ??= new List<RoomConstraints.Constraint>();
                RoomConstraints.LUXURY_BED_SINGLE.stomp_in_conflict.Add(RoomConstraints.REC_BUILDING);
            }

            private static void RegisterBuildings()
            {
                ModUtil.AddBuildingToPlanScreen("Power", AmmoniaGeneratorConfig.ID);
            }
        }
    }
}
