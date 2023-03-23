using Beached.Content.UI;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
    public class SimpleInfoScreenPatch
    {
        [HarmonyPatch(typeof(SimpleInfoScreen), "OnSelectTarget")]
        public class SimpleInfoScreen_OnSelectTarget_Patch
        {
            private static void Prefix(ref SimpleInfoScreen __instance, GameObject target)
            {
                if (target.TryGetComponent(out CreatureBrain _))
                {
                    var critterTraits = __instance.gameObject.AddOrGet<Beached_CritterTraitsScreen>();
                    critterTraits.Initialize(__instance);
                    critterTraits.SetTarget(target);

                    critterTraits.gameObject.SetActive(true);

                    return;
                }
                else if(__instance.TryGetComponent(out Beached_CritterTraitsScreen screen))
                {
                    screen.Hide();
                }
            }
        }
    }
}
