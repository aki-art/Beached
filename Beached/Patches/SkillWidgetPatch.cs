using HarmonyLib;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Beached.Patches
{
    public class SkillWidgetPatch
    {
        [HarmonyPatch(typeof(SkillWidget), "RefreshLines")]
        public class SkillWidget_RefreshLines_Patch
        {
            public static void Postfix(UILineRenderer[] ___lines)
            {
                foreach(var line in ___lines)
                {
                    line.BezierMode = UILineRenderer.BezierType.Basic;
                    line.BezierSegmentsPerCurve = 16;
                    line.LineJoins = UILineRenderer.JoinType.Bevel;
                    line.color = Color.red;
                    line.Rebuild(UnityEngine.UI.CanvasUpdate.Layout);
                    line.SetAllDirty();
                }
            }
        }
    }
}
