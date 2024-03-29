﻿using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class CharacterContainerPatch
	{
		[HarmonyPatch(typeof(CharacterContainer), nameof(CharacterContainer.SetInfoText))]
		public class CharacterContainer_SetInfoText_Patch
		{
			public static void Postfix(CharacterContainer __instance)
			{
				var lifeGoal = __instance.stats.GetLifeGoalTrait();

				if (lifeGoal == null)
					return;

				var label = Util.KInstantiateUI<LocText>(__instance.expectationRight.gameObject, __instance.expectationRight.transform.parent.gameObject);
				label.gameObject.SetActive(true);
				label.text = string.Format(STRINGS.UI.CHARACTERCONTAINER_LIFEGOAL_TRAIT, lifeGoal.Name);

				var tooltip = lifeGoal.GetTooltip();
				var attributes = Db.Get().Attributes;

				foreach (var item in __instance.stats.GetLifeGoalAttributes())
				{
					var attribute = attributes.TryGet(item.Key);

					if (attribute == null)
					{
						Log.Warning($"CharacterContainer_SetInfoText_Patch: No attribute with ID {item.Key}");
						continue;
					}

					tooltip += "\n";
					tooltip += string.Format(
						global::STRINGS.UI.CHARACTERCONTAINER_SKILL_VALUE,
						GameUtil.AddPositiveSign(item.Value.ToString(), true),
						attribute.Name);
				}

				label.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
				__instance.expectationLabels.Add(label);
			}
		}
	}
}
