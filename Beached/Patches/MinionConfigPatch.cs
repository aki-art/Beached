using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
    public class MinionConfigPatch
    {
        [HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        public class MinionConfig_CreatePrefab_Patch
        {
            public static void Postfix(GameObject __result)
            {
                var snapOn = __result.GetComponent<SnapOn>();
                snapOn.snapPoints.Add(new SnapOn.SnapPoint
                {
                    pointName = CONSTS.SNAPONS.CAP,
                    automatic = false,
                    context = "",
                    buildFile = Assets.GetAnim("beached_head_shroom_kanim"),
                    overrideSymbol = "snapTo_hat"
                });

                snapOn.snapPoints.Add(new SnapOn.SnapPoint
                {
                    pointName = CONSTS.SNAPONS.RUBBER_BOOTS,
                    automatic = false,
                    context = "",
                    buildFile = Assets.GetAnim("beached_rubberboots_kanim"),
                    overrideSymbol = "foot"
                });

                __result.AddOrGet<BeachedMinionStorage>();
                __result.AddOrGet<LifeGoalTracker>();
            }
        }
    }
}
