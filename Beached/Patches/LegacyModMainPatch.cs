using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
    public class LegacyModMainPatch
    {
        [HarmonyPatch(typeof(LegacyModMain), "ConfigElements")]
        public class LegacyModMain_ConfigElements_Patch
        {
            public static void Postfix()
            {
                Elements.AddAttributeModifiers();
            }
        }
    }
}
