using Beached.Integration;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class LootTables : ResourceSet<LootTable>
	{
		public const string ID = "Beached_LootTables";

		public LootTable<MaterialReward> slagmiteShellDrops;
		public LootTable<MaterialReward> gleamiteShellDrops;

		public LootTables(ResourceSet parent) : base(ID, parent)
		{
			ConfigureSlagmiteShell();
			ConfigureGleamiteShell();
		}

		private void ConfigureSlagmiteShell()
		{
			var defaultMass = 50f;

			slagmiteShellDrops = new LootTable<MaterialReward>("Beached_SlagmiteShellDrops", this)
				.Add(new MaterialReward(SimHashes.Cuprite, defaultMass))
				.Add(new MaterialReward(SimHashes.IronOre, defaultMass))
				.Add(new MaterialReward(SimHashes.Wolframite, defaultMass))
				.Add(new MaterialReward(SimHashes.Cobaltite, defaultMass))
				.Add(new MaterialReward(SimHashes.AluminumOre, defaultMass))
				.Add(new MaterialReward(Elements.bismuthOre, defaultMass))
				.Add(new MaterialReward(Elements.galena, defaultMass))
				.Add(new MaterialReward(Elements.zincOre, defaultMass))
				.Add(new MaterialReward(Elements.zirconiumOre, defaultMass), 0.25f);

			if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
				slagmiteShellDrops
					.Add(new MaterialReward(SimHashes.Cinnabar, defaultMass));

			if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID))
				slagmiteShellDrops
					.Add(new MaterialReward(SimHashes.NickelOre, defaultMass));

			if (DlcManager.IsContentSubscribed(DlcManager.EXPANSION1_ID))
				slagmiteShellDrops
					.Add(new MaterialReward(SimHashes.UraniumOre, defaultMass));

			if (Mod.integrations.IsModPresent(Integrations.CHEMICAL_PROCESSING))
			{
				slagmiteShellDrops
					.Add(new MaterialReward(Elements.ChemicalProcessing.argentiteOre, defaultMass))
					.Add(new MaterialReward(Elements.ChemicalProcessing.galena, defaultMass))
					.Add(new MaterialReward(Elements.ChemicalProcessing.aurichalciteOre, defaultMass));
			}
		}

		private void ConfigureGleamiteShell()
		{
			var defaultMass = 25f;
			var rare = 0.25f;
			var veryRare = 0.1f;

			gleamiteShellDrops = new LootTable<MaterialReward>("Beached_GleamiteShellDrops", this)
				.Add(new MaterialReward(SimHashes.Copper, defaultMass))
				.Add(new MaterialReward(SimHashes.Iron, defaultMass))
				.Add(new MaterialReward(SimHashes.Tungsten, defaultMass))
				.Add(new MaterialReward(SimHashes.Aluminum, defaultMass))
				.Add(new MaterialReward(Elements.bismuth, defaultMass))
				.Add(new MaterialReward(SimHashes.Lead, defaultMass))
				.Add(new MaterialReward(Elements.zinc, defaultMass))
				.Add(new MaterialReward(SimHashes.Cobalt, defaultMass))
				.Add(new MaterialReward(Elements.zirconium, defaultMass), rare)
				.Add(new MaterialReward(Elements.iridium, defaultMass), rare, _ => HasDiscovered(Elements.iridium.CreateTag()))
				.Add(new MaterialReward(SimHashes.Niobium, defaultMass), rare, critter => HighTierMetalCondition(critter, SimHashes.Niobium))
				.Add(new MaterialReward(SimHashes.TempConductorSolid, defaultMass), veryRare, critter => HighTierMetalCondition(critter, SimHashes.TempConductorSolid))
				.Add(new MaterialReward(Elements.calcium, defaultMass));

			if (Mod.integrations.IsModPresent(Integrations.ROCKETRY_EXPANDED))
			{
				gleamiteShellDrops.Add(
					new MaterialReward(Elements.RocketryExpanded.unobtaniumAlloy, defaultMass),
					rare,
					critter => HighTierMetalCondition(critter, Elements.RocketryExpanded.unobtaniumAlloy));
			}

			if (DlcManager.IsContentSubscribed(DlcManager.EXPANSION1_ID))
				gleamiteShellDrops
				.Add(new MaterialReward(SimHashes.EnrichedUranium, defaultMass));
			/*
						if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
							gleamiteShellDrops
								.Add(new MaterialReward(SimHashes.Mercury, defaultMass));*/


			if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID))
				gleamiteShellDrops
					.Add(new MaterialReward(SimHashes.Nickel, defaultMass))
					.Add(
					new MaterialReward(SimHashes.Iridium, defaultMass),
					rare,
					critter => HighTierMetalCondition(critter, SimHashes.Iridium));

			if (Mod.integrations.IsModPresent(Integrations.CHEMICAL_PROCESSING))
			{
				gleamiteShellDrops
					.Add(new MaterialReward(Elements.ChemicalProcessing.solidSilver, defaultMass))
					.Add(new MaterialReward(Elements.ChemicalProcessing.solidBrass, defaultMass))
					.Add(new MaterialReward(Elements.ChemicalProcessing.solidZinc, defaultMass));
			}
		}

		private bool HighTierMetalCondition(object critter, SimHashes element, float happinessTreshold = 0.8f)
		{
			return IsHappyEnough(critter, happinessTreshold) && HasDiscovered(element.CreateTag());
		}

		private bool IsHappyEnough(object data, float happinessTreshold = 0.8f)
		{
			if (data is GameObject gameObject)
			{
				var happiness = gameObject.GetAttributes().Get(Db.Get().CritterAttributes.Happiness.Id);
				if (happiness == null)
					return false;

				return (happiness.GetTotalValue() / happiness.GetBaseValue()) > happinessTreshold;
			}

			return false;
		}

		private bool HasDiscovered(Tag tag) => DiscoveredResources.Instance.IsDiscovered(tag);

		public bool TryGetLoot<LootType>(out LootType loot, HashedString tableId, object data = null, SeededRandom rng = null)
		{
			loot = default;

			var table = TryGet(tableId);

			if (table == null)
				return false;

			if (table is not LootTable<LootType> typedTable)
			{
				Log.Warning($"Incorrect type provided for LootTables.GetLoot. {typeof(LootType)} is not compatible with {table.GetType().Name}");
				return false;
			}

			return typedTable.TryGetItem(out loot, data, rng);
		}

		public struct MaterialReward(Tag tag, float amount)
		{
			public Tag tag = tag;
			public float mass = amount;

			public MaterialReward(SimHashes simHash, float amount) : this(simHash.CreateTag(), amount) { }
		}
	}
}
