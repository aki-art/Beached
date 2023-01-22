using Beached.Utils;
using HarmonyLib;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Beached.Content.BWorldGen
{
    public class BCluster : Cluster
    {
        public BCluster(string clusterId, int seed, List<string> chosenStoryTraitIds)
        {
            Log.Debug($"creating new Beached type Cluster {clusterId}");
            WorldGen.LoadSettings(false);

            Id = clusterId;
            this.seed = seed;
            this.chosenStoryTraitIds = chosenStoryTraitIds;
            unplacedStoryTraits = new List<WorldTrait>();
            clusterLayout = SettingsCache.clusterLayouts.clusterCache[clusterId];

            SetWorldSizes();

            clusterLayout.worldPlacements[clusterLayout.startWorldIndex].startWorld = true;

            size = BestFit.BestFitWorlds(clusterLayout.worldPlacements, false);
        }

        /// <see cref="Patches.ClusterPatch.Cluster_Generate_Patch"/>
        public void Generate(WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> errorCb, int seed = -1, bool debug = false)
        {
            Log.Debug("Generating Beached Cluster");
            doSimSettle = false;

            for (int i = 0; i != worlds.Count; i++)
            {
                var offsetSeed = seed + i;
                worlds[i].Initialise(
                    callbackFn,
                    errorCb,
                    offsetSeed,
                    offsetSeed,
                    offsetSeed,
                    offsetSeed,
                    debug);
            }

            IsGenerationComplete = false;
            thread = new Thread(new ThreadStart(ThreadMain));
            Util.ApplyInvariantCultureToThread(thread);
            thread.Start();
        }

        private void SetWorldSizes()
        {
            foreach (var worldPlacement in clusterLayout.worldPlacements)
            {
                var world = SettingsCache.worlds.GetWorldData(worldPlacement.world);
                if (world != null)
                {
                    worldPlacement.SetSize(world.worldsize);
                }
            }
        }
    }
}
