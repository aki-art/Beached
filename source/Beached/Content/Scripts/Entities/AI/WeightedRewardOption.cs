using ProcGen;
using System;

namespace Beached.Content.Scripts.Entities.AI
{
	[Serializable]
	public struct WeightedRewardOption(string itemTag, float totalMass, float weight = 1f) : IWeighted
	{
		public float weight { get; set; } = weight;
		public string id = itemTag;
		public float totalMass = totalMass;

		public WeightedRewardOption(SimHashes item, float totalMass, float weight = 1f) : this(item.ToString(), totalMass, weight)
		{

		}
	}
}
