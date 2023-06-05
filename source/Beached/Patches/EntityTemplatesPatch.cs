using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Patches
{
	public class EntityTemplatesPatch
	{
		[HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToBasicCreature))]
		public class EntityTemplates_ExtendEntityToBasicCreature_Patch
		{
			public static void Postfix(GameObject template)
			{
				if (FurSource.furries.Contains(template.PrefabID().ToString()))
					template.AddOrGet<FurSource>();
			}
		}

		// if a plant can survive in oxygen, allow it to live in salty oxygen as well
		[HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToBasicPlant))]
		public class EntityTemplates_ExtendEntityToBasicPlant_Patch
		{
			public static void Prefix(ref SimHashes[] safe_elements)
			{
				if (safe_elements != null)
				{
					var elements = safe_elements.ToHashSet();

					if (elements.Contains(SimHashes.Oxygen))
						elements.Add(Elements.saltyOxygen);

					safe_elements = elements.ToArray();
				}
			}
		}

		[HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToFood))]
		public class EntityTemplates_ExtendEntityToFood_Patch
		{
			public static void Postfix(GameObject template)
			{
				if (SmokeCookable.smokables.TryGetValue(template.PrefabID(), out var config))
				{
					var smokable = template.AddOrGet<SmokeCookable>();
					smokable.cyclesToSmoke = config.timeToSmokeInCycles;
					smokable.smokedItemTag = config.smokedItem;
					smokable.requiredTemperature = config.requiredTemperature;
				}

				var additionalMeats = new HashSet<Tag>()
				{
					MeatConfig.ID,
					FishMeatConfig.ID,
					ShellfishMeatConfig.ID
				};

				if (additionalMeats.Contains(template.PrefabID()))
					template.AddTag(BTags.meat);
			}
		}
	}
}
