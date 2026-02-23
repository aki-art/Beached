/*using HarmonyLib;

namespace Beached.Patches
{
	public class ComplexRecipePatch
	{
		[HarmonyPatch(typeof(ComplexRecipe), MethodType.Constructor, [
			typeof(string),
			typeof(ComplexRecipe.RecipeElement[]),
			typeof(ComplexRecipe.RecipeElement[])
			])]
		public class ComplexRecipe_Ctor_Patch
		{
			public static void Postfix(ComplexRecipe __instance)
			{

			}
		}
	}
}
*/