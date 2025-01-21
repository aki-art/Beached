using Beached.Content.Defs.Items;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	internal class DewPalmLeafConfig : IEntityConfig
	{
		public const string ID = "Beached_DewPalmLeaf";
		public const string BASE_TRAIT_ID = "Beached_DewPalmLeafOriginal";

		private static Dictionary<CellOffset, StandardCropPlant.AnimSet> anims;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Palm Leaf",
				"...",
				6f,
				Assets.GetAnim("beached_dewpalm_kanim"),
				"idle_empty",
				Grid.SceneLayer.BuildingFront,
				1,
				1,
				DECOR.NONE,
				additionalTags: [GameTags.HideFromSpawnTool, GameTags.PlantBranch]);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				CREATURES.TEMPERATURE.FREEZING_10,
				CREATURES.TEMPERATURE.FREEZING_9,
				CREATURES.TEMPERATURE.HOT_2,
				CREATURES.TEMPERATURE.HOT_3,
				null,
				true,
				0f,
				0.15f,
				PalmLeafConfig.ID,
				true,
				true,
				true,
				true,
				2400f,
				0f,
				2200f,
				BASE_TRAIT_ID,
				STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.NAME);

			prefab.AddOrGet<TreeBud>();
			prefab.AddOrGet<StandardCropPlant>();
			prefab.AddOrGet<BudUprootedMonitor>();

			AddLeaf("a", -1, +2);

			var def = prefab.AddOrGetDef<PlantBranch.Def>();
			def.animationSetupCallback += AdjustAnimation;

			return prefab;
		}

		private void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch)
		{
			var offset = Grid.GetOffset(Grid.PosToCell(trunk), Grid.PosToCell(branch));
			var crop = branch.GetComponent<StandardCropPlant>();
			var kbac = branch.GetComponent<KBatchedAnimController>();
			crop.anims = anims[offset];
			kbac.Offset = (-1 * offset).ToVector3();
			kbac.Play(crop.anims.grow, KAnim.PlayMode.Loop);
			crop.RefreshPositionPercent();
		}

		private void AddLeaf(string character, int x, int y)
		{
			anims ??= [];
			anims[new CellOffset(x, y)] = new StandardCropPlant.AnimSet()
			{
				grow = $"branch_{character}_grow",
				grow_pst = $"branch_{character}_grow_pst",
				idle_full = $"branch_{character}_idle_full",
				wilt_base = $"branch_{character}_wilt",
				harvest = $"branch_{character}_harvest"
			};
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
			inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
