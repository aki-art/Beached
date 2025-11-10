using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class GlaciersConfig : IMultiEntityConfig
	{
		public const float WEIGHT_PER_TILE = 30;

		// dying glacier
		public const string
			MUFFINS = "Beached_Glacier_Muffins",
			VARIANT_A = "Beached_Glacier_VariantA",
			VARIANT_B = "Beached_Glacier_VariantB",
			VARIANT_C = "Beached_Glacier_VariantC";

		public List<GameObject> CreatePrefabs()
		{
			var standardRewards = new Tag[]
			{
				BasicForagePlantConfig.ID,
				BasicSingleHarvestPlantConfig.SEED_ID,
				SquirrelConfig.EGG_ID,
				DatabankHelper.TAG
			};

			var standardOffset = new Vector3(0, 1f, 0);

			return [
				CreateMuffins(),
				CreateGenericPrefab(VARIANT_A, "beached_glacier_variant_a_kanim", 2, 2, standardRewards, standardOffset),
				CreateGenericPrefab(VARIANT_B, "beached_glacier_variant_b_kanim", 2, 2, standardRewards, standardOffset),
				CreateGenericPrefab(VARIANT_C, "beached_glacier_variant_c_kanim", 2, 2, standardRewards, standardOffset)
			];
		}

		private static GameObject CreateMuffins()
		{
			var prefab = CreatePrefab(MUFFINS, 7, 3, "beached_glacier_muffins_kanim", [SleepingMuffinsConfig.ID]);

			prefab.AddOrGetDef<GlacierStoryTraitInitializer.Def>();
			prefab.GetComponent<Glacier>().showRewardSilhouette = false;

			return prefab;
		}

		private static GameObject CreateGenericPrefab(string id, string anim, int width = 2, int height = 2, Tag[] rewardDrops = null, Vector3 rewardDropOffset = default, string initialAnim = "object")
		{
			var name = STRINGS.ENTITIES.GLACIERS.BEACHED_GLACIER_SMALL_GENERIC.NAME;
			var description = $"{STRINGS.ENTITIES.GLACIERS.BEACHED_GLACIER_SMALL_GENERIC.DESCRIPTION}\n\n{STRINGS.ENTITIES.GLACIERS.GENERIC_THAW}";

			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				name,
				description,
				width * height * WEIGHT_PER_TILE,
				Assets.GetAnim(anim),
				"idle",
				Grid.SceneLayer.BuildingBack,
				width,
				height,
				TUNING.DECOR.BONUS.TIER2,
				element: SimHashes.Ice,
				additionalTags:
				[
					BTags.glacier
				],
				defaultTemperature: MiscUtil.CelsiusToKelvin(-30));

			var glacier = prefab.AddComponent<Glacier>();
			glacier.rewards = rewardDrops;
			glacier.offset = rewardDropOffset;
			glacier.initialAnim = initialAnim;

			prefab.GetComponent<KBatchedAnimController>().SetFGLayer(Grid.SceneLayer.BuildingFront);

			prefab.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();

			prefab.AddOrGet<Meltable>().selfHeatKW = 9.60f;

			return prefab;
		}

		private static GameObject CreatePrefab(string id, int width, int height, string anim, Tag[] rewardDrops = null, Vector3 rewardDropOffset = default, string initialAnim = "object")
		{
			var name = Strings.Get($"STRINGS.ENTITIES.GLACIERS.{id.ToUpperInvariant()}.NAME");
			var description = $"{Strings.Get($"STRINGS.ENTITIES.GLACIERS.{id.ToUpperInvariant()}.DESCRIPTION")}\n\n{STRINGS.ENTITIES.GLACIERS.GENERIC_THAW}";

			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				name,
				description,
				width * height * WEIGHT_PER_TILE,
				Assets.GetAnim(anim),
				"idle",
				Grid.SceneLayer.BuildingBack,
				width,
				height,
				TUNING.DECOR.BONUS.TIER2,
				element: SimHashes.Ice,
				additionalTags:
				[
					BTags.glacier
				],
				defaultTemperature: MiscUtil.CelsiusToKelvin(-30));

			var glacier = prefab.AddComponent<Glacier>();
			glacier.rewards = rewardDrops;
			glacier.offset = rewardDropOffset;
			glacier.initialAnim = initialAnim;

			prefab.GetComponent<KBatchedAnimController>().SetFGLayer(Grid.SceneLayer.BuildingFront);

			prefab.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();

			prefab.AddOrGet<Meltable>().selfHeatKW = 9.60f;

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
