using Beached.Content.ModDb;
using UnityEngine;
using static FUtility.CONSTS.TECH;

namespace Beached.Patches
{
	public class ResourceTreeLoaderPatch
	{
		private const float X = 350, Y = 250;

		// CRASH TEST
		//	[HarmonyPatch(typeof(ResourceTreeLoader<ResourceTreeNode>), MethodType.Constructor, typeof(TextAsset))]
		public class ResourceTreeLoader_Load_Patch
		{
			public static void Postfix(ResourceTreeLoader<ResourceTreeNode> __instance, TextAsset file)
			{
				Log.Debug("loading resource tree: " + file.name);
				if (file.name != "TechTree_Expansion1_Generated")
					return;

				// SLag insulation
				//AddNode(__instance, BTechs.ADVANCED_INSULATION_ID, GASES.TEMPERATURE_MODULATION, GASES.DIRECTED_AIR_STREAMS, GASES.HVAC);

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
			}
		}
	}
}
