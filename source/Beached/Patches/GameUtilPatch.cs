using Beached.Content;
using HarmonyLib;
using STRINGS;

namespace Beached.Patches
{
    public class GameUtilPatch
    {
        // Changes the breathability label for SaltyOxygen
        // usually i don't like to prefix skip, but it is very unlikely this would conflict with some other mod
        [HarmonyPatch(typeof(GameUtil), "GetBreathableString")]
        public class GameUtil_GetBreathableString_Patch
        {
            public static bool Prefix(Element element, float Mass, ref string __result)
            {
                if (element.id == Elements.saltyOxygen)
                {
                    var color = ModAssets.Colors.positiveColorHex;
                    LocString legend;

                    switch (Mass)
                    {
                        case float n when n >= SimDebugView.optimallyBreathable:
                            legend = UI.OVERLAYS.OXYGEN.LEGEND1;
                            break;
                        case float n when n >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f:
                            legend = UI.OVERLAYS.OXYGEN.LEGEND2;
                            break;
                        case float n when n >= SimDebugView.minimumBreathable:
                            color = ModAssets.Colors.warningColorHex;
                            legend = UI.OVERLAYS.OXYGEN.LEGEND3;
                            break;
                        default:
                            color = ModAssets.Colors.negativeColorHex;
                            legend = UI.OVERLAYS.OXYGEN.LEGEND4;
                            break;
                    }

                    __result = string.Format(ELEMENTS.BREATHABLEDESC, color, legend);

                    return false;
                }

                return true;
            }
        }
    }
}
