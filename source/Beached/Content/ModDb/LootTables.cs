using Beached.Integration;

namespace Beached.Content.ModDb
{
	public class LootTables : ResourceSet<LootTable>
	{
		public const string ID = "Beached_LootTables";

		public LootTable<MaterialReward> slagmiteShellDrops;

		public LootTables(ResourceSet parent) : base(ID, parent)
		{
			ConfigureSlagmiteShell();
			// TODO: Moonlet added stuff
		}

		private void ConfigureSlagmiteShell()
		{
			var defaultMass = 50f;

			slagmiteShellDrops = new LootTable<MaterialReward>("Beached_SlagmiteShellDrops", this)
				.Add(new MaterialReward(SimHashes.Cuprite, defaultMass))
				.Add(new MaterialReward(SimHashes.IronOre, defaultMass))
				.Add(new MaterialReward(SimHashes.Wolframite, defaultMass))
				.Add(new MaterialReward(SimHashes.AluminumOre, defaultMass))
				.Add(new MaterialReward(Elements.bismuthOre, defaultMass))
				.Add(new MaterialReward(Elements.galena, defaultMass))
				.Add(new MaterialReward(Elements.zincOre, defaultMass))
				.Add(new MaterialReward(Elements.zirconiumOre, defaultMass));

			if (DlcManager.IsContentSubscribed(DlcManager.EXPANSION1_ID))
				slagmiteShellDrops
					.Add(new MaterialReward(SimHashes.Cobaltite, defaultMass));

			if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
				slagmiteShellDrops
					.Add(new MaterialReward(SimHashes.Cinnabar, defaultMass));

			if (Mod.integrations.IsModPresent(Integrations.CHEMICAL_PROCESSING))
			{
				slagmiteShellDrops
					.Add(new MaterialReward(Elements.ChemicalProcessing.argentiteOre, defaultMass))
					.Add(new MaterialReward(Elements.ChemicalProcessing.galena, defaultMass))
					.Add(new MaterialReward(Elements.ChemicalProcessing.aurichalciteOre, defaultMass));
			}
		}

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
