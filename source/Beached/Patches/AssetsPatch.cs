using Beached.Content.ModDb;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using HarmonyLib;

namespace Beached.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				ModAssets.LoadSprites(__instance);
				Assets.RegisterOnAddPrefab(AcidVulnerableCreature.OnAddPrefab);
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix()
			{
				BDb.AddRecipes();
				DNAInjector.InitializeOptions();
			}
		}
	}
}
