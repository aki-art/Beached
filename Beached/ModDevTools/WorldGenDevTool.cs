using Delaunay.Geo;
using HarmonyLib;
using ImGuiNET;
using Klei;
using ProcGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.ModDevTools
{
    public class WorldGenDevTool : DevTool
    {
        private string testString = "test";
        private Traverse CancelTool_OnDragTool;
        private Traverse CancelTool_OnDragComplete;
        private Traverse World_zoneRenderData_OnShadersReloaded;
        private WorldContainer worldContainer;

        public WorldGenDevTool()
        {
            CancelTool_OnDragTool = Traverse.Create(CancelTool.Instance).Method("OnDragTool", new Type[] { typeof(int), typeof(int) });
            CancelTool_OnDragComplete = Traverse.Create(CancelTool.Instance).Method("OnDragComplete", new Type[] { typeof(Vector3), typeof(Vector3) });
            World_zoneRenderData_OnShadersReloaded = Traverse.Create(World.Instance.zoneRenderData).Method("OnShadersReloaded");
        }

        protected override void RenderTo(DevPanel panel)
        {
            ImGui.TextColored(Color.red, "DESTRUCTIVE AND SLOW DEBUG TOOLS, DO NOT USE IN NORMAL GAMEPLAY");
            ImGui.Spacing();

            if (ImGui.Button("Delete world contents"))
            {
                worldContainer = ClusterManager.Instance.activeWorld;
                CleanWorld();
            }

            if (ImGui.Button("Generate"))
            {
                GenerateNewWorld();
            }
        }

        private void GenerateNewWorld()
        {
            worldContainer = ClusterManager.Instance.activeWorld;
            CleanWorld();
        }

        public void CleanWorld()
        {
            var minX = (int)worldContainer.minimumBounds.x;
            var maxX = (int)worldContainer.maximumBounds.x;
            var minY = (int)worldContainer.minimumBounds.y;
            var maxY = (int)worldContainer.maximumBounds.y;

            DeleteWarpPortalLeadingHere();
            CancelOrders();
            DeleteBuildings(worldContainer);
            RemoveCells(minX, maxX, minY, maxY);

            RemoveWorldZones();
            GameScheduler.Instance.StartCoroutine(KeepTryingToDeletePickupables());
        }

        // Keeps attempting to delete all contents of a world until there is nothing left (or we ran out of max attempts)
        private IEnumerator KeepTryingToDeletePickupables()
        {
            bool success = false;
            var deleteAttempts = 0;

            while (!success && deleteAttempts++ < 16)
            {
                if (Components.Pickupables.GetWorldItems(ClusterManager.Instance.activeWorldId).Count == 0)
                {
                    Log.Info($"World {ClusterManager.Instance.activeWorld.worldName} was destroyed. It took {deleteAttempts} iterations.");

                    success = true;

                    yield break;
                }

                DeletePickupables(ClusterManager.Instance.activeWorld);

                yield return new WaitForSeconds(0.2f);
            }

            if (!success)
            {
                Log.Warning($"World {ClusterManager.Instance.activeWorld.worldName} could not be fully destroyed, giving up. (is something creating an infinitely respawning item?).");
                Log.Warning(string.Join(", ", Components.Pickupables.GetWorldItems(ClusterManager.Instance.activeWorldId)));
            }

            yield return null;
        }

        private void CancelOrders()
        {
            CancelTool_OnDragComplete.GetValue((Vector3)worldContainer.minimumBounds, (Vector3)worldContainer.maximumBounds);
        }

        private void RemoveCells(int minX, int maxX, int minY, int maxY)
        {
            var entombedItemVis = Game.Instance.GetComponent<EntombedItemVisualizer>();

            for (int x = minX + 1; x < maxX; x++)
            {
                for (int y = minY + 1; y < maxY; y++)
                {
                    int cell = Grid.XYToCell(x, y);
                    if (Grid.IsValidCellInWorld(cell, worldContainer.id))
                    {
                        CancelTool_OnDragTool.GetValue(cell, 0);

                        if (entombedItemVis.IsEntombedItem(cell))
                        {
                            entombedItemVis.RemoveItem(cell);
                        }

                        DestroyCell(cell);
                    }
                }
            }
        }

        private void RemoveWorldZones()
        {
            var overworldCell = new WorldDetailSave.OverworldCell
            {
                poly = new Polygon(new Rect(worldContainer.WorldOffset, worldContainer.WorldSize)),
                zoneType = SubWorld.ZoneType.Space
            };

            SaveLoader.Instance.clusterDetailSave.overworldCells.Add(overworldCell);

            worldContainer.ClearWorldZones();
            World_zoneRenderData_OnShadersReloaded.GetValue();
        }

        public void DestroyCell(int cell)
        {
            var objects = new List<GameObject>
            {
                Grid.Objects[cell, (int)ObjectLayer.Backwall],
                Grid.Objects[cell, (int)ObjectLayer.Wire],
                Grid.Objects[cell, (int)ObjectLayer.Building],
                Grid.Objects[cell, (int)ObjectLayer.GasConduit],
                Grid.Objects[cell, (int)ObjectLayer.LiquidConduit],
                Grid.Objects[cell, (int)ObjectLayer.Minion],
                Grid.Objects[cell, (int)ObjectLayer.SolidConduit],
                Grid.Objects[cell, (int)ObjectLayer.FoundationTile],
                Grid.Objects[cell, (int)ObjectLayer.LogicWire],
                Grid.Objects[cell, (int)ObjectLayer.Pickupables]
            };

            foreach (GameObject gameObject in objects)
            {
                if (gameObject != null)
                {
                    UnityEngine.Object.Destroy(gameObject);
                }
            }

            DebugTool.Instance.ClearCell(cell);

            if (ElementLoader.elements[Grid.ElementIdx[cell]].id != SimHashes.Void)
            {
                SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
            }
        }

        private void DeleteWarpPortalLeadingHere()
        {
            foreach (WarpPortal portal in UnityEngine.Object.FindObjectsOfType<WarpPortal>())
            {
                int targetWorldId = Traverse.Create(portal).Method("GetTargetWorldID").GetValue<int>();
                if (targetWorldId == worldContainer.id)
                {
                    portal.CancelAssignment();
                    UnityEngine.Object.Destroy(portal);
                }
            }
        }

        private void DeleteBuildings(WorldContainer world)
        {
            ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
            GameScenePartitioner.Instance.GatherEntries((int)world.minimumBounds.x, (int)world.minimumBounds.y, world.Width, world.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);

            foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is BuildingComplete buildingComplete)
                {
                    buildingComplete.DeleteObject();
                }
            }
            pooledList.Clear();
        }

        private void DeletePickupables(WorldContainer world)
        {
            ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
            GameScenePartitioner.Instance.GatherEntries((int)world.minimumBounds.x, (int)world.minimumBounds.y, world.Width, world.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);

            foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is Pickupable pickupable)
                {
                    pickupable.DeleteObject();
                }
            }
            pooledList.Clear();
        }
    }
}
