using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
    internal class BuildingTemplatesPatch
    {
        //[HarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
        public class BuildingTemplates_CreateBuildingDef_Patch
        {
            public static void Postfix(ref GameObject __result)
            {
                if (__result.TryGetComponent(out Door _))
                {
                    Lubricatable.ConfigurePrefab(__result, 5f, 5f / 12f);
                }
            }
        }
    }
}
