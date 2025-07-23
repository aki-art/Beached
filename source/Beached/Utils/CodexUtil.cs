using UnityEngine;
using static CodexEntryGenerator_Elements;

namespace Beached.Utils
{
	public static class CodexUtil
	{
		public static ConversionEntry SimpleConversionBase(ElementEntryContext context, GameObject prefab, string title = null)
		{
			var conversionEntry = new ConversionEntry()
			{
				title = title ?? prefab.GetProperName(),
				prefab = prefab,
				inSet = []
			};

			context.usedMap.Add(prefab.PrefabID(), conversionEntry);

			return conversionEntry;
		}

		public static ElementUsage UsageMassPerCycle(Tag tag, float amount)
		{
			return new ElementUsage(tag, amount, true)
			{
				customFormating = (tag, amount, continous) => GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerCycle)
			};
		}

		public static ElementUsage UsageMassOnce(Tag tag, float amount)
		{
			return new ElementUsage(tag, amount, false)
			{
				customFormating = (tag, amount, continous) => GameUtil.GetFormattedMass(amount)
			};
		}
	}
}
