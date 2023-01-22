using Beached.Content.BWorldGen;
using HarmonyLib;
using ProcGenGame;
using System;

namespace Beached.Patches
{
    public class ClusterPatch
    {
        [HarmonyPatch(typeof(Cluster), "Generate")]
        public class Cluster_Generate_Patch
        {
            public static void Prefix(Cluster __instance, WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> error_cb, int worldSeed, int layoutSeed, int terrainSeed, int noiseSeed, bool doSimSettle, bool debug)
            {
                if (__instance is BCluster bCluster)
                {
                    bCluster.Generate(callbackFn, error_cb, worldSeed, debug);
                }
            }
        }

    }
}
