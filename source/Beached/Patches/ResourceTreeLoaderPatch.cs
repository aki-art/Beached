using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static FUtility.CONSTS.TECH;

namespace Beached.Patches
{
	public class ResourceTreeLoaderPatch
	{
		private const float X = 350, Y = 250;

		[HarmonyPatch(typeof(ResourceTreeLoader<ResourceTreeNode>), MethodType.Constructor, typeof(TextAsset))]
		public class ResourceTreeLoader_Load_Patch
		{
			private static readonly HashSet<string> fileNames = [
				"TechTree_Generated",
				"TechTree_Expansion1_Generated"
				];

			public static void Postfix(ResourceTreeLoader<ResourceTreeNode> __instance, TextAsset file)
			{
				Log.Debug("loading resource tree: " + file.name);
				if (!fileNames.Contains(file.name))
					return;

				// SLag insulation
				//AddNode(__instance, BTechs.ADVANCED_INSULATION_ID, GASES.TEMPERATURE_MODULATION, GASES.DIRECTED_AIR_STREAMS, GASES.HVAC);

				foreach (var item in __instance)
				{
					if (item.Id == BTechs.HYDRO_ELECTRONICS)
						return;
				}

				foreach (var item in __instance)
				{
					if (item.Id == SOLIDS.BASIC_REFINEMENT)
					{
						Log.Debug($"basic refinement data");
						Log.Debug($"edges");
						foreach (var edge in item.edges)
						{
							Log.Debug($"edgeType: {edge.edgeType} \n" +
								$"\t\tsource: {edge.source?.Name} offset: {edge.sourceOffset} " +
								$"\t\ttarget: {edge.target?.Name} offset: {edge.targetOffset}");
						}
						Log.Debug(item.edges);
						break;
					}
				}
;
				// Add Power Regulation → Kinetics → Power Regulation
				AddNode(
					__instance,
					BTechs.HYDRO_ELECTRONICS,
					POWER.POWER_REGULATION,
					1,
					-1,
					POWER.POWER_REGULATION,
					[POWER.ADVANCED_POWER_REGULATION]);

				// Add Materials I → Brute-Force Refinement
				AddNode(
					__instance,
					BTechs.MATERIALS1,
					COLONY_DEVELOPMENT.JOBS,
					0,
					0,
					null,
					[SOLIDS.BASIC_REFINEMENT],
			SOLIDS.BASIC_REFINEMENT);

				// remove Jobs → Brute-Force Refinement
				RemoveRequirement(__instance, COLONY_DEVELOPMENT.JOBS, SOLIDS.BASIC_REFINEMENT);
			}

			private static void RemoveRequirement(ResourceTreeLoader<ResourceTreeNode> instance, string parentId, string childId)
			{
				var parent = GetNode(instance, parentId);
				var child = GetNode(instance, childId);

				parent.references.Remove(child);
				parent.edges.RemoveAll(node => node.target?.Id == childId);
				child.edges.RemoveAll(node => node.source?.Id == parentId);
			}

			private static ResourceTreeNode GetNode(ResourceTreeLoader<ResourceTreeNode> nodes, string id)
			{
				foreach (var node in nodes)
				{
					if (node.Id == id)
						return node;
				}

				return null;
			}

			private static ResourceTreeNode AddNode(
				ResourceTreeLoader<ResourceTreeNode> nodes,
				string techId,
				string xRefNodeId,
				int xOffset,
				int yOffset,
				string requiredTech = null,
				string[] unlockedTechs = null,
				string yRefNodeId = null)
			{
				float x = 0, y = 0;

				var xRefNode = GetNode(nodes, xRefNodeId);

				if (xRefNode != null)
				{
					x = xRefNode.nodeX + (X * xOffset);
					y = xRefNode.nodeY + (Y * yOffset);
					Log.Debug($"found {xRefNodeId} y is now at {y}");
				}

				if (xRefNode == null)
					return null;

				if (yRefNodeId != null)
				{
					var yRefNode = GetNode(nodes, yRefNodeId);

					if (yRefNode != null)
					{
						y = yRefNode.nodeY + (Y * yOffset);
						Log.Debug($"found {yRefNodeId} y is now at {y}");
					}
				}

				var references = new List<ResourceTreeNode>();

				if (unlockedTechs != null)
				{
					foreach (var tech in unlockedTechs)
					{
						// references only want children
						references.Add(GetNode(nodes, tech));
					}
				}

				var node = new ResourceTreeNode
				{
					height = xRefNode.height,
					width = xRefNode.width,
					nodeX = x,
					nodeY = y,
					edges = [], // edfes need to reference the node itself, so we create this list later
					references = references,
					Disabled = false,
					Id = techId,
					Name = techId
				};

				var edges = new List<ResourceTreeNode.Edge>();

				if (unlockedTechs != null)
				{
					foreach (var tech in unlockedTechs)
					{
						// edges are ONE WAY, parent → child
						edges.Add(new ResourceTreeNode.Edge(node, GetNode(nodes, tech), ResourceTreeNode.Edge.EdgeType.PolyLineEdge));
					}
				}

				node.edges = edges;

				// modifying the parent of this tech
				if (requiredTech != null)
				{
					var required = GetNode(nodes, requiredTech);
					// references only want to know their children
					required.references.Add(node);
					// edges want a one way parent → child connection
					required.edges.Add(new ResourceTreeNode.Edge(required, node, ResourceTreeNode.Edge.EdgeType.PolyLineEdge));
				}

				nodes.resources.Add(node);

				Log.Debug($"added {techId} node");
				return node;
			}
		}
	}
}
