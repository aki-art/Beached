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

			chances.CreateTreasureEntry(SimHashes.Diamond, 0.1f, precious: true)
				.Add(RareGemsConfig.FLAWLESS_DIAMOND, 1, 1f, true);

			chances.CreateTreasureEntry(Elements.aquamarine, 0.1f, precious: true)
				.Add(RareGemsConfig.MAXIXE, 1, 1f, true);

			chances.CreateTreasureEntry(Elements.zirconiumOre, 0.1f, precious: true)
				.Add(RareGemsConfig.HADEAN_ZIRCON, 1, 1f, true);

			chances.CreateTreasureEntry(Elements.crackedNeutronium, 0.1f, precious: true)
				.Add(RareGemsConfig.STRANGE_MATTER, 1, 1, true);

			chances.CreateTreasureEntry(Elements.pearl, 0.1f, precious: true)
				.Add(RareGemsConfig.MOTHER_PEARL, 1, 1, true);

			chances.CreateTreasureEntry(SimHashes.SandStone, 0.1f)
				.Add(HatchConfig.EGG_ID)
				.Add(PrickleFlowerConfig.SEED_ID);

			chances.CreateTreasureEntry(Elements.siltStone, 0.1f)
				.Add(SlickShellConfig.EGG_ID);

			chances.CreateTreasureEntry(Elements.amber, 0.1f, precious: true)
				.Add(AmberInclusionsConfig.STRANGE_HATCH, 1, 1, notifyPlayer: true)
				.Add(AmberInclusionsConfig.FLYING_CENTIPEDE, 1, 1, notifyPlayer: true)
				.Add(AmberInclusionsConfig.FEATHER, 1, 1, notifyPlayer: true);

			// Prehistoric Pack Amber
			chances.CreateTreasureEntry(SimHashes.Amber, 0.02f, precious: true)
				.Add(AmberInclusionsConfig.STRANGE_HATCH, 1, 1, notifyPlayer: true)
				.Add(AmberInclusionsConfig.FLYING_CENTIPEDE, 1, 1, notifyPlayer: true)
				.Add(AmberInclusionsConfig.FEATHER, 1, 1, notifyPlayer: true);

			foreach (var chance in chances)
			{
				var element = ElementLoader.FindElementByHash(chance.Key);
				if (element == null)
					continue;

				element.AddTag(BTags.inclusionContaining);

				if (chance.Value.precious)
					element.AddTag(BTags.preciousContaining);
			}
		}

		public void TrySpawnTreasure(Diggable diggable, Element element, WorkerBase worker)
		{
			if (!CanWorkerFindTreasure(worker))
			{
				return;
			}

			var mass = Grid.Mass[diggable.cached_cell];
			Log.Debug($"mass: {mass}");

			//var multiplier = Mathf.Ceil(mass / 1000.0f);
			var multiplier = 1.0f;

			if (worker.GetComponent<MinionResume>().HasPerk(BSkillPerks.CanFindMoreTreasures))
				multiplier += 1.0f;

			if (chances.TryGetValue(element.id, out var treasureSource))
			{
				if (treasureSource.Roll(multiplier))
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
