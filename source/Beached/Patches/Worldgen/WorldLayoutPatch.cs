using Beached.Content.BWorldGen;
using Delaunay.Geo;
using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;
using VoronoiTree;

namespace Beached.Patches.Worldgen
{
    public class WorldLayoutPatch
    {
        [HarmonyPatch(typeof(WorldLayout), "TagTopAndBottomSites")]
        public class WorldLayout_TagTopAndBottomSites_Patch
        {
            private static bool initialized;
            private static LineSegment bottomLeftEdge;
            private static LineSegment bottomRightEdge;
            private static LineSegment bottomLeftCorner;
            private static LineSegment bottomRightCorner;

            private const float MARGIN = 5f;

            public static void Postfix(WorldLayout __instance)
            {
                var voronoiTree = __instance.voronoiTree;

                // TODO: move this out to some class to handle
                if (!initialized)
                {
                    bottomLeftEdge = new(
                        new Vector2?(new Vector2(0, MARGIN)),
                        new Vector2?(new Vector2(__instance.mapWidth / 2f, MARGIN)));

                    bottomRightEdge = new(
                        new Vector2?(new Vector2(__instance.mapWidth / 2f, MARGIN)),
                        new Vector2?(new Vector2(__instance.mapWidth, MARGIN)));

                    bottomLeftCorner = new(
                        new Vector2?(new Vector2(0, MARGIN)),
                        new Vector2?(new Vector2(MARGIN, MARGIN)));

                    bottomRightCorner = new(
                        new Vector2?(new Vector2(__instance.mapWidth - MARGIN, MARGIN)),
                        new Vector2?(new Vector2(__instance.mapWidth, MARGIN)));

                    initialized = true;
                }

                MarkTags(voronoiTree, bottomRightEdge, BWorldGenTags.AtSideADepth);
                MarkTags(voronoiTree, bottomLeftEdge, BWorldGenTags.AtSideBDepth);
                //MarkTags(voronoiTree, bottomRightCorner, BWorldGenTags.AtSideACorner);
                //MarkTags(voronoiTree, bottomLeftCorner, BWorldGenTags.AtSideBCorner);
            }

            private static void MarkTags(Tree voronoiTree, LineSegment edge, Tag tag)
            {
                var bottomRightSites = new List<Diagram.Site>();
                voronoiTree.GetIntersectingLeafSites(edge, bottomRightSites);

                foreach (var site in bottomRightSites)
                {
                    voronoiTree.GetNodeForSite(site).AddTag(tag);
                }
            }
        }
    }
}
