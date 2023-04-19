using Beached.Content.Defs;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
    public class GeyserGenericConfigPatch
    {
        [HarmonyPatch(typeof(GeyserGenericConfig), "GenerateConfigs")]
        public class GeyserGenericConfig_GenerateConfigs_Patch
        {
            public static void Postfix(List<GeyserGenericConfig.GeyserPrefabParams> __result)
            {
                GeyserConfigs.GenerateConfigs(__result);
            }
        }

        [HarmonyPatch(typeof(GeyserGenericConfig), "CreateGeyser")]
        public class GeyserGenericConfig_CreateGeyser_Patch
        {
            public static void Postfix(GameObject __result)
            {
                __result.AddComponent<Vista>();
            }
        }
    }
}
