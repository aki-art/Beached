﻿#if TRANSPILERS
using Beached.Content.BWorldGen;
using Beached.Content.Defs.StarmapEntities;
using HarmonyLib;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using static ProcGen.WorldPlacement;

namespace Beached.Patches.Worldgen
{
	public class ClusterPatches
	{
		public static bool allowSpawnAdjacentSpawn = false;

		[HarmonyPatch(typeof(Cluster), nameof(Cluster.AssignClusterLocations))]
		public class Cluster_AssignClusterLocations_Patch
		{
			[HarmonyPrepare]
			public static bool Prepare() => true;

			private static LocationType specialWorldPosition = LocationType.Cluster;

			// Hexes that are considered "inside the swarm"
			private static HashSet<AxialI> swarmLocations;

			// Hexes that are considered "inside or directly adjaced to swarm"
			private static HashSet<AxialI> swarmLocationsWithBuffer;

			// Hexes that are inside of the swarm orbit
			private static HashSet<AxialI> innerLocations;

			// Hexes that are outside of the swarm orbit
			private static HashSet<AxialI> outerLocations;

			private static AxialI swarmOriginLocation;

			private static void Prefix(Cluster __instance)
			{
				PopulateSwarmLocationData(__instance);
			}

			private static void Postfix(Cluster __instance)
			{
				Log.Debug($"cluster init id: {__instance.Id}");
				Log.Debug($"cluster init id: {__instance.clusterLayout.name}");
				if (!WorldgenUtil.IsBeachedWorld(__instance.clusterLayout.name))
					return;

				__instance.poiPlacements.Add(swarmOriginLocation, MeteorSwarmVisualPOIConfig.ID);

				HashSet<AxialI> worldLocations = [];

				foreach (WorldGen world in __instance.worlds)
					worldLocations.Add(world.data.clusterLocation);

				foreach (AxialI swarmLocation in swarmLocations)
				{
					if (!worldLocations.Contains(swarmLocation))
						__instance.poiPlacements.Add(swarmLocation, MeteorSwarmDamagePOIConfig.ID);
				}
			}

			public static AxialI GetAsteroidSwarmOrigin(int seed, int rings)
			{
				//the 6 corner locations of the cluster
				AxialI[] beachedBeltSources =
				[
					new (0,rings),
					new (0,-rings),
					new (-rings,0),
					new (rings,0),
					new (rings,-rings),
					new (-rings,rings),
				];

				return beachedBeltSources.GetRandom(new SeededRandom(seed));
			}

			private static void PopulateSwarmLocationData(Cluster __instance)
			{
				swarmLocations = [];
				innerLocations = [];
				outerLocations = [];

				int rings = __instance.numRings - 1;
				swarmOriginLocation = GetAsteroidSwarmOrigin(__instance.seed, rings);

				AxialI center = new AxialI(0, 0);
				Vector3 swarmOriginPos = AxialUtil.AxialToWorld(swarmOriginLocation.R, swarmOriginLocation.Q);
				Vector3 centerPos = AxialUtil.AxialToWorld(0, 0);
				float worldRadius = Vector3.Distance(centerPos, swarmOriginPos); //grid radius in world coordinates

				// iterate all hexes of the cluster and assign them to one of the 3 belt location types
				foreach (AxialI location in AxialUtil.GetAllPointsWithinRadius(center, rings))
				{
					Vector3 locationPoint = AxialUtil.AxialToWorld(location.R, location.Q);
					float distanceF = Vector3.Distance(swarmOriginPos, locationPoint);

					int distance = Mathf.RoundToInt(distanceF);

					if (distance > Mathf.RoundToInt(worldRadius * 1.5f) + 1)
					{
						outerLocations.Add(location);
						continue;
					}

					if (distance < Mathf.RoundToInt(worldRadius * 1.5f) - 1)
					{
						innerLocations.Add(location);
						continue;
					}

					swarmLocations.Add(location);
				}

				swarmLocationsWithBuffer = [];

				foreach (AxialI location in swarmLocations)
					swarmLocationsWithBuffer.UnionWith(AxialUtil.GetAllPointsWithinRadius(location, 1));

				swarmLocationsWithBuffer = new(swarmLocationsWithBuffer.Distinct());
			}

			private static int GetWorldByIndex(int index, Cluster instance)
			{
				Debug.Log("world index: " + index);

				WorldPlacement worldPlacement = SettingsCache.clusterLayouts.clusterCache[instance.Id].worldPlacements[index];
				Debug.Log("World locationType to int: " + worldPlacement.locationType);

				specialWorldPosition = worldPlacement.locationType;

				return index;
			}

			private static HashSet<AxialI> InsertSwarmBlockings(HashSet<AxialI> bufferSet)
			{
				bufferSet.Add(swarmOriginLocation);

				if (specialWorldPosition == BLocationTypes.BeforeMeteorSwarm)
				{
					bufferSet.UnionWith(outerLocations);
					if (allowSpawnAdjacentSpawn)
						bufferSet.UnionWith(swarmLocations);
					bufferSet.UnionWith(swarmLocationsWithBuffer);
				}
				else if (specialWorldPosition == BLocationTypes.AfterMeteorSwarm)
				{
					bufferSet.UnionWith(innerLocations);
					if (allowSpawnAdjacentSpawn)
						bufferSet.UnionWith(swarmLocations);
					bufferSet.UnionWith(swarmLocationsWithBuffer);
				}
				else if (specialWorldPosition == BLocationTypes.InsideMeteorSwarm)
				{
					bufferSet.UnionWith(outerLocations);
					bufferSet.UnionWith(innerLocations);
				}
				else
					///normal items are disallowed to spawn inside the swarm
					bufferSet.UnionWith(swarmLocations);

				return bufferSet;
			}

			///called at the start of the poi gen to cleanup the asteroid variable
			private static void StartPOIGeneration() => specialWorldPosition = LocationType.Cluster;

			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				List<CodeInstruction> codes = orig.ToList();

				System.Reflection.FieldInfo cluster_worlds = AccessTools.Field(typeof(Cluster), nameof(Cluster.worlds));
				System.Type worldGenList = typeof(List<WorldGen>);
				int getWorldsIndex = codes.FindIndex(ci => ci.LoadsField(cluster_worlds));

				if (getWorldsIndex == -1)
				{
					Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding worlds index");
					return codes;
				}

				//grab the worlds iterator current index
				int grabWorldIndex_insertAt = codes.FindIndex(getWorldsIndex, ci => ci.IsLdloc());

				if (grabWorldIndex_insertAt == -1)
				{
					Log.Warning("Cluster.AssignClusterLocations Transpiler failed at finding iterator");
					return codes;
				}

				System.Reflection.MethodInfo m_GetWorldByIndex = AccessTools.Method(typeof(Cluster_AssignClusterLocations_Patch), "GetWorldByIndex");

				//grabbing the current world index and determine if any special placement rules apply
				codes.InsertRange(grabWorldIndex_insertAt + 1,
				[
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_GetWorldByIndex)
				]);

				System.Reflection.MethodInfo m_AddSwarmBlockedLocations = AccessTools.Method(typeof(Cluster_AssignClusterLocations_Patch), nameof(InsertSwarmBlockings));

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


				System.Reflection.MethodInfo m_FeatureClusterSpaceEnabled = AccessTools.Method(typeof(DlcManager), "FeatureClusterSpaceEnabled");
				System.Reflection.MethodInfo m_StartPOIGeneration = AccessTools.Method(typeof(Cluster_AssignClusterLocations_Patch), "StartPOIGeneration");

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

				return codes;
			}
		}
	}
}

#endif