using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
    public class OilFloaterDecorConfigPatch
    {

        [HarmonyPatch(typeof(OilFloaterDecorConfig), "CreateOilFloater")]
        public class OilFloaterDecorConfig_CreateOilFloater_Patch
        {
            public static void Postfix(GameObject __result)
            {
                // TODO: should it live in salty oxygen?
            }
        }
    }
}
