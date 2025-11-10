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
}
