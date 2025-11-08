using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	internal class FossilMinePatch
	{

		[HarmonyPatch(typeof(FossilMine), "OnSpawn")]
		public class FossilMine_OnSpawn_Patch
		{
			public static void Postfix(FossilMine __instance)
			{
				__instance.workable.requiredSkillPerk = BSkillPerks.CanFindMoreTreasures.Id;
				__instance.workable.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
				__instance.workable.SkillExperienceSkillGroup = BSkillGroups.PRECISION_ID;
			}
		}
	}
}
