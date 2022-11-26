using Beached.Utils;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
    public class CharacterContainerPatch
    {
        [HarmonyPatch(typeof(CharacterContainer), "SetInfoText")]
        public class CharacterContainer_SetInfoText_Patch
        {
            public static void Postfix(MinionStartingStats ___stats, LocText ___expectationRight, List<LocText> ___expectationLabels)
            {
                var lifeGoal = ___stats.GetLifeGoalTrait();

                if (lifeGoal != null)
                {
                    var label = Util.KInstantiateUI<LocText>(___expectationRight.gameObject, ___expectationRight.transform.parent.gameObject);
                    label.gameObject.SetActive(true);
                    label.text = string.Format(STRINGS.UI.CHARACTERCONTAINER_LIFEGOAL_TRAIT, lifeGoal.Name);

                    var tooltip = lifeGoal.GetTooltip();

                    Log.Debug("tt");
                    Log.Debug("tooltip: " + ___stats.GetLifeGoalAttributes()?.Count);
                    foreach (var item in ___stats.GetLifeGoalAttributes())
                    {
                        Log.Debug("tooltip: " + item.Key);
                        var attribute = Db.Get().Attributes.Get(item.Key);
                        tooltip += "\n";
                        tooltip += string.Format(global::STRINGS.UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(item.Value.ToString(), true), attribute.Name);
                    }

                    label.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
                    ___expectationLabels.Add(label);
                }
            }
        }
    }
}
