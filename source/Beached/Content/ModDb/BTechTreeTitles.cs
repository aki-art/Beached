using Database;
using System.Linq;

namespace Beached.Content.ModDb
{
	public class BTechTreeTitles
	{
		public const string UNKNOWN_ID = "Beached_TechTree_Unknown";

		public static void Register(TechTreeTitles techTreeTitles)
		{
			var lowestY = techTreeTitles.resources.OrderBy(t => t.node.nodeY).First();

			var unknownNode = new ResourceTreeNode()
			{
				nodeX = lowestY.node.nodeX,
				// TODO: this is hardcoded coords, not ideal for compatibility
				nodeY = lowestY.node.nodeY - lowestY.height * 8f, // Y down
				height = lowestY.node.height,
				width = lowestY.node.width,
				Id = UNKNOWN_ID,
				Name = STRINGS.RESEARCH.TREES.TITLE_UNKNOWN
			};

			new TechTreeTitle(UNKNOWN_ID, techTreeTitles, STRINGS.RESEARCH.TREES.TITLE_UNKNOWN, unknownNode);
		}
	}
}
