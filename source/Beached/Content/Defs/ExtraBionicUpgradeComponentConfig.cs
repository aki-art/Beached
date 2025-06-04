using Beached.Content.ModDb;
using Database;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs
{
	public class ExtraBionicUpgradeComponentConfig
	{
		public const string PRECISION_ID = "Beached_Booster_Precision1";

		public static void AddPrefabs(BionicUpgradeComponentConfig original, List<GameObject> prefabs)
		{
			CreatePrecision1Booster(original, prefabs);
		}

		private static void CreatePrecision1Booster(BionicUpgradeComponentConfig original, List<GameObject> prefabs)
		{
			var modifiers = original.CreateBoosterModifiers(PRECISION_ID, new Dictionary<string, float>()
			{
				{ BAttributes.PRECISION_ID, 5f },
				{ Db.Get().Attributes.Digging.Id, 2f}
			});

			SkillPerk[] perks = [BSkillPerks.CanFindTreasures];

			var skilledWorker = new BionicUpgrade_SkilledWorker.Def(
				PRECISION_ID,
				PRECISION_ID,
				modifiers,
				perks,
				[BAccessories.ARCHEOLOGY_HAT]);

			var description = $"{skilledWorker.GetDescription()}\n\n{string.Format((string)global::STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, global::STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}";

			var item = BionicUpgradeComponentConfig.CreateNewUpgradeComponent(
				PRECISION_ID,
				stateMachine: smi => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skilledWorker),
				sm_description: description,
				dlcIDs: DlcManager.DLC3,
				animStateName: "basic_excavation_0",
				isCarePackage: true,
				skillPerks: perks);

			prefabs.Add(item);
		}
	}
}
