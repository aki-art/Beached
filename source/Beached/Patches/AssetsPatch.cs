using Beached.Content;
using Beached.Content.Defs.Entities;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using Klei.AI;
using PeterHan.PLib.Core;
using UnityEngine;

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


			private static Color Premultiply(Color color)
			{
				var a = color.a;
				return new Color(color.r * a, color.g * a, color.b * a, a);
			}


			public static void OnPostprocessTexture(UnityEngine.Texture2D texture)
			{

				var width = texture.width;
				var height = texture.height;
				for (var x = 0; x < width; ++x)
				{
					for (var y = 0; y < height; ++y)
					{
						var color = texture.GetPixel(x, y);
						texture.SetPixel(x, y, Premultiply(color));
					}
				}

				texture.Apply();
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix()
			{
				var group = Assets.GetPrefab(DreckoConfig.ID).GetComponent<KBatchedAnimController>().batchGroupID;
				Log.Debug("DRECKO ANIM");
				Log.Debug($"{group: D}");
				var wall = Assets.GetAnim("beached_wall_skin_aqua_water_kanim");
				foreach (var texture in wall.textureList)
				{
					Log.Debug($"texture {texture.name} {texture.wrapMode}");
					texture.wrapMode = UnityEngine.TextureWrapMode.Repeat;

					OnPostprocessTexture(texture);

				}
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
