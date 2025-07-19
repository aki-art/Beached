using Beached.Content;
using Beached.Content.Defs.Entities;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using Klei.AI;
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
				BEntities.ModifyBaseEggChances();
				Assets.RegisterOnAddPrefab(AcidVulnerableCreature.OnAddPrefab);
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix()
			{
				BDb.SetMeatTags();

				DNAInjector.InitializeOptions();

				EdiblePatch.Edible_AddOnConsumeEffects_Patch.Patch(Mod.harmonyInstance);
				EdiblePatch.Edible_OnSpawn_Patch.Patch(Mod.harmonyInstance);
				DoorPatch.Door_UpdateAnimAndSoundParams_Patch.Patch(Mod.harmonyInstance);

				foreach (var geyser in Assets.GetPrefabsWithComponent<Geyser>())
				{
					if (geyser.HasTag(BTags.geyserNoTraits))
						continue;

					geyser.AddOrGet<Traits>();
					geyser.AddOrGet<Beached_GeyserTraits>();
				}

				PGameUtils.CopySoundsToAnim("beached_mussel_giblets_impact_kanim", "paint_impact_kanim");

#if WIKI
				FoodGraphGenerator.Generate(Path.Combine(FUtility.Utils.ModPath, "food_graphs.txt"));
#endif
			}
		}
	}
}
