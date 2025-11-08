using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class LegacyModMainPatch
	{
		[HarmonyPatch(typeof(LegacyModMain), "Load")]
		public class LegacyModMain_Load_Patch
		{
			public static void Postfix()
			{
				Recipes.AddRecipes();
			}
		}
	}


	[HarmonyPatch(typeof(ComplexRecipeManager), "DeriveRecipiesFromSource")]
	public class ComplexRecipeManager_DeriveRecipiesFromSource_Patch
	{
		public static void Postfix(ComplexRecipe sourceRecipe)
		{
			Log.Debug("-----------------------------------------------");
			Log.Debug($"RECIPE: {sourceRecipe.id}");
			if (sourceRecipe.ingredients != null)
			{
				foreach (var ingredient in sourceRecipe.ingredients)
				{
					for (var index = 0; index < ingredient.possibleMaterials.Length; ++index)
					{
						Log.Debug($"\t- {ingredient.possibleMaterials[index]}, exists: {(Assets.TryGetPrefab(ingredient.possibleMaterials[index]) != null)}");
					}
				}
			}
		}
	}
}
