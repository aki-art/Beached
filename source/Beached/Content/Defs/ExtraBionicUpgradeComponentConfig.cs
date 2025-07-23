using Beached.Content.ModDb;
using Database;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs
{
	public class ExtraBionicUpgradeComponentConfig
	{
		public const string PRECISION1_ID = "Beached_Booster_Precision1";
		public const string PRECISION2_ID = "Beached_Booster_Precision2";

		public static void AddPrefabs(BionicUpgradeComponentConfig original, List<GameObject> prefabs)
		{
			if (!DlcManager.IsContentSubscribed(DlcManager.DLC3_ID))
				return;

			CreatePrecision1Booster(original, prefabs);
			CreatePrecision2Booster(original, prefabs);
		}

		private static void CreatePrecision1Booster(BionicUpgradeComponentConfig original, List<GameObject> prefabs)
		{
			var modifiers = original.CreateBoosterModifiers(PRECISION1_ID, new Dictionary<string, float>()
			{
				{ BAttributes.PRECISION_ID, 5f },
				{ Db.Get().Attributes.Digging.Id, 2f}
			});

			SkillPerk[] perks = [BSkillPerks.CanFindTreasures, BSkillPerks.CanFindMoreTreasures];

			var skilledWorker = new BionicUpgrade_SkilledWorker.Def(
				PRECISION1_ID,
				PRECISION1_ID,
				modifiers,
				perks,
				[BAccessories.ARCHEOLOGY_HAT]);

			var description = $"{skilledWorker.GetDescription()}\n\n{string.Format((string)global::STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, global::STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}";

			var item = BionicUpgradeComponentConfig.CreateNewUpgradeComponent(
				PRECISION1_ID,
				stateMachine: smi => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skilledWorker),
				sm_description: description,
				dlcIDs: DlcManager.DLC3,
				animStateName: "basic_excavation_0",
				isCarePackage: true,
				skillPerks: perks,
				isStartingBooster: true);

			prefabs.Add(item);
		}

		private static void CreatePrecision2Booster(BionicUpgradeComponentConfig original, List<GameObject> prefabs)
		{
			var modifiers = original.CreateBoosterModifiers(PRECISION2_ID, new Dictionary<string, float>()
			{
				{ BAttributes.PRECISION_ID, 5f },
				{ Db.Get().Attributes.Digging.Id, 2f}
			});

			SkillPerk[] perks = [BSkillPerks.CanFindMoreTreasures, BSkillPerks.CanCutGems, BSkillPerks.CanAnalyzeClusters];

			var skilledWorker = new BionicUpgrade_SkilledWorker.Def(
				PRECISION2_ID,
				PRECISION2_ID,
				modifiers,
				perks,
				[BAccessories.ARCHEOLOGY_HAT]); // TODO update with hat

			var description = $"{skilledWorker.GetDescription()}\n\n{string.Format((string)global::STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, global::STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}";

			var item = BionicUpgradeComponentConfig.CreateNewUpgradeComponent(
				PRECISION2_ID,
				stateMachine: smi => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skilledWorker),
				sm_description: description,
				dlcIDs: DlcManager.DLC3,
				animStateName: "basic_excavation_0",
				isCarePackage: true,
				skillPerks: perks,
				isStartingBooster: true);

			prefabs.Add(item);
		}
	}
}
