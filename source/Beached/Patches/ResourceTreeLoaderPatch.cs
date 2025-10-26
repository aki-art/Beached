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

				AddNode(
					__instance,
					BTechs.HYDRO_ELECTRONICS,
					POWER.POWER_REGULATION,
					1,
					-1);
			}

			private static void AddNode(ResourceTreeLoader<ResourceTreeNode> nodes, string techId, string previousNode, int xOffset, int yOffset)
			{
				ResourceTreeNode tempModNode = null;
				float x = 0, y = 0;

				foreach (var item in nodes)
				{
					if (item.Id == previousNode)
					{
						tempModNode = item;
						x = tempModNode.nodeX + (X * xOffset);
						y = tempModNode.nodeY + (Y * yOffset);
					}
				}
;
				if (tempModNode == null)
					return;

				var node = new ResourceTreeNode
				{
					height = tempModNode.height,
					width = tempModNode.width,
					nodeX = x,
					nodeY = y,
					edges = [.. tempModNode.edges],
					references = [],
					Disabled = false,
					Id = techId,
					Name = techId
				};


				tempModNode.references.Add(node);
				nodes.resources.Add(node);

				Log.Debug("added hydro node");
			}
		}
	}
}
