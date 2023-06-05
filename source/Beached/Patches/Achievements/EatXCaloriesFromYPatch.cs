using Beached.Content.Defs.Foods;
using System.Collections.Generic;

namespace Beached.Patches.Achievements
{
	public class EatXCaloriesFromYPatch
	{
		//[HarmonyPatch(typeof(EatXCaloriesFromY), MethodType.Constructor, typeof(int), typeof(List<string>))]
		public static class PatchCarnivoreAchievment
		{
			public static void Postfix(List<string> fromFoodType)
			{
				if (!fromFoodType.Contains(SmokedMeatConfig.ID))
				{
					fromFoodType.Add(SmokedMeatConfig.ID);
				}
			}
		}

	}
}
