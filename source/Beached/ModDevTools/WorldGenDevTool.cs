using Delaunay.Geo;
using HarmonyLib;
using ImGuiNET;
using Klei;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Beached.ModDevTools
{
    public class WorldGenDevTool : DevTool
    {
        private WorldContainer worldContainer;
        public static OfflineWorldGen offlineWorldGen;
        private static float progress = 0;
        private static int testingSeed = 0;
        private static Cluster cluster;
        private WorldLayoutHelper layoutHelper;
        private WorldGen layoutPreviewWorldgen;
        public unsafe static Vector2* testPoints;

        public WorldGenDevTool()
        {
        }

        public override void RenderTo(DevPanel panel)
        {
            ImGui.TextColored(Color.red, "DESTRUCTIVE AND SLOW DEBUG TOOLS, DO NOT USE IN NORMAL GAMEPLAY");
            ImGui.Spacing();

            ImGui.InputInt("Seed", ref testingSeed);

            if (ImGui.Button("Delete world contents"))
            {
                worldContainer = ClusterManager.Instance.activeWorld;
                CleanWorld();
            }

            if (ImGui.Button("Generate"))
            {
                GenerateNewWorld();
            }

            if (ImGui.Button("Offlineworldgen Generate"))
            {
                if (offlineWorldGen == null)
                {
                    ImGui.Text("null offlineWorldgen");
                    return;
                }
                else
                {
                    offlineWorldGen.debug = true;
                    offlineWorldGen.Generate();
                }
            }

            if (ImGui.Button("Reload Worldgen"))
            {
                WorldGen.LoadSettings(false);

                cluster = Cluster.Load();
                SettingsCache.LoadFiles(new List<YamlIO.Error>());
            }

            else ImGui.Text("Layout cache is null");

            if (ImGui.Button("Generate Layout"))
            {
                layoutHelper ??= new WorldLayoutHelper();
                layoutPreviewWorldgen = layoutHelper.Generate(testingSeed);
            }

            //ImDrawListPtr drawList = ImGui.GetWindowDrawList();
            var p = ImGui.GetCursorScreenPos();
            var testPoints = new[]
            {
                new Vector2(0, 0) + p,
                new Vector2(0, 100) + p,
                new Vector2(100, 100) + p,
                new Vector2(100, 0) + p
            };

            ImGui.GetWindowDrawList().AddConvexPolyFilled(ref testPoints[0], 4, ToUint(Color.red));
            /*
                        for (int i = 0; i < testPoints.Length; i++)
                        {
                            drawList.AddConvexPolyFilled(ref testPoints[i], 5, ToUint(Color.red));
                        }
            */
/*
            ImGui.GetWindowDrawList().AddTriangleFilled(
                new Vector2(0, 0),
                new Vector2(100, 0),
                new Vector2(100, 100),
                ToUint(Color.blue));*/

            if (layoutPreviewWorldgen != null && ImGui.Button("Save Image"))
            {
                DisplayLayout();

            }
        }

        private unsafe static ImDrawListPtr draw()
        {
            var drawList = ImGui.GetWindowDrawList();
            //IntPtr mem = Marshal.AllocCoTaskMem(4);
            //testPoints = (Vector2*)mem;

            var c = (Color32)Color.red;
            var colori = (uint)(((c.a << 24) | (c.r << 16) | (c.g << 8) | c.b) & 0xffffffffL); ;
            return drawList;
        }

        private void DisplayLayout()
        {
            var image = new Texture2D(224, 224, TextureFormat.ARGB32, false);

            Log.Debug(layoutPreviewWorldgen.TerrainCells.Count);

            foreach (var terrainCell in layoutPreviewWorldgen.TerrainCells)
            {
                Log.Debug("drawing terraincell: " + terrainCell.GetSubWorldType(layoutPreviewWorldgen));
                for (int x = 0; x < 224; x++)
                {
                    for (int y = 0; y < 224; y++)
                    {
                        if (terrainCell.poly.Contains(new Vector2(x, y)))
                        {
                            var subworld = terrainCell.GetSubWorldType(layoutPreviewWorldgen);
                            var subworldData = SettingsCache.subworlds[subworld];
                            if (subworldData == null)
                            {
                                Debug.Log("no zonetype for " + subworld);
                                continue;
                            }

                            var color = World.Instance.zoneRenderData.zoneColours[(int)subworldData.zoneType];
                            image.SetPixel(x, y, color);
                        }
                    }
                }
            }

            image.Apply();
            ModAssets.SaveImage(image, "layout");
        }

        private static uint ToUint(Color32 c)
        {
            return (uint)(((c.a << 24) | (c.r << 16) | (c.g << 8) | c.b) & 0xffffffffL);
        }

        private bool UpdateProgress(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
        {
            progress = completePercent;
            return true;
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
            CancelTool.Instance.OnDragComplete(worldContainer.minimumBounds, worldContainer.maximumBounds);
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
                        CancelTool.Instance.OnDragTool(cell, 0);

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
            World.Instance.zoneRenderData.OnShadersReloaded();
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
                if (portal.GetTargetWorldID() == worldContainer.id)
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
