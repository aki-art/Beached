using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SkipSaveFileSerialization]
	public class Treasury : KMonoBehaviour
	{
		public TreasureChances chances;

		// used to keep track for the WorldDamager, so the dug mass can be adjusted
		public static Dictionary<int, WorkerBase> diggers;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			diggers = [];
		}

		public void Configure()
		{
			chances = [];

			chances.CreateTreasureEntry(SimHashes.Diamond, 0.1f)
				.Add(RareGemsConfig.FLAWLESS_DIAMOND, 1, 1f, true);

			chances.CreateTreasureEntry(Elements.aquamarine, 0.1f)
				.Add(RareGemsConfig.MAXIXE, 1, 1f, true);

			chances.CreateTreasureEntry(Elements.zirconiumOre, 0.1f)
				.Add(RareGemsConfig.HADEAN_ZIRCON, 1, 1f, true);

			chances.CreateTreasureEntry(Elements.crackedNeutronium, 0.1f)
				.Add(RareGemsConfig.STRANGE_MATTER, 1, 1, true);

			chances.CreateTreasureEntry(SimHashes.SandStone, 0.1f)
				.Add(HatchConfig.EGG_ID)
				.Add(PrickleFlowerConfig.SEED_ID);

			chances.CreateTreasureEntry(Elements.siltStone, 0.1f)
				.Add(SlickShellConfig.EGG_ID);

			chances.CreateTreasureEntry(Elements.amber, 0.1f)
				.Add(AmberInclusionsConfig.STRANGE_HATCH, 1, 1, notifyPlayer: true)
				.Add(AmberInclusionsConfig.FLYING_CENTIPEDE, 1, 1, notifyPlayer: true)
				.Add(AmberInclusionsConfig.FEATHER, 1, 1, notifyPlayer: true);
		}

		public void TrySpawnTreasure(Diggable diggable, Element element, WorkerBase worker)
		{
			if (!CanWorkerFindTreasure(worker))
			{
				return;
			}

			if (chances.TryGetValue(element.id, out var treasureSource))
			{
				if (treasureSource.Roll())
				{
					var item = treasureSource.GetRandomTreasure();
					if (item == null) return;
					var cell = Grid.PosToCell(diggable);
					var loot = MiscUtil.Spawn(item.tag, diggable.gameObject);

					var primaryElement = loot.GetComponent<PrimaryElement>();
					primaryElement.Mass = item.amount;
					primaryElement.Temperature = Grid.Temperature[cell];

					if (item.notifyPlayer)
					{
						//notifier.AutoClickFocus = true;
						//notifier.Add(notification, "");
					}

					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, loot.GetProperName(), loot.transform);
				}
			}
		}

		private static bool CanWorkerFindTreasure(Component worker)
		{
			return worker != null &&
					worker.TryGetComponent(out MinionResume resume) &&
					resume.HasPerk(BSkillPerks.CanFindTreasures);
		}
	}
}
