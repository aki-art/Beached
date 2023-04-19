using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beached.Patches
{
    internal class CometPatch
    {

        [HarmonyPatch(typeof(Comet), "Sim33ms")]
        public class Comet_Sim33ms_Patch
        {
            public static void Postfix(Comet __instance)
            {
                if (__instance.hasExploded)
                {
                    return;
                }

                var myWorldId = __instance.GetMyWorldId();
                
                if (!BeachedGrid.forceFieldLevelPerWorld.TryGetValue(myWorldId, out var forceFieldY) ||
                    forceFieldY == BeachedGrid.INVALID_FORCEFIELD_OFFSET)
                {
                    return;
                }

                Grid.PosToXY(__instance.transform.position, out _, out var y);

                if (y <= forceFieldY && __instance.TryGetComponent(out PrimaryElement primaryElement))
                {
                    var cell = Grid.PosToCell(__instance);
                    var previousCell = Grid.PosToCell(__instance.previousPosition);
                    __instance.Explode(__instance.transform.GetPosition(), cell, previousCell, primaryElement.Element);
                    __instance.hasExploded = true;

                    if (__instance.destroyOnExplode)
                    {
                        Util.KDestroyGameObject(__instance.gameObject);
                    }
                }
            }
        }
    }
}
