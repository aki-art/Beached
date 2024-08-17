using Beached.Content.ModDb;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using PeterHan.PLib.Core;

namespace Beached.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix()
			{
				Assets.RegisterOnAddPrefab(AcidVulnerableCreature.OnAddPrefab);
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix()
			{
				BDb.AddRecipes();
				DNAInjector.InitializeOptions();

				EdiblePatch.Edible_AddOnConsumeEffects_Patch.Patch(Mod.harmonyInstance);

				PGameUtils.CopySoundsToAnim("beached_mussel_giblets_impact_kanim", "paint_impact_kanim");
			}
		}
	}
}
