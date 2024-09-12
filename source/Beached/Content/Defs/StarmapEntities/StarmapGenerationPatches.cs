using HarmonyLib;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beached.Content.Defs.StarmapEntities
{
    /// <summary>
    /// TODO: move to "Patches" namespace
    /// </summary>
    internal class StarmapGenerationPatches
    {
        /// <summary>
        /// position relative from swarm orbit origin
        /// </summary>
        enum SpecialMeteorSwarmRelation
        {
            none = 0,
            beforeSwarmOrbit = 1,
            inSwarmOrbit = 2,
            afterSwarmOrbit = 3,
        }

        [HarmonyPatch(typeof(Cluster), nameof(Cluster.AssignClusterLocations))]
        public class Cluster_AssignClusterLocations_Patch
        {
            [HarmonyPrepare]
            public static bool Prepare() => true;

            static SpecialMeteorSwarmRelation SpecialWorldPosition = SpecialMeteorSwarmRelation.none;

            /// <summary>
            /// Hexes that are considered "inside the swarm"
            /// </summary>
            static HashSet<AxialI> SwarmLocations;
            /// <summary>
            /// Hexes that are considered "inside or directly adjaced to swarm"
            /// </summary>
            static HashSet<AxialI> SwarmLocationsWithBuffer;

            /// <summary>
            /// Hexes that are inside of the swarm orbit
            /// </summary>
            static HashSet<AxialI> InnerLocations;

            /// <summary>
            /// Hexes that are outside of the swarm orbit
            /// </summary>
            static HashSet<AxialI> OuterLocations;
            static AxialI SwarmOriginLocation;

            static void Prefix(Cluster __instance)
            {
                PopulateSwarmLocationData(__instance);
            }
            static void Postfix(Cluster __instance)
            {
                __instance.poiPlacements.Add(SwarmOriginLocation, MeteorSwarmVisualPOIConfig.ID);
                HashSet<AxialI> worldLocations = new();
                foreach (var world in __instance.worlds)
                {
                    worldLocations.Add(world.data.clusterLocation);
                }

                foreach (var swarmLocation in SwarmLocations)
                {
                    if (!worldLocations.Contains(swarmLocation))
                        __instance.poiPlacements.Add(swarmLocation, MeteorSwarmDamagePOIConfig.ID);
                }
            }
            public static AxialI GetAsteroidSwarmOrigin(int seed, int rings)
            {
                //the 6 corner locations of the cluster
                AxialI[] BeachedBeltSources = [
                    new (0,rings),
                    new (0,-rings),
                    new (-rings,0),
                    new (rings,0),
                    new (rings,-rings),
                    new (-rings,rings),
                ];

                var myRandom = new SeededRandom(seed);
                return BeachedBeltSources[myRandom.RandomRange(0, BeachedBeltSources.Length)];
            }
            private static void PopulateSwarmLocationData(Cluster __instance)
            {
                SwarmLocations = new();
                InnerLocations = new();
                OuterLocations = new();

                int rings = __instance.numRings - 1;
                SwarmOriginLocation = GetAsteroidSwarmOrigin(__instance.seed, rings);
                AxialI center = new(0, 0);

                var swarmOriginPos = AxialUtil.AxialToWorld(SwarmOriginLocation.R, SwarmOriginLocation.Q);
                var centerPos = AxialUtil.AxialToWorld(0, 0);

                float worldRadius = Vector3.Distance(centerPos, swarmOriginPos); //grid radius in world coordinates


                //iterate all hexes of the cluster and assign them to one of the 3 belt location types
                foreach (AxialI location in AxialUtil.GetAllPointsWithinRadius(center, rings))
                {
                    var locationPoint = AxialUtil.AxialToWorld(location.R, location.Q);
                    var distanceF = Vector3.Distance(swarmOriginPos, locationPoint);

                    var distance = Mathf.RoundToInt(distanceF);
                    if (distance > Mathf.RoundToInt(worldRadius * 1.5f) + 1)
                    {
                        OuterLocations.Add(location);
                        continue;
                    }
                    if (distance < Mathf.RoundToInt(worldRadius * 1.5f) - 1)
                    {
                        InnerLocations.Add(location);
                        continue;
                    }
                    SwarmLocations.Add(location);
                }
                SwarmLocationsWithBuffer = new();
                foreach (AxialI location in SwarmLocations)
                    SwarmLocationsWithBuffer.UnionWith(AxialUtil.GetAllPointsWithinRadius(location, 1));

                SwarmLocationsWithBuffer = new(SwarmLocationsWithBuffer.Distinct());
            }
            private static int GetWorldByIndex(int index, Cluster instance)
            {
                Debug.Log("world index: " + index);
                SpecialWorldPosition = SpecialMeteorSwarmRelation.none;

                var worldPlacement = SettingsCache.clusterLayouts.clusterCache[instance.Id].worldPlacements[index];
                var world = instance.worlds[index].Settings.world;
                var worldCategoryInt = (int)worldPlacement.locationType;
                Debug.Log("World locationType to int: " + worldCategoryInt);
                if (worldCategoryInt == Hash.SDBMLower("Beached_BeforeMeteorSwarm"))
                {
                    Log.Debug(Strings.Get(world.name) + " spawns within the swarm orbit");
                    SpecialWorldPosition = SpecialMeteorSwarmRelation.beforeSwarmOrbit;
                }
                else if (worldCategoryInt == Hash.SDBMLower("Beached_InsideMeteorSwarm"))
                {
                    Log.Debug(Strings.Get(world.name) + " spawns inside the swarm");
                    SpecialWorldPosition = SpecialMeteorSwarmRelation.inSwarmOrbit;
                }
                else if (worldCategoryInt == Hash.SDBMLower("Beached_AfterMeteorSwarm"))
                {
                    Log.Debug(Strings.Get(world.name) + " spawns beneath the swarm orbit");
                    SpecialWorldPosition = SpecialMeteorSwarmRelation.afterSwarmOrbit;
                }
                else
                    Log.Debug(Strings.Get(world.name) + " follows regular spawning rules");

                return index;
            }
            private static HashSet<AxialI> InsertSwarmBlockings(HashSet<AxialI> bufferSet)
            {
                bufferSet.Add(SwarmOriginLocation);
                switch (SpecialWorldPosition)
                {
                    case SpecialMeteorSwarmRelation.beforeSwarmOrbit: ///inner asteroids that only spawn within the swarm orbit
                        bufferSet.UnionWith(OuterLocations);
                        //bufferSet.UnionWith(SwarmLocations); //use this if you want to allow asteroids directly adjacent to the swarm
                        bufferSet.UnionWith(SwarmLocationsWithBuffer);
                        break;
                    case SpecialMeteorSwarmRelation.afterSwarmOrbit: ///outer asteroids that only spawn outside the swarm orbit
                        bufferSet.UnionWith(InnerLocations);
                        //bufferSet.UnionWith(SwarmLocations); //use this if you want to allow asteroids directly adjacent to the swarm
                        bufferSet.UnionWith(SwarmLocationsWithBuffer);
                        break;
                    case SpecialMeteorSwarmRelation.inSwarmOrbit: ///swarm asteroids that only spawn inside the swarm
                        bufferSet.UnionWith(OuterLocations);
                        bufferSet.UnionWith(InnerLocations);
                        break;
                    default: ///normal items are disallowed to spawn inside the swarm
                        bufferSet.UnionWith(SwarmLocations);
                        break;
                }
                return bufferSet;
            }
            ///called at the start of the poi gen to cleanup the asteroid variable
            static void StartPOIGeneration()
            {
                SpecialWorldPosition = SpecialMeteorSwarmRelation.none;
            }

            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var cluster_worlds = AccessTools.Field(typeof(Cluster), nameof(Cluster.worlds));
                Type worldGenList = typeof(List<ProcGenGame.WorldGen>);
                int getWorldsIndex = codes.FindIndex(ci => ci.LoadsField(cluster_worlds));
                if (getWorldsIndex == -1)
                {
                    Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding worlds index");
                    return codes;
                }
                //grab the worlds iterator current index
                var grabWorldIndex_insertAt = codes.FindIndex(getWorldsIndex, ci => ci.IsLdloc());
                if (grabWorldIndex_insertAt == -1)
                {
                    Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding iterator");
                    return codes;
                }

                var m_GetWorldByIndex = AccessTools.Method(typeof(Cluster_AssignClusterLocations_Patch), "GetWorldByIndex");

                //grabbing the current world index and determine if any special placement rules apply
                codes.InsertRange(grabWorldIndex_insertAt + 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, m_GetWorldByIndex)
                });

                var m_AddSwarmBlockedLocations = AccessTools.Method(typeof(Cluster_AssignClusterLocations_Patch), "InsertSwarmBlockings");

                //regular asteroid placement rules: insert extra blocking locations
                int antiBuffer_stfld_index = codes.FindIndex(ci => ci.opcode == OpCodes.Stfld && ci.operand.ToString().Contains("antiBuffer"));

                if (antiBuffer_stfld_index == -1)
                {
                    Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding antiBuffer");
                    return codes;
                }
                codes.Insert(antiBuffer_stfld_index, new(OpCodes.Call, m_AddSwarmBlockedLocations));

                //reduced asteroid placement rules: insert extra blocking locations
                int minBuffers_stfld_index = codes.FindIndex(ci => ci.opcode == OpCodes.Stfld && ci.operand.ToString().Contains("minBuffers"));

                if (minBuffers_stfld_index == -1)
                {
                    Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding minBuffers");
                    return codes;
                }
                codes.Insert(minBuffers_stfld_index, new(OpCodes.Call, m_AddSwarmBlockedLocations));


                var m_FeatureClusterSpaceEnabled = AccessTools.Method(typeof(DlcManager), "FeatureClusterSpaceEnabled");
                var m_StartPOIGeneration = AccessTools.Method(typeof(Cluster_AssignClusterLocations_Patch), "StartPOIGeneration");
                int clusterSpaceEnabledIndex = codes.FindIndex(ci => ci.Calls(m_FeatureClusterSpaceEnabled));

                if (clusterSpaceEnabledIndex == -1)
                {
                    Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding clusterSpaceEnabledIndex");
                    return codes;
                }
                codes.Insert(clusterSpaceEnabledIndex, new(OpCodes.Call, m_StartPOIGeneration));


                //spacePOI add extra blocking locations, use poiWorldAvoidance because thats used in all 3 poi placement checks
                int poiWorldAvoidance_stfld_index = codes.FindIndex(ci => ci.opcode == OpCodes.Stfld && ci.operand.ToString().Contains("poiWorldAvoidance"));

                if (poiWorldAvoidance_stfld_index == -1)
                {
                    Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding poiWorldAvoidance");
                    return codes;
                }
                codes.Insert(poiWorldAvoidance_stfld_index, new(OpCodes.Call, m_AddSwarmBlockedLocations));

                TranspilerHelper.PrintInstructions(codes);
                return codes;
            }
        }
    }
}
