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
                ConfigureSnapons(__result);

                __result.AddOrGet<Beached_MinionStorage>();
                __result.AddOrGet<Beached_LifeGoalTracker>();
                __result.AddOrGet<Beached_MinionEvents>();
            }

            private static void ConfigureSnapons(GameObject __result)
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

                AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.MAXIXE, "beached_maxixe_necklace_kanim");
                AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.ZIRCON, "beached_zircon_necklace_kanim");
                AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.ZEOLITE, "beached_zeolite_necklace_kanim");
                AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.HEMATITE, "beached_hematite_necklace_kanim");
                AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.PEARL, "beached_pearl_necklace_kanim");
                AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.STRANGE_MATTER, "beached_strange_matter_necklace_kanim");
            }

            private static void AddNecklaceSnapon(SnapOn snapOn, string snaponId, string anim)
            {
                snapOn.snapPoints.Add(new SnapOn.SnapPoint
                {
                    pointName = snaponId,
                    automatic = false,
                    context = "",
                    buildFile = Assets.GetAnim(anim),
                    overrideSymbol = "necklace"
                });
            }
        }
    }
}
