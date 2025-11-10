/*using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class MovablePatch
	{
		[HarmonyPatch(typeof(Movable), "OnSpawn")]
		public class Movable_OnSpawn_Patch
		{
			public static void Postfix(Movable __instance)
			{
				if (__instance.requiredSkillPerk == Db.Get().SkillPerks.CanWrangleCreatures.Id)
				{
					__instance.requiredSkillPerk = BSkillPerks.CAN_MOVE_CRITTERS_ID;
					__instance.UpdateStatusItem();
				}
			}
		}
	}
}
*/