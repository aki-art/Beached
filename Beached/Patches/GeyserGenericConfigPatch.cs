using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;

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
    }
}
