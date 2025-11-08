using FUtility;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;

public class LootTable(string id, ResourceSet parent) : Resource(id, parent)
{
}

public class LootTable<LootType>(string id, ResourceSet parent) : LootTable(id, parent)
{
	public List<WeightedOption> options = [];

	public LootTable<LootType> Add(WeightedOption option)
	{
		options.Add(option);
		return this;
	}

	public LootTable<LootType> Add(LootType item, float weight = 1f, Func<object, bool> condition = null)
	{
		return Add(new WeightedOption()
		{
			weight = weight,
			item = item,
			condition = condition
		});
	}

	public bool TryGetItem(out LootType item, object data = null, SeededRandom rng = null)
	{
		item = default;

		var option = options
			.Where(option => option.condition == null || option.condition.Invoke(data))
			.GetWeightedRandom(rng);

		if (option == null)
			return false;

		item = option.item;
		return true;
	}

	public class WeightedOption : IWeighted
	{
		public float weight { get; set; }
		public LootType item;
		public Func<object, bool> condition;
	}
}
