﻿using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
    internal class ClusterMapMeteorShowerConfigPatch
    {

        [HarmonyPatch(typeof(ClusterMapMeteorShowerConfig), "CreatePrefabs")]
        public class ClusterMapMeteorShowerConfig_CreatePrefabs_Patch
        {
            public static void Postfix(List<GameObject> __result)
            {
                __result.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor(
                    BGameplayEvents.DIAMOND_SHOWER,
                    STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BEACHED_DIAMOND.NAME,
                    STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BEACHED_DIAMOND.DESCRIPTION,
                    "shower_cluster_biological_kanim",
                    "idle_loop",
                    "ui",
                    DlcManager.AVAILABLE_EXPANSION1_ONLY,
                    SimHashes.Unobtanium));
            }
        }
    }
}