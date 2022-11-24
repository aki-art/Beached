using Beached.Content.ModDb.Germs;
using Database;
using HarmonyLib;
using System;

namespace Beached.Patches.DatabasePatches
{
    public class DiseasesPatch
    {
        [HarmonyPatch(typeof(Diseases), MethodType.Constructor, new Type[] { typeof(ResourceSet), typeof(bool) })]
        public class Diseases_Constructor_Patch
        {
            public static void Postfix(Diseases __instance, bool statsOnly)
            {
                BDiseases.Register(__instance, statsOnly);
            }
        }
    }
}
