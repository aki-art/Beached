using Beached.Content.Defs;
using HarmonyLib;
using TemplateClasses;

namespace Beached.Patches
{
    public class DebugBaseTemplateButtonPatch
    {
        [HarmonyPatch(typeof(DebugBaseTemplateButton), "GetSelectionAsAsset")]
        public class DebugBaseTemplateButton_GetSelectionAsAsset_Patch
        {
            public static void Postfix(TemplateContainer __result)
            {
                if (__result.otherEntities == null)
                {
                    return;
                }

                foreach(var entity in __result.otherEntities)
                {
                    if(entity.id == TemplateProcessorConfig.ID)
                    {
                        var value = new Prefab.template_amount_value(CONSTS.TEMPLATE_VALUES.BIOME_OVERRIDE, 1);
                        entity.other_values = entity.other_values.AddToArray(value);
                    }
                }
            }
        }
    }
}
